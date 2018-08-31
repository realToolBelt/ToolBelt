using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
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
                _birthDateControl.MaximumDate = DateTime.Today;
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
                        .BindCommand(ViewModel, vm => vm.Save, v => v._miSave)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Cancel, v => v._miCancel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.BirthDate, v => v._birthDateControl.ViewModel)
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