using MoreLinq.Extensions;
using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using ToolBelt.Extensions;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Projects
{
    public class ProjectsPageViewModel : BaseViewModel
    {
        // TODO: Move this to remote settings?
        private const int ItemsPerPage = 20;

        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        // keep references to the newest and oldest project to load additional data
        private Project _newestProject;

        private Project _oldestProject;
        private ProjectFilter _filter = new ProjectFilter();

        public ProjectsPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "Projects";

            this.WhenActivated((CompositeDisposable _) =>
            {
                // track every time this screen is activated
                analyticService.TrackScreen("my-projects");
            });

            ViewProjectDetails = ReactiveCommand.CreateFromTask<Project, Unit>(async project =>
            {
                analyticService.TrackTapEvent("view-project");

                await NavigationService.NavigateAsync(
                    nameof(ProjectDetailsPage),
                    new NavigationParameters
                    {
                        { "project", project }
                    }).ConfigureAwait(false);

                return Unit.Default;
            });

            AddProject = ReactiveCommand.CreateFromTask(async () =>
            {
                analyticService.TrackTapEvent("new-project");
                await NavigationService.NavigateAsync($"NavigationPage/{nameof(EditProjectPage)}", useModalNavigation: true).ConfigureAwait(false);
            });

            Filter = ReactiveCommand.CreateFromTask(async () =>
            {
                // TODO: Finish this
                await NavigationService.NavigateAsync(
                    $"NavigationPage/{nameof(ProjectFilterPage)}",
                    new NavigationParameters
                    {
                        { "filter", _filter }
                    },
                    useModalNavigation: true).ConfigureAwait(false);
            });

            // set up the command used to load projects
            LoadProjects = ReactiveCommand.CreateFromTask(_ =>
            {
                this.Log().Debug($"Loading projects on thread: {Thread.CurrentThread.ManagedThreadId}, IsBackground = {Thread.CurrentThread.IsBackground}");
                AssertRunningOnBackgroundThread();
                return projectDataStore.LoadOldProjects(_oldestProject, ItemsPerPage);
            });

            // set up the command used to refresh projects
            RefreshProjects = ReactiveCommand.CreateFromTask(_ =>
            {
                // TODO: Should track this with analytics?
                this.Log().Debug($"Refreshing projects on thread: {Thread.CurrentThread.ManagedThreadId}, IsBackground = {Thread.CurrentThread.IsBackground}");
                AssertRunningOnBackgroundThread();
                return _newestProject != null ?
                    projectDataStore.LoadNewProjects(_newestProject, ItemsPerPage) :
                    projectDataStore.LoadNewProjects(ItemsPerPage);
            });

            LoadProjects
                .Merge(RefreshProjects)
                .SubscribeSafe(projects =>
                {
                    if (projects.Any())
                    {
                        // NOTE: This probably isn't necessary...
                        projects = projects.ExceptBy(Projects, p => p.Id);

                        // get the oldest and newest projects from the new data set. Age is simply
                        // determined by the date the project was created
                        var oldProject = projects.MinBy(p => p.CreateDate).First();
                        var newProject = projects.MaxBy(p => p.CreateDate).First();

                        if (_oldestProject == null && _newestProject == null)
                        {
                            _oldestProject = oldProject;
                            _newestProject = newProject;

                            // first projects being added. Add them to the list
                            Projects.AddRange(projects);
                        }
                        else if (_oldestProject?.CreateDate > oldProject.CreateDate)
                        {
                            _oldestProject = oldProject;

                            // if the projects are older, add them to the end of the list
                            Projects.AddRange(projects);
                        }
                        else if (_newestProject?.CreateDate < newProject.CreateDate)
                        {
                            _newestProject = newProject;

                            // if the projects are newer, insert them at the beginning of the list
                            Projects.InsertRange(0, projects);
                        }
                    }
                });

            // when either of the commands are executing, update the busy state
            LoadProjects.IsExecuting
                .CombineLatest(RefreshProjects.IsExecuting, (isLoadExecuting, isRefreshExecuting) => isLoadExecuting || isRefreshExecuting)
                .DistinctUntilChanged()
                .StartWith(false)
                .ToProperty(this, x => x.IsBusy, out _isBusy, scheduler: RxApp.MainThreadScheduler);

            // When an exception is thrown for either command, log the error and let the user handle
            // the exception
            LoadProjects.ThrownExceptions
                .Merge(RefreshProjects.ThrownExceptions)
                .SelectMany(exception =>
                {
                    this.Log().ErrorException("Error loading or refreshing data", exception);
                    return SharedInteractions.Error.Handle(exception);
                })
                .Subscribe();
        }

        public ReactiveCommand AddProject { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveCommand<Unit, IEnumerable<Project>> LoadProjects { get; }

        public ReactiveList<Project> Projects { get; } = new ReactiveList<Project>();

        public ReactiveCommand<Unit, IEnumerable<Project>> RefreshProjects { get; }

        public ReactiveCommand<Project, Unit> ViewProjectDetails { get; }

        public ReactiveCommand Filter { get; }
    }
}
