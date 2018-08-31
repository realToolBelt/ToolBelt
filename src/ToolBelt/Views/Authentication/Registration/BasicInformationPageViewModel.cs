using Prism.Navigation;
using ReactiveUI;
using System;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication.Registration
{
    public class BasicInformationPageViewModel : BaseViewModel
    {
        public BasicInformationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Your Information";

            AddValidationRules();

            Next = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!IsValid())
                {
                    return;
                }

                // TODO: Should save to user profile at this point and flag that this page has been completed...
                // TODO: Add to user info and move on to the next page
                await navigationService.NavigateAsync(nameof(TradeSpecialtiesPage)).ConfigureAwait(false);
            });
        }

        public ValidatableObject<DateTime?> BirthDate { get; } = new ValidatableObject<DateTime?>();

        public ValidatableObject<string> Email { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> FirstName { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> LastName { get; } = new ValidatableObject<string>();

        public ReactiveCommand Next { get; }

        public ValidatableObject<string> Phone { get; } = new ValidatableObject<string>();

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