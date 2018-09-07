using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using ToolBelt.Services;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class CreateProjectPageViewModel : BaseViewModel
    {
        public CreateProjectPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "New Project";
            analyticService.TrackScreen("create-project-page");

            AddValidationRules();

            Save = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!IsValid())
                {
                    return;
                }

                analyticService.TrackTapEvent("save");

                // TODO: perform save
                await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
            });

            Cancel = ReactiveCommand.CreateFromTask(async () =>
            {
                if (ProjectName.IsChanged
                    || StartDate.IsChanged
                    || EndDate.IsChanged)
                {
                    bool keepEditing = await dialogService.DisplayAlertAsync(
                        "Unsaved changes",
                        "Are you sure you want to discard this project?",
                        "Keep Editing",
                        "Discard");
                    if (keepEditing)
                    {
                        // the user has chosen the option to continue editing
                        return;
                    }
                }

                analyticService.TrackTapEvent("cancel");

                // if there are no changes, or the user chooses to discard, go back to the previous screen
                await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
            });

            // accept changes so we can determine whether they've been changed later on
            ProjectName.AcceptChanges();
            StartDate.AcceptChanges();
            EndDate.AcceptChanges();
        }

        /// <summary>
        /// Gets the command used to cancel editing of the data.
        /// </summary>
        public ReactiveCommand Cancel { get; }

        public ValidatableObject<DateTime?> EndDate { get; } = new ValidatableObject<DateTime?>();

        public ValidatableObject<string> ProjectName { get; } = new ValidatableObject<string>();

        /// <summary>
        /// Gets the command used to save the data.
        /// </summary>
        public ReactiveCommand Save { get; }

        public ValidatableObject<DateTime?> StartDate { get; } = new ValidatableObject<DateTime?>();

        /// <summary>
        /// Adds the validation rules for this instance.
        /// </summary>
        private void AddValidationRules()
        {
            ProjectName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Project name cannot be empty" });
            StartDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "Start date cannot be empty" });
            EndDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "End date cannot be empty" });

            StartDate.Validations.Add(new ActionValidationRule<DateTime?>(
                startDate => !EndDate.Value.HasValue || !startDate.HasValue || startDate.Value < EndDate.Value.Value,
                "Start date cannot come after the end date"));

            EndDate.Validations.Add(new ActionValidationRule<DateTime?>(
                endDate => !StartDate.Value.HasValue || !endDate.HasValue || endDate.Value > StartDate.Value.Value,
                "End date cannot come before the start date"));
        }

        /// <summary>
        /// Returns <c>true</c> if the data is considered valid in it's current state.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
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