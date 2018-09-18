using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPageBase<ProfilePageViewModel>
    {
        public ProfilePage()
        {
            using (this.Log().Perf($"{nameof(ProfilePage)}: Initialize component."))
            {
                InitializeComponent();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ProfilePage)}: Activate."))
                {
                    this
                        .BindCommand(ViewModel, vm => vm.Edit, v => v._miEdit)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.User.EmailAddress, v => v._lblEmail.Text)
                        .DisposeWith(disposable);

                    //this
                    //    .OneWayBind(ViewModel, vm => vm.User.Name, v => v._lblFirstName.Text)
                    //    .DisposeWith(disposable);
                }
            });
        }
    }
}