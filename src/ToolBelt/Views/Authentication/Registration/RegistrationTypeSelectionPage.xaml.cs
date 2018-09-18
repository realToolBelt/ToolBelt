using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationTypeSelectionPage : ContentPageBase<RegistrationTypeSelectionPageViewModel>
    {
        public RegistrationTypeSelectionPage(IDeviceOrientation deviceOrientationService)
        {
            using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Initialize component."))
            {
                InitializeComponent();
                OnOrientationChanged(deviceOrientationService.GetOrientation());
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Activate."))
                {
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

        protected override void OnOrientationChanged(DeviceOrientations orientation)
        {
            switch (orientation)
            {
                case DeviceOrientations.Landscape:
                    _mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    _mainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);

                    Grid.SetColumn(_frameContractor, 1);
                    Grid.SetRow(_frameContractor, 0);
                    break;

                default:
                    _mainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);
                    _mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

                    Grid.SetColumn(_frameContractor, 0);
                    Grid.SetRow(_frameContractor, 1);
                    break;
            }
        }
    }
}