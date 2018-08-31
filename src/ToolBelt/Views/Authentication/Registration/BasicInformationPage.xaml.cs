using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasicInformationPage : ContentPageBase<BasicInformationPageViewModel>
    {
        public BasicInformationPage()
        {
            using (this.Log().Perf($"{nameof(BasicInformationPage)}: Initialize component."))
            {
                InitializeComponent();
                _birthDateControl.MaximumDate = DateTime.Today;
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(BasicInformationPage)}: Activate."))
                {
                    this
                        .BindCommand(ViewModel, vm => vm.Next, v => v._btnNext)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.FirstName, v => v._firstNameControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.LastName, v => v._lastNameControl.ViewModel)
                        .DisposeWith(disposable);
                    this
                        .OneWayBind(ViewModel, vm => vm.Email, v => v._emailControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Phone, v => v._phoneControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.BirthDate, v => v._birthDateControl.ViewModel)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}