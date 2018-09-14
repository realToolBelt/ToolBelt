using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProjectPage : ContentPageBase<EditProjectPageViewModel>
    {
        public EditProjectPage()
        {
            using (this.Log().Perf($"{nameof(EditProjectPage)}: Initialize component."))
            {
                InitializeComponent();
                _startDateControl.MinimumDate = DateTime.Today;
                _endDateControl.MinimumDate = DateTime.Today;
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(EditProjectPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.ProjectName, v => v._projectNameControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.StartDate, v => v._startDateControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.EndDate, v => v._endDateControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Description, v => v._projectDescriptionControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Save, v => v._miSave)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Cancel, v => v._miCancel)
                        .DisposeWith(disposable);

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var platformConfig = Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>();
                        var softInputMode = platformConfig.GetWindowSoftInputModeAdjust();
                        platformConfig.UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
                        Disposable.Create(() =>
                        {
                            // restore the original soft input mode
                            platformConfig.UseWindowSoftInputModeAdjust(softInputMode);
                        })
                        .DisposeWith(disposable);
                    }
                }
            });
        }
    }
}