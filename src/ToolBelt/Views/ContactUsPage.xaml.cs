using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUsPage : ContentPageBase<ContactUsPageViewModel>
    {
        public ContactUsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .BindCommand(ViewModel, vm => vm.Submit, v => v._btnSubmit)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Name, v => v._txtName.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Email, v => v._txtEmail.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Message, v => v._txtMessage.Text)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                    .DisposeWith(disposable);
            });
        }
    }
}