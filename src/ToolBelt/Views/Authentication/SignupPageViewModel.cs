using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System.Reactive;
using ToolBelt.ViewModels;
using ToolBelt.Views.Authentication.Registration;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel
    {
        private bool _agreeWithTermsAndConditions;

        public SignupPageViewModel(
            INavigationService navigationService,
            IUserDialogs dialogService) : base(navigationService)
        {
            Title = "Sign Up";

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Missing information",
                            Message = "You must agree to the terms and conditions",
                            OkText = "OK"
                        }).ConfigureAwait(false);
                }

                // TODO:
                await navigationService.NavigateAsync($"/NavigationPage/{nameof(BasicInformationPage)}").ConfigureAwait(false);
            });

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Missing information",
                            Message = "You must agree to the terms and conditions",
                            OkText = "OK"
                        }).ConfigureAwait(false);
                }

                // TODO:
                await navigationService.NavigateAsync($"/NavigationPage/{nameof(BasicInformationPage)}").ConfigureAwait(false);
            });
        }

        public bool AgreeWithTermsAndConditions
        {
            get => _agreeWithTermsAndConditions;
            set => this.RaiseAndSetIfChanged(ref _agreeWithTermsAndConditions, value);
        }

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }
    }
}
