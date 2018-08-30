using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using ToolBelt.Services;
using ToolBelt.Services.Authentication;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication
{
    public class LoginPageViewModel : BaseViewModel, IAuthenticationDelegate
    {
        private readonly IAuthenticatorFactory _authenticatorFactory;
        private readonly IContainerRegistry _containerRegistry;
        private readonly IUserDataStore _userDataStore;

        public LoginPageViewModel(
            INavigationService navigationService,
            IAuthenticatorFactory authenticatorFactory,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore) : base(navigationService)
        {
            Title = "Login";
            _authenticatorFactory = authenticatorFactory;
            _containerRegistry = containerRegistry;
            _userDataStore = userDataStore;

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                var auth = _authenticatorFactory.GetAuthenticationService(AuthenticationProviderType.Google, this);
                AuthenticationState.Authenticator = auth;
                await Authenticate.Handle(auth);
            });

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                var auth = _authenticatorFactory.GetAuthenticationService(AuthenticationProviderType.Facebook, this);
                AuthenticationState.Authenticator = auth;
                await Authenticate.Handle(auth);
            });
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
                // TODO: New user.  Show the registration flow
            }
            else
            {
                _containerRegistry.RegisterInstance<IUserService>(new UserService(user));

                // the user is already registered. Show the main page.
                await NavigationService.NavigateAsync($"/Root/Details/{nameof(MainPage)}").ConfigureAwait(false);
            }
        }

        void IAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            // TODO: Show message here...
            AuthenticationState.Authenticator = null;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            // TODO: Move this to ExtendedSplashView
            base.OnNavigatedTo(parameters);

            // clear the current authentication data from the container
            _containerRegistry.RegisterInstance<IAuthenticator>(null);
            _containerRegistry.RegisterInstance<IUserService>(null);
        }
    }
}
