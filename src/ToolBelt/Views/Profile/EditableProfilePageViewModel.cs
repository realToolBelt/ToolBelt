using Plugin.Media;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
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
            IPageDialogService dialogService) : base(navigationService)
        {
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
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await dialogService.DisplayAlertAsync("Photos Not Supported", ":( Permission not granted to photos.", "OK").ConfigureAwait(false);
                    return;
                }

                using (var file = await CrossMedia.Current.PickPhotoAsync(_pickOptions))
                {
                    if (file != null)
                    {
                        Photo = file.GetStream();
                    }
                }
            });

            SelectCommunities = ReactiveCommand.CreateFromTask(async () =>
            {
                NavigationParameters args = new NavigationParameters();
                args.Add("items", new[]
                {
                    "Laborer",
                    "Carpenter",
                    "Construction manager",
                    "Painter",
                    "Admin support",
                    "Plumber",
                    "Professional",
                    "Heat A/C mech",
                    "Operating engineer",
                    "Repairer",
                    "Manager",
                    "Electrician",
                    "Roofer",
                    "Truck driver",
                    "Brickmason",
                    "Foreman",
                    "Service",
                    "Drywall",
                    "Welder",
                    "Carpet and tile",
                    "Material moving",
                    "Concrete",
                    "Ironworker",
                    "Helper",
                    "Insulation",
                    "Sheet metal",
                    "Fence Erector",
                    "Highway Maint",
                    "Misc worker",
                    "Inspector",
                    "Glazier",
                    "Plasterer",
                    "Dredge",
                    "Power-line installer",
                    "Driller",
                    "Elevator",
                    "Paving",
                    "Iron reinforcement",
                    "Boilermaker",
                    "Other"
                }
                .Select(community => new SelectionViewModel<string>(community)));

                await NavigationService.NavigateAsync(nameof(MultiSelectListViewPage), args).ConfigureAwait(false);
            });
        }

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

        public ReactiveCommand SelectCommunities { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue("selected_items", out IEnumerable items))
            {
                Communities.Reset(items.OfType<SelectionViewModel>().Select(item => item.Item.ToString()));
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
