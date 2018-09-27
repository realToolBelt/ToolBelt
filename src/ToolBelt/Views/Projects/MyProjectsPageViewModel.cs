using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Services;
using ToolBelt.Services.Analytics;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Projects
{
    public class MyProjectsPageViewModel : BaseViewModel, IDestructible
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public MyProjectsPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore,
            IUserDialogs dialogService,
            IUserService userService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "My Projects";

            this.WhenActivated((CompositeDisposable _) =>
            {
                // track every time this screen is activated
                analyticService.TrackScreen("my-projects");
            });

            var initialize = ReactiveCommand.CreateFromTask<Unit, IEnumerable<Project>>(_ =>
            {
                AssertRunningOnBackgroundThread();
                return projectDataStore.LoadProjectsForUser(userService.AuthenticatedUser.AccountId);
            });

            initialize
                .Subscribe(projects =>
                {
                    Projects.Reset(projects.Select(proj => new MyProjectViewModel(proj, Delete, Close, Edit)));
                });

            Projects.ActOnEveryObject(
                _ =>
                {
                    this.Log().Debug("Item added");
                },
                projViewModel =>
                {
                    this.Log().Debug("Item removed");

                    // make sure we dispose of the item to release the subscription from the item's
                    // commands to our commands
                    projViewModel.Dispose();
                });

            // When an exception is thrown while loading data, log the error and let the user handle
            // the exception
            initialize.ThrownExceptions
                .SelectMany(exception =>
                {
                    this.Log().ErrorException("Error loading or refreshing data", exception);
                    return SharedInteractions.Error.Handle(exception);
                })
                .Subscribe();

            Activator
                .Activated
                .Take(1)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .InvokeCommand(initialize);

            ViewProjectDetails = ReactiveCommand.CreateFromTask<MyProjectViewModel, Unit>(async project =>
            {
                analyticService.TrackTapEvent("view-project");

                await NavigationService.NavigateAsync(
                    nameof(ProjectDetailsPage),
                    new NavigationParameters
                    {
                        { "project", project.Project }
                    }).ConfigureAwait(false);

                return Unit.Default;
            });

            Delete = ReactiveCommand.CreateFromTask<MyProjectViewModel, Unit>(async project =>
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

                // remove the project
                await projectDataStore.DeleteProjectAsync(project.Project);
                Projects.Remove(project);

                dialogService.Toast(new ToastConfig("Project deleted successfully!"));

                return Unit.Default;
            });

            Close = ReactiveCommand.CreateFromTask<MyProjectViewModel, Unit>(async project =>
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

                //project.Project.Status = ProjectStatus.Closed;

                // TODO: Save to the database

                return Unit.Default;
            });

            Edit = ReactiveCommand.CreateFromTask<MyProjectViewModel, Unit>(async project =>
            {
                analyticService.TrackTapEvent("edit-project");

                await NavigationService.NavigateAsync(
                    $"NavigationPage/{nameof(EditProjectPage)}",
                    new NavigationParameters
                    {
                        { "project", project.Project }
                    },
                    useModalNavigation: true).ConfigureAwait(false);

                return Unit.Default;
            });

            initialize.IsExecuting
                .StartWith(false)
                .ToProperty(this, x => x.IsBusy, out _isBusy, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<MyProjectViewModel, Unit> Close { get; }

        public ReactiveCommand<MyProjectViewModel, Unit> Delete { get; }

        public ReactiveCommand<MyProjectViewModel, Unit> Edit { get; }

        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveList<MyProjectViewModel> Projects { get; } = new ReactiveList<MyProjectViewModel>();

        public ReactiveCommand<MyProjectViewModel, Unit> ViewProjectDetails { get; }

        public void Destroy()
        {
            Projects.Clear();
        }
    }

    /// <summary>
    /// View-model that wraps a project and adds additional behavior for a user's projects.
    /// </summary>
    /// <seealso cref="ReactiveUI.ReactiveObject" />
    /// <seealso cref="System.IDisposable" />
    public class MyProjectViewModel : ReactiveObject, IDisposable
    {
        private CompositeDisposable _disposables;

        public MyProjectViewModel(
            Project project,
            ReactiveCommand<MyProjectViewModel, Unit> delete,
            ReactiveCommand<MyProjectViewModel, Unit> close,
            ReactiveCommand<MyProjectViewModel, Unit> edit)
        {
            Project = project;
            Delete = ReactiveCommand.Create<Unit, MyProjectViewModel>(_ => this);
            Close = ReactiveCommand.Create<Unit, MyProjectViewModel>(_ => this);
            Edit = ReactiveCommand.Create<Unit, MyProjectViewModel>(_ => this);

            // hook the observables up to the higher-level observables
            _disposables = new CompositeDisposable(
                Delete.InvokeCommand(delete),
                Close.InvokeCommand(close),
                Edit.InvokeCommand(edit));
        }

        /// <summary>
        /// Gets the command used to close the project associated with this instance.
        /// </summary>
        public ReactiveCommand<Unit, MyProjectViewModel> Close { get; }

        /// <summary>
        /// Gets the command used to delete the project associated with this instance.
        /// </summary>
        public ReactiveCommand<Unit, MyProjectViewModel> Delete { get; }

        /// <summary>
        /// Gets the command used to edit the project associated with this instance.
        /// </summary>
        public ReactiveCommand<Unit, MyProjectViewModel> Edit { get; }

        /// <summary>
        /// Gets the project associated with this instance.
        /// </summary>
        public Project Project { get; }

        #region IDisposable Support

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _disposables.Dispose();
                    _disposables = null;
                }

                IsDisposed = true;
            }
        }

        #endregion
    }
}