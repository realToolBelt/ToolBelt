using Prism.Navigation;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Services.Analytics;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication.Registration
{
    public class ContractorRegistrationPageViewModel : BaseViewModel
    {
        private readonly IEnumerable<IValidity> _validatableFields;
        private string _userId;

        public ContractorRegistrationPageViewModel(
            INavigationService navigationService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "Contractor";
            analyticService.TrackScreen(
                "registration-page",
                new Dictionary<string, string>
                {
                    { "registration-type", "contractor" }
                });

            // store the fields in an enumerable for easy use later on in this class
            _validatableFields = new IValidity[]
            {
                CompanyName,
                CompanyEmail,
                CompanyUrl,
                AddressLineOne,
                AddressLineTwo,
                City,
                State,
                Zip,
                PrimaryPhone,
                SecondaryPhone,
                SocialNetwork1,
                SocialNetwork2,
                SocialNetwork3,
                PrimaryContact
            };

            AddValidationRules();

            NavigatingTo
                .Where(args => args.ContainsKey("user_id"))
                .Select(args => args["user_id"].ToString())
                .BindTo(this, x => x._userId);

            Next = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!IsValid())
                {
                    return;
                }

                var contractor = new ContractorAccount
                {
                    AccountId = _userId,
                    EmailAddress = CompanyEmail.Value,
                    CompanyName = CompanyName.Value,
                    CompanyUrl = CompanyUrl.Value,

                    //PrimaryContact = PrimaryContact.Value,
                    PhoneNumber = PrimaryPhone.Value,

                    //SecondaryPhoneNumber = SecondaryPhone.Value,
                    //PhysicalAddress = new Models.Address
                    //{
                    //    Address1 = AddressLineOne.Value,
                    //    Address2 = AddressLineTwo.Value,
                    //    City = City.Value,
                    //    State = State.Value,
                    //    PostalCode = Zip.Value
                    //},
                    //SocialNetwork1 = SocialNetwork1.Value,
                    //SocialNetwork2 = SocialNetwork2.Value,
                    //SocialNetwork3 = SocialNetwork3.Value
                };

                // TODO: ...
                await navigationService.NavigateAsync(
                    nameof(TradeSpecialtiesPage),
                    new NavigationParameters { { "account", contractor } }).ConfigureAwait(false);
            });
        }

        public ValidatableObject<string> AddressLineOne { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> AddressLineTwo { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> City { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> CompanyEmail { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> CompanyName { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> CompanyUrl { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> PrimaryPhone { get; } = new ValidatableObject<string>();

        public ReactiveCommand Next { get; }

        public ValidatableObject<string> SecondaryPhone { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> State { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> Zip { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> SocialNetwork1 { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> SocialNetwork2 { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> SocialNetwork3 { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> PrimaryContact { get; } = new ValidatableObject<string>();

        private Dictionary<string, string> States { get; } = new Dictionary<string, string>
        {
            { "AL", "Alabama" },
            { "AK", "Alaska" },
            { "AZ", "Arizona" },
            { "AR", "Arkansas" },
            { "CA", "California" },
            { "CO", "Colorado" },
            { "CT", "Connecticut" },
            { "DE", "Delaware" },
            { "DC", "District of Columbia" },
            { "FL", "Florida" },
            { "GA", "Georgia" },
            { "HI", "Hawaii" },
            { "ID", "Idaho" },
            { "IL", "Illinois" },
            { "IN", "Indiana" },
            { "IA", "Iowa" },
            { "KS", "Kansas" },
            { "KY", "Kentucky" },
            { "LA", "Louisiana" },
            { "ME", "Maine" },
            { "MD", "Maryland" },
            { "MA", "Massachusetts" },
            { "MI", "Michigan" },
            { "MN", "Minnesota" },
            { "MS", "Mississippi" },
            { "MO", "Missouri" },
            { "MT", "Montana" },
            { "NE", "Nebraska" },
            { "NV", "Nevada" },
            { "NH", "New Hampshire" },
            { "NJ", "New Jersey" },
            { "NM", "New Mexico" },
            { "NY", "New York" },
            { "NC", "North Carolina" },
            { "ND", "North Dakota" },
            { "OH", "Ohio" },
            { "OK", "Oklahoma" },
            { "OR", "Oregon" },
            { "PA", "Pennsylvania" },
            { "RI", "Rhode Island" },
            { "SC", "South Carolina" },
            { "SD", "South Dakota" },
            { "TN", "Tennessee" },
            { "TX", "Texas" },
            { "UT", "Utah" },
            { "VT", "Vermont" },
            { "VA", "Virginia" },
            { "WA", "Washington" },
            { "WV", "West Virginia" },
            { "WI", "Wisconsin" },
            { "WY", "Wyoming" }
        };

        /// <summary>
        /// Adds the validation rules for this instance.
        /// </summary>
        private void AddValidationRules()
        {
            CompanyName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Company name cannot be empty" });
            PrimaryContact.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Primary contact cannot be empty" });
            CompanyEmail.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Company email cannot be empty" });
            CompanyEmail.Validations.Add(new EmailRule { ValidationMessage = "Company email must be a valid email address" });

            CompanyUrl.Validations.Add(new ValidUrlRule { ValidationMessage = "Company URL must be a valid URL" });

            PrimaryPhone.Validations.Add(new PhoneRule { ValidationMessage = "Phone number must be a valid phone number" });
            SecondaryPhone.Validations.Add(new PhoneRule { ValidationMessage = "Phone number must be a valid phone number" });

            SocialNetwork1.Validations.Add(new ValidUrlRule { ValidationMessage = "Must be a valid URL" });
            SocialNetwork2.Validations.Add(new ValidUrlRule { ValidationMessage = "Must be a valid URL" });
            SocialNetwork3.Validations.Add(new ValidUrlRule { ValidationMessage = "Must be a valid URL" });
        }

        /// <summary>
        /// Returns <c>true</c> if the data is considered valid in it's current state.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        private bool IsValid()
        {
            // NOTE: Validate each control individually so we get error indicators for all
            foreach (var field in _validatableFields)
            {
                field.Validate();
            }

            return _validatableFields.All(field => field.IsValid);
        }
    }
}