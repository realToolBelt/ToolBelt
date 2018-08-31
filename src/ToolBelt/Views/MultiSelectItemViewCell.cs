using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using ToolBelt.ViewModels;
using Xamarin.Forms;

namespace ToolBelt.Views
{
    public class MultiSelectItemViewCell : ReactiveViewCell<SelectionViewModel>
    {
        private readonly Label _label = new Label
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.StartAndExpand
        };

        private readonly Switch _switch = new Switch
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.End
        };

        public MultiSelectItemViewCell()
        {
            View = new StackLayout
            {
                Padding = new Thickness(10, 15),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    _label,
                    _switch
                }
            };

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.DisplayValue, v => v._label.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.IsSelected, v => v._switch.IsToggled)
                    .DisposeWith(disposable);

                // TODO: Need to make sure the view-cells get disposed!
                Disposable.Create(() => System.Diagnostics.Debug.WriteLine("    DISPOSING MULTI SELECT CELL")).DisposeWith(disposable);
            });
        }
    }
}
