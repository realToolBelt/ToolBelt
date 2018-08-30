using Prism.Navigation;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.About
{
    public class AboutUsPageViewModel : BaseViewModel
    {
        public AboutUsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "About Us";
        }
    }
}
