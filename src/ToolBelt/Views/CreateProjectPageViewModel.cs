using Prism.Navigation;
using ReactiveUI;
using System;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class CreateProjectPageViewModel : BaseViewModel
    {
        public CreateProjectPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "New Project";

            AddValidationRules();

            Save = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!IsValid())
                {
                    return;
                }

                // TODO: perform save
                await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
            });

            Cancel = ReactiveCommand.CreateFromTask(async () =>
            {
                // TODO: Prompt user to confirm cancel (if fields are modified)...
                await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
            });
        }

        public ReactiveCommand Cancel { get; }

        public ValidatableObject<DateTime?> EndDate { get; } = new ValidatableObject<DateTime?>();

        public ValidatableObject<string> ProjectName { get; } = new ValidatableObject<string>();

        public ReactiveCommand Save { get; }

        public ValidatableObject<DateTime?> StartDate { get; } = new ValidatableObject<DateTime?>();

        private void AddValidationRules()
        {
            ProjectName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Project name cannot be empty" });

            StartDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "Start date cannot be empty" });
            EndDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "End date cannot be empty" });
        }

        private bool IsValid()
        {
            // NOTE: Validate each control individually so we get error indicators for all
            ProjectName.Validate();
            StartDate.Validate();
            EndDate.Validate();

            return ProjectName.IsValid
                && StartDate.IsValid
                && EndDate.IsValid;
        }
    }
}