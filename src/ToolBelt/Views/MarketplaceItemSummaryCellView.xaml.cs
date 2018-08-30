using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketplaceItemSummaryCellView : ReactiveViewCell<MarketplaceItemSummary>
    {
        public MarketplaceItemSummaryCellView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Title, v => v._lblTitle.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Description, v => v._lblDescription.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Price, v => v._lblPrice.Text, price => $"{price:C}")
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.IconSource, v => v._imgIcon.Source)
                    .DisposeWith(disposable);
            });
        }
    }
}