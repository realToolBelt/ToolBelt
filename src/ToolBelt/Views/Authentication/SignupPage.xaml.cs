using ReactiveUI;
using Splat;
using System.Reactive;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPageBase<SignupPageViewModel>
    {
        public SignupPage()
        {
            using (this.Log().Perf($"{nameof(SignupPage)}: Initialize component."))
            {
                InitializeComponent();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(SignupPage)}: Activate."))
                {
                    this
                        .Bind(ViewModel, vm => vm.AgreeWithTermsAndConditions, v => v._chkAgreeWithTerms.IsToggled)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.SignInWithGoogle, v => v._btnGoogle)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.SignInWithFacebook, v => v._btnFacebook)
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