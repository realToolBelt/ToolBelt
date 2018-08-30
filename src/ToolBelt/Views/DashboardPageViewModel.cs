using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class DashboardPageViewModel : BaseViewModel
    {
        private readonly IProjectDataStore _projectDataStore;
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public DashboardPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Dashboard";
            _projectDataStore = projectDataStore;

            var loadProjectsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var random = new Random();
                await Task.Delay(random.Next(400, 2000));

                return await projectDataStore.GetProjectsAsync().ConfigureAwait(false);
            });

            // when the command is executing, update the busy state
            loadProjectsCommand.IsExecuting
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);

            loadProjectsCommand
                .Execute()
                .Subscribe(projects => Projects.Reset(projects));

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
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveList<Project> Projects { get; } = new ReactiveList<Project>();

        public ReactiveCommand<Project, Unit> ViewProjectDetails { get; }
    }
}
