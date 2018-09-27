using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Services.Analytics;
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

                //StartStatus,
                Description,
                SkillsRequired,
                PayRate,

                //PaymentType
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
                    //_project.Name = ProjectName.Value;
                    _project.Description = Description.Value;

                    //_project.SkillsRequired = SkillsRequired.Value;
                    //_project.PaymentRate = decimal.Parse(PayRate.Value);
                    //_project.PaymentType = PaymentType.Value;

                    //switch (StartStatus.Value)
                    //{
                    //    case ProjectStartStatus.ReadyNow:
                    //        _project.EstimatedStartDate = DateTime.Today;
                    //        break;

                    // case ProjectStartStatus.OneToTwoWeeks: _project.EstimatedStartDate =
                    // DateTime.Today.AddDays(7); break;

                    // case ProjectStartStatus.ThreeToFourWeeks: _project.EstimatedStartDate =
                    // DateTime.Today.AddDays(7 * 3); break;

                    // case ProjectStartStatus.FiveOrMoreWeeks: _project.EstimatedStartDate =
                    // DateTime.Today.AddDays(7 * 5); break;

                    //    default:
                    //        throw new InvalidOperationException("Invalid start status");
                    //}

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
                    //ProjectName.Value = project.Name;
                    //StartStatus.Value = project.StartStatus;
                    Description.Value = project.Description;

                    //SkillsRequired.Value = project.SkillsRequired;
                    //PayRate.Value = project.PaymentRate.ToString();
                    //PaymentType.Value = project.PaymentType;

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

        //public ValidatableObject<WorkPaymentType> PaymentType { get; } = new ValidatableObject<WorkPaymentType>();

        //public List<WorkPaymentType> PaymentTypes { get; } =
        //    new List<WorkPaymentType>
        //    {
        //        WorkPaymentType.Hourly,
        //        WorkPaymentType.Piece
        //    };

        public ValidatableObject<string> PayRate { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> ProjectName { get; } = new ValidatableObject<string>();

        /// <summary>
        /// Gets the command used to save the data.
        /// </summary>
        public ReactiveCommand Save { get; }

        public ValidatableObject<string> SkillsRequired { get; } = new ValidatableObject<string>();

        //public ValidatableObject<ProjectStartStatus> StartStatus { get; } = new ValidatableObject<ProjectStartStatus>();

        //public List<KeyValuePair<string, ProjectStartStatus>> StartStatuses { get; } =
        //    new List<KeyValuePair<string, ProjectStartStatus>>
        //    {
        //        new KeyValuePair<string, ProjectStartStatus>("Ready Now", ProjectStartStatus.ReadyNow),
        //        new KeyValuePair<string, ProjectStartStatus>("1 - 2 Weeks", ProjectStartStatus.OneToTwoWeeks),
        //        new KeyValuePair<string, ProjectStartStatus>("3 - 4 Weeks", ProjectStartStatus.ThreeToFourWeeks),
        //        new KeyValuePair<string, ProjectStartStatus>("5+ Weeks", ProjectStartStatus.FiveOrMoreWeeks),
        //    };

        /// <summary>
        /// Adds the validation rules for this instance.
        /// </summary>
        private void AddValidationRules()
        {
            ProjectName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Project name cannot be empty" });
            PayRate.Validations.Add(new IsNotNullRule<string> { ValidationMessage = "Pay rate cannot be empty" });
            PayRate.Validations.Add(new ActionValidationRule<string>(value => decimal.TryParse(value, out _), "Pay rate must be a number"));
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