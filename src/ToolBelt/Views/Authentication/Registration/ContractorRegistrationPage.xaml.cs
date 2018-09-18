using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractorRegistrationPage : ContentPageBase<ContractorRegistrationPageViewModel>
    {
        public ContractorRegistrationPage()
        {
            using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Activate."))
                {
                    this
                        .Bind(ViewModel, vm => vm.CompanyName, v => v._companyName.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.CompanyEmail, v => v._companyEmail.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.CompanyUrl, v => v._companyUrl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.AddressLineOne, v => v._addressLineOne.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.AddressLineTwo, v => v._addressLineTwo.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.City, v => v._addressCity.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.State, v => v._addressState.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.Zip, v => v._addressZip.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Save, v => v._btnSave)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}