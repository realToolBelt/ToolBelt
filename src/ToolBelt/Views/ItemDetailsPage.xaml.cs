using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailsPage : ContentPageBase<ItemDetailsPageViewModel>
    {
        public ItemDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .Bind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Item.Title, v => v._lblTitle.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Item.Price, v => v._lblPrice.Text, price => $"{price:C}")
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Item.Description, v => v._lblDescription.Text)
                    .DisposeWith(disposable);
            });
        }
    }
}