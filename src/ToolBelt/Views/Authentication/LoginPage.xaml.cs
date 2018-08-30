using ReactiveUI;
using Splat;
using System.Reactive;
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
                        .OneWayBind(ViewModel, vm => vm.SignInWithGoogle, v => v._btnGoogle.Command)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.SignInWithFacebook, v => v._btnFacebook.Command)
                        .DisposeWith(disposable);

                    ViewModel.Authenticate
                        .RegisterHandler(context =>
                        {
                            // show the login pages
                            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                            presenter.Login(context.Input.Authenticator);

                            // set the output to mark this as done
                            context.SetOutput(Unit.Default);
                        })
                        .DisposeWith(disposable);
                }
            });
        }
    }
}