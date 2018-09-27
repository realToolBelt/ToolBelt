using Prism.Navigation;
using ToolBelt.Services.Analytics;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.About
{
    public class PrivacyPolicyPageViewModel : BaseViewModel
    {
        public PrivacyPolicyPageViewModel(
            INavigationService navigationService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "Privacy Policy";
            analyticService.TrackScreen("privacy-policy-page");
        }
    }
}
