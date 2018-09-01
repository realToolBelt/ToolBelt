using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Profile
{
    public class EditableProfilePageViewModel : BaseViewModel
    {
        private readonly Plugin.Media.Abstractions.PickMediaOptions _pickOptions =
            new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            };

        private Stream _photo;

        public EditableProfilePageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Edit Profile";

            AddValidationRules();

            NavigatedTo
                .Take(1)
                .Select(args => args["user"] as User)
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
                        // check if we should show a rationale for requesting permissions
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos))
                        {
                            await dialogService.DisplayAlertAsync("Need photo permissions", "Need access to photos", "OK");
                        }

                        // request the permissions we need
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);
                        if (results.ContainsKey(Permission.Photos))
                        {
                            status = results[Permission.Photos];
                        }
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
                        // permission was not granted.  Let the user know they can't pick a photo without permissions
                        await dialogService.DisplayAlertAsync("Photos Not Supported", ":( Permission not granted to photos.", "OK").ConfigureAwait(false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // TODO: log the exception...
                    await dialogService.DisplayAlertAsync("AnError", "An error has occurred.", "OK").ConfigureAwait(false);
                    return;
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
                NavigationParameters args = new NavigationParameters();
                args.Add(
                    "items",
                    (await projectDataStore.GetTradeSpecialtiesAsync()).Select(specialty => new SelectionViewModel<TradeSpecialty>(specialty)
                    {
                        DisplayValue = specialty.Name
                    }));

                await NavigationService.NavigateAsync(nameof(MultiSelectListViewPage), args).ConfigureAwait(false);
            });
        }

        public ValidatableObject<DateTime?> BirthDate { get; } = new ValidatableObject<DateTime?>();

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

            BirthDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "Birth Date cannot be empty" });
        }

        private bool IsValid()
        {
            // NOTE: Validate each control individually so we get error indicators for all
            Email.Validate();
            FirstName.Validate();
            LastName.Validate();
            Phone.Validate();
            BirthDate.Validate();

            return Email.IsValid
                && FirstName.IsValid
                && LastName.IsValid
                && Phone.IsValid
                && BirthDate.IsValid;
        }
    }
}
