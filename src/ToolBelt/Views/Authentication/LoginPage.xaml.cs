using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPageBase<LoginPageViewModel>
    {
        public LoginPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            using (this.Log().Perf($"{nameof(LoginPage)}: Initialize component."))
            {
                InitializeComponent();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(LoginPage)}: Activate."))
                {
                    this
                        .BindCommand(ViewModel, vm => vm.SignInWithGoogle, v => v._btnGoogle)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.SignInWithFacebook, v => v._btnFacebook)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.SignInWithTwitter, v => v._btnTwitter)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}