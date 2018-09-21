using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendedSplashPage : ContentPageBase<ExtendedSplashPageViewModel>
    {
        public ExtendedSplashPage()
        {
            using (this.Log().Perf($"{nameof(ExtendedSplashPage)}: Initialize component."))
            {
                InitializeComponent();
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ExtendedSplashPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsVisible)
                        .DisposeWith(disposable);

                    this
                        .WhenAnyValue(x => x.ViewModel.Initialize)
                        .ToSignal()
                        .ObserveOn(RxApp.TaskpoolScheduler)
                        .InvokeCommand(this, v => v.ViewModel.Initialize)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}