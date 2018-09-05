using Prism.Navigation;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class TradesmenPageViewModel : BaseViewModel
    {
        public TradesmenPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Tradesmen";
        }
    }
}