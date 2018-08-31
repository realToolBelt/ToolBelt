using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditableProfilePage : ContentPageBase<EditableProfilePageViewModel>
    {
        public EditableProfilePage()
        {
            using (this.Log().Perf($"{nameof(EditableProfilePage)}: Initialize component."))
            {
                InitializeComponent();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(EditableProfilePage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.FirstName, v => v._emailControl.ViewModel)
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
                        .OneWayBind(ViewModel, vm => vm.Photo, v => v._imgPhoto.Source, photo => photo == null ? null : ImageSource.FromStream(() => photo))
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.ChangePhoto, v => v._btnPhoto)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Communities, v => v._rptCommunities.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.BirthDate.Value, v => v._birthDatePicker.NullableDate)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Save, v => v._miSave)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Cancel, v => v._miCancel)
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

                    //this
                    //    .WhenAnyObservable(v => v.ViewModel.Communities.ShouldReset)
                    //    .Select(_ => ViewModel.Communities)
                    //    .Merge(this.WhenAnyValue(v => v.ViewModel.Communities))
                    //    .BindTo(this, v => v._rptCommunities.ItemsSource)
                    //    .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.SelectCommunities, v => v._btnSelectCommunities)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}