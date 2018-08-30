using Prism.Navigation;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ReviewsPageViewModel : BaseViewModel
    {
        public ReviewsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Reviews";
        }
    }
}