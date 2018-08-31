using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
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
                _birthDatePicker.MaximumDate = DateTime.Today;
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
                        .Bind(ViewModel, vm => vm.BirthDate.Value, v => v._birthDatePicker.NullableDate)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.BirthDate.IsValid, v => v._birthDatePicker.IsValid)
                        .DisposeWith(disposable);

                    var errorsChanged = this
                        .WhenAnyObservable(v => v.ViewModel.BirthDate.Errors.Changed)
                        .Select(_ => ViewModel.BirthDate.Errors)
                        .Publish()
                        .RefCount();

                    errorsChanged
                        .Select(errors => errors.FirstOrDefault())
                        .StartWith(ViewModel?.BirthDate.Errors ?? Enumerable.Empty<string>())
                        .BindTo(this, v => v._lblBirthDateError.Text)
                        .DisposeWith(disposable);

                    errorsChanged
                        .Select(errors => errors.Count > 0)
                        .StartWith(ViewModel?.BirthDate.Errors.Count > 0)
                        .BindTo(this, v => v._lblBirthDateError.IsVisible)
                        .DisposeWith(disposable);

                    _birthDatePicker
                        .Events()
                        .Focused
                        .Where(args => args.IsFocused)
                        .Subscribe(_ => ViewModel.BirthDate.ClearValidationErrors())
                        .DisposeWith(disposable);
                }
            });
        }
    }
}