using Prism.Navigation;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.About
{
    public class PrivacyPolicyPageViewModel : BaseViewModel
    {
        public PrivacyPolicyPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Privacy Policy";
        }
    }
}
