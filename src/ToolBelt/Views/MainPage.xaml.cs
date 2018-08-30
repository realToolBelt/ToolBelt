using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPageBase<MainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Pages, v => v._carousel.ItemsSource)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Position, v => v._carousel.Position)
                    .DisposeWith(disposable);

                this
                    .BindCommand(ViewModel, vm => vm.ViewDashboard, v => v._lblDashboard.GestureRecognizers[0])
                    .DisposeWith(disposable);

                this
                    .BindCommand(ViewModel, vm => vm.ViewMessages, v => v._lblMessages.GestureRecognizers[0])
                    .DisposeWith(disposable);

                this
                    .BindCommand(ViewModel, vm => vm.ViewReviews, v => v._lblReviews.GestureRecognizers[0])
                    .DisposeWith(disposable);
            });
        }
    }
}