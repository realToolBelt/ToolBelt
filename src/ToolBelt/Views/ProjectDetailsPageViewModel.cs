using Prism.Navigation;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ProjectDetailsPageViewModel : BaseViewModel
    {
        public ProjectDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Project Details";
        }
    }
}