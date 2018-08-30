using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPageBase<SignupPageViewModel>
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .Bind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Name, v => v._txtName.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Email, v => v._txtEmail.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Password, v => v._txtPassword.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.PasswordConfirm, v => v._txtPasswordConfirm.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.AgreeWithTermsAndConditions, v => v._chkAgreeWithTerms.IsToggled)
                    .DisposeWith(disposable);

                this
                    .BindCommand(ViewModel, vm => vm.Register, v => v._btnRegister)
                    .DisposeWith(disposable);
            });
        }
    }
}