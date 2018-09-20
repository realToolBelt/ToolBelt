using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Projects
{
    public class EditProjectPageViewModel : BaseViewModel
    {
        private readonly IEnumerable<IValidity> _validatableFields;
        private Project _project;

        public EditProjectPageViewModel(
            INavigationService navigationService,
            IUserDialogs dialogService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "New Project";
            analyticService.TrackScreen("edit-project-page");

            // store the fields in an enumerable for easy use later on in this class
            _validatableFields = new IValidity[]
            {
                ProjectName,
                StartDate,
                EndDate,
                Description,
                SkillsRequired,
                PayRate,
                PaymentType
            };

            AddValidationRules();

            Save = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!IsValid())
                {
                    return;
                }

                analyticService.TrackTapEvent("save");

                if (_project != null)
                {
                    // if the product was being edited, map the local fields back to the project and
                    // save it
                    _project.Name = ProjectName.Value;
                    _project.EstimatedStartDate = StartDate.Value.Value;
                    _project.EstimatedEndDate = EndDate.Value.Value;
                    _project.Description = Description.Value;
                    _project.SkillsRequired = SkillsRequired.Value;
                    _project.PaymentRate = decimal.Parse(PayRate.Value);
                    _project.PaymentType = PaymentType.Value;

                    // TODO: projectDataStore.Update(_project);
                }
                else
                {
                    // the project was a new project. Save it

                    // TODO: projectDataStore.Save(_project);
                }

                // TODO: Return the project in the navigation parameters so we can refresh it in the UI of the calling page.

                await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
            });

            Cancel = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_validatableFields.Any(field => field.IsChanged))
                {
                    bool keepEditing = await dialogService.ConfirmAsync(
                        new ConfirmConfig
                        {
                            Title = "Unsaved changes",
                            Message = "Are you sure you want to discard this project?",
                            OkText = "Keep Editing",
                            CancelText = "Discard"
                        });
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

            NavigatingTo
                .Take(1)
                .Where(args => args.ContainsKey("project"))
                .Select(args => (Project)args["project"])
                .Subscribe(project =>
                {
                    // store the project being edited
                    _project = project;
                    Title = "Edit Project";

                    // map the project being edited to the local fields
                    ProjectName.Value = project.Name;
                    StartDate.Value = project.EstimatedStartDate;
                    EndDate.Value = project.EstimatedEndDate;
                    Description.Value = project.Description;
                    SkillsRequired.Value = project.SkillsRequired;
                    PayRate.Value = project.PaymentRate.ToString();
                    PaymentType.Value = project.PaymentType;

                    // accept changes for all fields
                    foreach (var field in _validatableFields)
                    {
                        field.AcceptChanges();
                    }
                });

            // accept changes so we can determine whether they've been changed later on
            foreach (var field in _validatableFields)
            {
                field.AcceptChanges();
            }
        }

        /// <summary>
        /// Gets the command used to cancel editing of the data.
        /// </summary>
        public ReactiveCommand Cancel { get; }

        public ValidatableObject<string> Description { get; } = new ValidatableObject<string>();

        public ValidatableObject<DateTime?> EndDate { get; } = new ValidatableObject<DateTime?>();

        public ValidatableObject<WorkPaymentType> PaymentType { get; } = new ValidatableObject<WorkPaymentType>();

        public List<WorkPaymentType> PaymentTypes { get; } =
            new List<WorkPaymentType>
            {
                WorkPaymentType.Hourly,
                WorkPaymentType.Piece
            };

        public ValidatableObject<string> PayRate { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> ProjectName { get; } = new ValidatableObject<string>();

        /// <summary>
        /// Gets the command used to save the data.
        /// </summary>
        public ReactiveCommand Save { get; }

        public ValidatableObject<string> SkillsRequired { get; } = new ValidatableObject<string>();

        public ValidatableObject<DateTime?> StartDate { get; } = new ValidatableObject<DateTime?>();

        /// <summary>
        /// Adds the validation rules for this instance.
        /// </summary>
        private void AddValidationRules()
        {
            ProjectName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Project name cannot be empty" });
            StartDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "Start date cannot be empty" });
            EndDate.Validations.Add(new IsNotNullRule<DateTime?> { ValidationMessage = "End date cannot be empty" });
            PayRate.Validations.Add(new IsNotNullRule<string> { ValidationMessage = "Payment rate cannot be empty" });
            PayRate.Validations.Add(new ActionValidationRule<string>(value => decimal.TryParse(value, out _), "Payment rate must be a number"));

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
            foreach (var field in _validatableFields)
            {
                field.Validate();
            }

            return _validatableFields.All(field => field.IsValid);
        }
    }
}