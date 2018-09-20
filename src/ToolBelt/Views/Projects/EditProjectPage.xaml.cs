using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Models;
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
                        .OneWayBind(ViewModel, vm => vm.SkillsRequired, v => v._skillsRequiredControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Save, v => v._miSave)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Cancel, v => v._miCancel)
                        .DisposeWith(disposable);

                    BindPaymentControls(disposable);

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

        private void BindPaymentControls(CompositeDisposable disposable)
        {
            this
                .Bind(ViewModel, vm => vm.PayRate.Value, v => v._txtPayRate.Text)
                .DisposeWith(disposable);

            this
                .OneWayBind(ViewModel, vm => vm.PayRate.IsValid, v => v._txtPayRate.IsValid)
                .DisposeWith(disposable);

            var errorsChanged = this
                .WhenAnyObservable(v => v.ViewModel.PayRate.Errors.Changed)
                .Select(_ => ViewModel.PayRate.Errors)
                .Publish()
                .RefCount();

            errorsChanged
                .Select(errors => errors.FirstOrDefault())
                .StartWith(ViewModel?.PayRate.Errors ?? Enumerable.Empty<string>())
                .BindTo(this, v => v._lblPayRateError.Text)
                .DisposeWith(disposable);

            errorsChanged
                .Select(errors => errors.Count > 0)
                .StartWith(ViewModel?.PayRate.Errors.Count > 0)
                .BindTo(this, v => v._lblPayRateError.IsVisible)
                .DisposeWith(disposable);

            _txtPayRate
                .Events()
                .Focused
                .Where(args => args.IsFocused)
                .Subscribe(_ => ViewModel.PayRate.ClearValidationErrors())
                .DisposeWith(disposable);

            this
                .OneWayBind(ViewModel, vm => vm.PaymentTypes, v => v._pickPaymentType.ItemsSource)
                .DisposeWith(disposable);

            this
                .Bind(ViewModel, vm => vm.PaymentType.Value, v => v._pickPaymentType.SelectedItem)
                .DisposeWith(disposable);
        }
    }
}