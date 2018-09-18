using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;
using ToolBelt.Views.Authentication.Registration;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel
    {
        private readonly IContainerRegistry _containerRegistry;
        private readonly IUserDataStore _userDataStore;
        private bool _agreeWithTermsAndConditions;

        public SignupPageViewModel(
            INavigationService navigationService,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IUserDialogs dialogService,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
        {
            Title = "Sign Up";
            _containerRegistry = containerRegistry;
            _userDataStore = userDataStore;

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await ShowTermsAndConditionsValidationMessage(dialogService).ConfigureAwait(false);
                    return;
                }

                if (await firebaseAuthService.SignInWithGoogle())
                {
                    var userId = firebaseAuthService.GetCurrentUserId();
                    var account = await new FakeUserDataStore().GetUserById(userId);

                    if (account != null)
                    {
                        // if the account is not null, the user is already registered. Just log them
                        // in and head to the home screen
                        _containerRegistry.RegisterInstance<IUserService>(new UserService(account));

                        await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        // The account does not yet exist. Go through the registration process
                        await NavigationService
                            .NavigateAsync(
                                $"/NavigationPage/{nameof(RegistrationTypeSelectionPage)}",
                                new NavigationParameters
                                {
                                    { "user_id", userId }
                                }).ConfigureAwait(false);
                    }
                }
            });

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await ShowTermsAndConditionsValidationMessage(dialogService).ConfigureAwait(false);
                    return;
                }

                await dialogService.AlertAsync("Coming Soon!").ConfigureAwait(false);
            });

            SignInWithTwitter = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await ShowTermsAndConditionsValidationMessage(dialogService).ConfigureAwait(false);
                    return;
                }

                await dialogService.AlertAsync("Coming Soon!").ConfigureAwait(false);
            });

            SignInWithGoogle.ThrownExceptions.Subscribe(error => System.Diagnostics.Debug.WriteLine(error.ToString()));
        }

        public bool AgreeWithTermsAndConditions
        {
            get => _agreeWithTermsAndConditions;
            set => this.RaiseAndSetIfChanged(ref _agreeWithTermsAndConditions, value);
        }

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }

        public ReactiveCommand<Unit, Unit> SignInWithTwitter { get; }

        private static async Task ShowTermsAndConditionsValidationMessage(IUserDialogs dialogService)
        {
            await dialogService.AlertAsync(
                new AlertConfig
                {
                    Title = "Missing information",
                    Message = "You must agree to the terms and conditions",
                    OkText = "OK"
                }).ConfigureAwait(false);
        }
    }
}
