using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using ToolBelt.Services;
using ToolBelt.Services.Authentication;
using ToolBelt.ViewModels;
using ToolBelt.Views.Authentication.Registration;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel, IAuthenticationDelegate
    {
        private readonly IAuthenticatorFactory _authenticatorFactory;
        private readonly IContainerRegistry _containerRegistry;
        private readonly IUserDataStore _userDataStore;
        private bool _agreeWithTermsAndConditions;

        public SignupPageViewModel(
            INavigationService navigationService,
            IAuthenticatorFactory authenticatorFactory,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IUserDialogs dialogService) : base(navigationService)
        {
            Title = "Sign Up";
            _authenticatorFactory = authenticatorFactory;
            _containerRegistry = containerRegistry;
            _userDataStore = userDataStore;

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

                    return;
                }

                var auth = _authenticatorFactory.GetAuthenticationService(AuthenticationProviderType.Google, this);
                AuthenticationState.Authenticator = auth;
                await Authenticate.Handle(auth);
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

                    return;
                }

                var auth = _authenticatorFactory.GetAuthenticationService(AuthenticationProviderType.Facebook, this);
                AuthenticationState.Authenticator = auth;
                await Authenticate.Handle(auth);
            });

            SignInWithGoogle.ThrownExceptions.Subscribe(error => System.Diagnostics.Debug.WriteLine(error.ToString()));
        }

        public bool AgreeWithTermsAndConditions
        {
            get => _agreeWithTermsAndConditions;
            set => this.RaiseAndSetIfChanged(ref _agreeWithTermsAndConditions, value);
        }

        public Interaction<IAuthenticator, Unit> Authenticate { get; } = new Interaction<IAuthenticator, Unit>();

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }

        void IAuthenticationDelegate.OnAuthenticationCanceled()
        {
            AuthenticationState.Authenticator = null;
        }

        async void IAuthenticationDelegate.OnAuthenticationCompleted(OAuthToken token, AuthenticationProviderUser providerUser)
        {
            _containerRegistry.RegisterInstance<IAuthenticator>(AuthenticationState.Authenticator);

            var user = await _userDataStore.GetUserFromProvider(providerUser);
            if (user == null)
            {
                user = new User
                {
                    Email = providerUser.Email,
                    Token = AuthToken.FromOAuthToken(token),
                    AccountType = Models.AccountType.Contractor, // NOTE: Hard-coded for now...
                };

                // TODO: Save the account here?

                await NavigationService
                    .NavigateAsync(
                        $"/NavigationPage/{nameof(TradeSpecialtiesPage)}",
                        new NavigationParameters
                        {
                            { "user", user }
                        }).ConfigureAwait(false);
            }
            else
            {
                // if the user is an existing user, we should let them know they're already registered... Should we just sign in?
                //_containerRegistry.RegisterInstance<IUserService>(new UserService(user));

                //// the user is already registered. Show the main page.
                //await NavigationService.NavigateAsync($"/Root/Details/{nameof(MainPage)}").ConfigureAwait(false);
            }
        }

        void IAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            // TODO: Show message here...
            AuthenticationState.Authenticator = null;
        }
    }
}
