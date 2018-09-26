using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Profile
{
    public class EditableProfilePageViewModel : BaseViewModel
    {
        private readonly PickMediaOptions _pickOptions =
            new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            };

        private Stream _photo;

        public EditableProfilePageViewModel(
            INavigationService navigationService,
            IUserDialogs dialogService,
            IProjectDataStore projectDataStore,
            IPermissionsService permissionsService) : base(navigationService)
        {
            Title = "Edit Profile";

            AddValidationRules();

            NavigatedTo
                .Take(1)
                .Select(args => args["user"] as Account)
                .Subscribe(user =>
                {
                    // TODO: Handle null user, handle editing...
                });

            ChangePhoto = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    // check if permission has been granted to the photos
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                    if (status != PermissionStatus.Granted)
                    {
                        status = await permissionsService.CheckPermissionsAsync(Permission.Photos, dialogService);
                    }

                    // if permission was granted, open the photo picker
                    if (status == PermissionStatus.Granted)
                    {
                        using (var file = await CrossMedia.Current.PickPhotoAsync(_pickOptions))
                        {
                            if (file != null)
                            {
                                Photo = file.GetStream();
                            }
                        }
                    }
                    else
                    {
                        // permission was not granted. Let the user know they can't pick a photo
                        // without permissions
                        await dialogService.AlertAsync(
                            new AlertConfig
                            {
                                Title = "Photos Not Supported",
                                Message = ":( Permission not granted to photos.",
                                OkText = "OK"
                            }).ConfigureAwait(false);

                        return;
                    }
                }
                catch (MediaPermissionException mediaException)
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Permission Error",
                            Message = $"Permissions not granted: {string.Join(", ", mediaException.Permissions)}",
                            OkText = "OK"
                        }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // TODO: log the exception...
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Error",
                            Message = "An error has occurred.",
                            OkText = "OK"
                        }).ConfigureAwait(false);
                }
            });

            Save = ReactiveCommand.CreateFromTask(async () =>
            {
                // TODO:...
            });

            Cancel = ReactiveCommand.CreateFromTask(async () =>
            {
                await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
            });

            SelectCommunities = ReactiveCommand.CreateFromTask(async () =>
            {
                var args = new NavigationParameters();
                args.Add(
                    "items",
                    (await projectDataStore.GetTradesAsync()).Select(specialty => new SelectionViewModel<Trade>(specialty)
                    {
                        DisplayValue = specialty.Name
                    }));

                await NavigationService.NavigateAsync(nameof(MultiSelectListViewPage), args).ConfigureAwait(false);
            });
        }


        public ReactiveCommand Cancel { get; }

        public ReactiveCommand ChangePhoto { get; }

        public ReactiveList<string> Communities { get; } = new ReactiveList<string>();

        public ValidatableObject<string> Email { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> FirstName { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> LastName { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> Phone { get; } = new ValidatableObject<string>();

        public Stream Photo
        {
            // TODO: Dispose photo when viewmodel goes out of scope
            get => _photo;
            private set
            {
                // dispose of any current photo after setting the new one
                using (_photo)
                {
                    this.RaiseAndSetIfChanged(ref _photo, value);
                }
            }
        }

        public ReactiveCommand Save { get; }

        public ReactiveCommand SelectCommunities { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue("selected_items", out IEnumerable items))
            {
                Communities.Reset(items.OfType<SelectionViewModel>().Select(item => item.DisplayValue));
                this.RaisePropertyChanged(nameof(Communities));
            }
        }

        private void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Email cannot be empty" });
            Email.Validations.Add(new EmailRule { ValidationMessage = "Email should be an email address" });

            FirstName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "First Name cannot be empty" });
            LastName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Last Name cannot be empty" });
            Phone.Validations.Add(new PhoneRule { ValidationMessage = "Invalid phone number" });
        }

        private bool IsValid()
        {
            // NOTE: Validate each control individually so we get error indicators for all
            Email.Validate();
            FirstName.Validate();
            LastName.Validate();
            Phone.Validate();

            return Email.IsValid
                && FirstName.IsValid
                && LastName.IsValid
                && Phone.IsValid;
        }
    }
}
