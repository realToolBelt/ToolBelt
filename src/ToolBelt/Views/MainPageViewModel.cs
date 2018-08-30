using Prism.Navigation;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using ToolBelt.ViewModels;
using Xamarin.Forms;

namespace ToolBelt.Views
{
    public class MainPageViewModel : BaseViewModel
    {
        private int _position;

        public MainPageViewModel(
            INavigationService navigationService,
            DashboardPage dashboard,
            Messages.MessagesPage messagesPage,
            ReviewsPage reviewsPage) : base(navigationService)
        {
            Title = "Home";

            Pages.Add(dashboard);
            Pages.Add(messagesPage);
            Pages.Add(reviewsPage);

            ViewDashboard = ReactiveCommand.Create(() => Position = 0);
            ViewMessages = ReactiveCommand.Create(() => Position = 1);
            ViewReviews = ReactiveCommand.Create(() => Position = 2);
        }

        public ReactiveList<View> Pages { get; } = new ReactiveList<View>();

        public int Position
        {
            get => _position;
            set => this.RaiseAndSetIfChanged(ref _position, value);
        }

        public ReactiveCommand ViewDashboard { get; }

        public ReactiveCommand ViewMessages { get; }

        public ReactiveCommand ViewReviews { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // if we're given a page to navigate to, select that page
            if (parameters.TryGetValue("page", out string pageName))
            {
                var index = Pages.IndexOf(Pages.FirstOrDefault(p => p.GetType().Name == pageName));
                if (index >= 0)
                {
                    Position = index;
                }
            }
        }
    }
}