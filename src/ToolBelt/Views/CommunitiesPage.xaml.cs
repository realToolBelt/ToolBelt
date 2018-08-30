using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommunitiesPage : ContentPageBase<CommunitiesPageViewModel>
    {
        public CommunitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Communities, v => v._communitiesPicker.ItemsSource)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.SelectedCommunity, v => v._communitiesPicker.SelectedItem)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Items, v => v._lstMarketplaceItems.ItemsSource)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                    .DisposeWith(disposable);

                _lstMarketplaceItems
                    .Events()
                    .ItemSelected
                    .Select(item => item.SelectedItem as MarketplaceItemSummary)
                    .Where(item => item != null)
                    .Subscribe(item =>
                    {
                        if (item.TapCommand != null)
                        {
                            item.TapCommand?.Execute().Subscribe();
                        }

                        _lstMarketplaceItems.SelectedItem = null;
                    })
                    .DisposeWith(disposable);
            });
        }
    }
}