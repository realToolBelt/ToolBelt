using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.Services.Authentication;
using ToolBelt.ViewModels;
using Xamarin.Auth;

namespace ToolBelt.Views
{
    public class ExtendedSplashPageViewModel : BaseViewModel, IAuthenticationDelegate
    {
        private readonly AccountStore _accountStore;
        private readonly IContainerRegistry _containerRegistry;
        private readonly IPageDialogService _dialogService;
        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private readonly IUserDataStore _userDataStore;

        public ExtendedSplashPageViewModel(
            INavigationService navigationService,
            IAuthenticatorFactory authenticatorFactory,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IPageDialogService dialogService) : base(navigationService)
        {
            _accountStore = AccountStore.Create();
            _containerRegistry = containerRegistry;
            _userDataStore = userDataStore;
            _dialogService = dialogService;

            Initialize = ReactiveCommand.CreateFromTask(async () =>
            {
                foreach (var provider in Enum.GetValues(typeof(AuthenticationProviderType)).Cast<AuthenticationProviderType>())
                {
                    var account = _accountStore.FindAccountsForService(provider.ToString()).FirstOrDefault();
                    if (account != null)
                    {
                        var authService = authenticatorFactory.GetAuthenticationService(provider, this);
                        if (authService != null)
                        {
                            AuthenticationState.Authenticator = authService;
                            bool result = await authService.UserIsAuthenticatedAndValidAsync(account).ConfigureAwait(false);
                            if (!result)
                            {
                                // Remove the account from the store
                                await authService.LogOut().ConfigureAwait(false);
                            }
                            else
                            {
                                // NOTE: We'll navigate to the main page in the authentication
                                // succeeded event
                                return;
                            }
                        }
                    }
                }

                // TODO: Should make this an extension method as the parameters are the same in ever case, and navigation is absolute
                // navigate to the login page in any other case
                await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
            });

            // when the command is executing, update the busy state
            this.WhenAnyObservable(x => x.Initialize.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);
        }

        public ReactiveCommand Initialize { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        void IAuthenticationDelegate.OnAuthenticationCanceled()
        {
            AuthenticationState.Authenticator = null;
        }

        async void IAuthenticationDelegate.OnAuthenticationCompleted(OAuthToken token, AuthenticationProviderUser providerUser)
        {
            var user = await _userDataStore.GetUserFromProvider(providerUser);
            if (user != null)
            {
                _containerRegistry.RegisterInstance<IAuthenticator>(AuthenticationState.Authenticator);
                _containerRegistry.RegisterInstance<IUserService>(new UserService(user));

                // the user is already registered. Show the main page.
                await NavigationService.NavigateAsync($"/Root/Details/{nameof(MainPage)}").ConfigureAwait(false);
            }
            else
            {
                // This should only happen if we successfully authenticate with the OAuth process,
                // but fail to retrieve the user from the database. Just show that login failed
                await _dialogService.DisplayAlertAsync("Login Failed", "Failed to log in.  Please try again.", "OK").ConfigureAwait(false);
            }
        }

        void IAuthenticationDelegate.OnAuthenticationFailed(string message, Exception exception)
        {
            // TODO: Show message here...
            AuthenticationState.Authenticator = null;
        }
    }
}