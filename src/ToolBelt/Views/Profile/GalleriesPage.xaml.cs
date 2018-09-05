using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleriesPage : ContentPageBase<GalleriesPageViewModel>
    {
        public GalleriesPage()
        {
            using (this.Log().Perf($"{nameof(GalleriesPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(GalleriesPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Albums, v => v._lstAlbums.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._lstAlbums.IsRefreshing)
                        .DisposeWith(disposable);

                    this
                        .WhenAnyValue(x => x.ViewModel)
                        .Where(vm => vm != null)
                        .ToSignal()
                        .InvokeCommand(this, x => x.ViewModel.Initialize)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}