using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.ViewModels;
using ToolBelt.Views.Authentication.Registration;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel
    {
        private bool _agreeWithTermsAndConditions;

        public SignupPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService) : base(navigationService)
        {
            Title = "Sign Up";

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await dialogService.DisplayAlertAsync("Missing information", "You must agree to the terms and conditions", "OK").ConfigureAwait(false);
                }

                // TODO:
                await navigationService.NavigateAsync($"/NavigationPage/{nameof(BasicInformationPage)}").ConfigureAwait(false);
            });

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await dialogService.DisplayAlertAsync("Missing information", "You must agree to the terms and conditions", "OK").ConfigureAwait(false);
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
