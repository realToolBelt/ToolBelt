using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ProjectsPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private readonly IProjectDataStore _projectDataStore;

        public ProjectsPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Projects";
            _projectDataStore = projectDataStore;

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

            LoadProjects = ReactiveCommand.CreateFromTask(async _ =>
            {
                var random = new Random();
                await Task.Delay(random.Next(400, 2000)).ConfigureAwait(false);

                return await projectDataStore.GetProjectsAsync().ConfigureAwait(false);
            });

            LoadProjects.Subscribe(projects => Projects.Reset(projects));

            // when the command is executing, update the busy state
            LoadProjects.IsExecuting
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveCommand<Unit, IEnumerable<Project>> LoadProjects { get; }

        public ReactiveList<Project> Projects { get; } = new ReactiveList<Project>();

        public ReactiveCommand<Project, Unit> ViewProjectDetails { get; }
    }
}
