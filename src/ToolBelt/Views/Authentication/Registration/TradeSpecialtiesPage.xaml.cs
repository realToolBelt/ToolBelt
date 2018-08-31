using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradeSpecialtiesPage : ContentPageBase<TradeSpecialtiesPageViewModel>
    {
        public TradeSpecialtiesPage()
        {
            using (this.Log().Perf($"{nameof(TradeSpecialtiesPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(TradeSpecialtiesPage)}: Activate."))
                {
                    this
                        .BindCommand(ViewModel, vm => vm.SelectAll, v => v._miSelectAll)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.SelectNone, v => v._miSelectNone)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Items, v => v._lstItems.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Next, v => v._btnNext)
                        .DisposeWith(disposable);

                    _lstItems
                        .Events()
                        .ItemSelected
                        .Select(item => item.SelectedItem as SelectionViewModel)
                        .Where(item => item != null)
                        .Subscribe(item =>
                        {
                            item.IsSelected = !item.IsSelected;
                            _lstItems.SelectedItem = null;
                        })
                        .DisposeWith(disposable);
                }
            });
        }
    }
}