using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Projects
{
    public class MyProjectsPageViewModel : BaseViewModel
    {
        public MyProjectsPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore,
            IUserDialogs dialogService,
            IUserService userService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "My Projects";

            Initialize = ReactiveCommand.CreateFromTask<Unit, IEnumerable<Project>>(_ =>
            {
                AssertRunningOnBackgroundThread();
                return projectDataStore.LoadProjectsForUser("");
            });

            Initialize
                .Subscribe(projects =>
                {
                    Projects.Reset(projects);
                });

            Observable
                .Return(Unit.Default)
                .Take(1)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .InvokeCommand(Initialize);

            ViewProjectDetails = ReactiveCommand.CreateFromTask<Project, Unit>(async project =>
            {
                // TODO: Add project as parameter...
                await NavigationService.NavigateAsync(
                    nameof(ProjectDetailsPage),
                    new NavigationParameters
                    {
                        { "project", project }
                    }).ConfigureAwait(false);

                return Unit.Default;
            });

            Delete = ReactiveCommand.CreateFromTask<Project, Unit>(async project =>
            {
                bool shouldDelete = await dialogService.ConfirmAsync(
                    new ConfirmConfig
                    {
                        Message = "Are you sure you want to delete the project?",
                        OkText = "Delete",
                        CancelText = "Cancel"
                    });
                if (!shouldDelete)
                {
                    return Unit.Default;
                }

                analyticService.TrackTapEvent("delete-project");

                project.Status = ProjectStatus.Deleted;

                // TODO: Save to the database

                return Unit.Default;
            });

            Close = ReactiveCommand.CreateFromTask<Project, Unit>(async project =>
            {
                bool shouldDelete = await dialogService.ConfirmAsync(
                    new ConfirmConfig
                    {
                        Message = "Are you sure you want to close the project?",
                        OkText = "Close",
                        CancelText = "Cancel"
                    });
                if (!shouldDelete)
                {
                    return Unit.Default;
                }

                analyticService.TrackTapEvent("close-project");

                project.Status = ProjectStatus.Closed;

                // TODO: Save to the database

                return Unit.Default;
            });
        }

        public ReactiveCommand<Project, Unit> Close { get; }

        public ReactiveCommand<Project, Unit> Delete { get; }

        public ReactiveList<Project> Projects { get; } = new ReactiveList<Project>();

        public ReactiveCommand<Project, Unit> ViewProjectDetails { get; }

        private ReactiveCommand<Unit, IEnumerable<Project>> Initialize { get; }
    }
}