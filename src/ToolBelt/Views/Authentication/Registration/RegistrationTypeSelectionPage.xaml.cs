using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationTypeSelectionPage : ContentPageBase<RegistrationTypeSelectionPageViewModel>
    {
        private readonly IDeviceOrientation _deviceOrientationService;
        private ScreenOrientation _lastOrientation = ScreenOrientation.Unknown;

        public RegistrationTypeSelectionPage(IDeviceOrientation deviceOrientationService)
        {
            _deviceOrientationService = deviceOrientationService;

            using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Activate."))
                {
                    OnOrientationChanged(deviceOrientationService.ScreenMetrics.Orientation);

                    _tgrTradesmen
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Tradesman)
                        .DisposeWith(disposable);

                    _tgrContractor
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Contractor)
                        .DisposeWith(disposable);
                }
            });
        }

        protected override bool OnBackButtonPressed()
        {
            // execute the "Go Back" command on the view-model
            ViewModel.GoBack.Execute().Subscribe();

            // let XF know we're handling this one ourselves
            return true;
        }

        protected void OnOrientationChanged(ScreenOrientation orientation)
        {
            if (_lastOrientation != orientation)
            {
                _lastOrientation = orientation;
                if (orientation == ScreenOrientation.Landscape)
                {
                    VisualStateManager.GoToState(_flexMain, "Landscape");
                }
                else
                {
                    VisualStateManager.GoToState(_flexMain, "Portrait");
                }
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            OnOrientationChanged(_deviceOrientationService.ScreenMetrics.Orientation);
        }
    }
}