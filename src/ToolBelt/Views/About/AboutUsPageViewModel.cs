using Prism.Navigation;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.About
{
    public class AboutUsPageViewModel : BaseViewModel
    {
        public AboutUsPageViewModel(
            INavigationService navigationService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "About Us";
            analyticService.TrackScreen("about-us-page");
        }
    }
}
