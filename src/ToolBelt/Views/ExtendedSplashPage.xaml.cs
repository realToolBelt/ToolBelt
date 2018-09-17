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
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            using (this.Log().Perf($"{nameof(ExtendedSplashPage)}: Initialize component."))
            {
                InitializeComponent();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

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

                    // TODO: Make sure this runs on the background thread...
                    this
                        .WhenAnyValue(x => x.ViewModel.Initialize)
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Initialize)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}