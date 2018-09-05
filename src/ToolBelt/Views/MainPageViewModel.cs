using Prism.Navigation;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            Title = "Home";
        }
    }
}