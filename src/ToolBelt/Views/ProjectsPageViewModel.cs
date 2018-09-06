using MoreLinq.Extensions;
using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using ToolBelt.Extensions;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ProjectsPageViewModel : BaseViewModel
    {
        // TODO: Move this to remote settings?
        private const int ItemsPerPage = 20;

        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        // keep references to the newest and oldest project to load additional data
        private Project _newestProject;

        private Project _oldestProject;

        public ProjectsPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Projects";

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
                        // determined by Id for our purposes
                        var oldProject = projects.MinBy(p => p.Id).First();
                        var newProject = projects.MaxBy(p => p.Id).First();

                        if (_oldestProject == null && _newestProject == null)
                        {
                            _oldestProject = oldProject;
                            _newestProject = newProject;

                            // first projects being added. Add them to the list
                            Projects.AddRange(projects);
                        }
                        else if (_oldestProject?.Id > oldProject.Id)
                        {
                            _oldestProject = oldProject;

                            // if the projects are older, add them to the end of the list
                            Projects.AddRange(projects);
                        }
                        else if (_newestProject?.Id < newProject.Id)
                        {
                            _newestProject = newProject;

                            // if the projects are newer, insert them at the beginning of the list
                            Projects.InsertRange(0, projects);
                        }
                    }
                });

            // when the command is executing, update the busy state
            this.WhenAnyObservable(
                x => x.LoadProjects.IsExecuting,
                x => x.RefreshProjects.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy, scheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveCommand<Unit, IEnumerable<Project>> LoadProjects { get; }

        public ReactiveList<Project> Projects { get; } = new ReactiveList<Project>();

        public ReactiveCommand<Unit, IEnumerable<Project>> RefreshProjects { get; }

        public ReactiveCommand<Project, Unit> ViewProjectDetails { get; }
    }
}
