using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using ToolBelt.Models;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Projects
{
    public class ProjectDetailsPageViewModel : BaseViewModel
    {
        private Project _project;

        public ProjectDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Project Details";

            NavigatedTo
                .Take(1)
                .Select(args => (Project)args["project"])
                .Subscribe(project =>
                {
                    Project = project;
                });
        }

        public Project Project
        {
            get => _project;
            private set => this.RaiseAndSetIfChanged(ref _project, value);
        }
    }
}