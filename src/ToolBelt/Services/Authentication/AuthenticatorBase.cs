using System;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace ToolBelt.Services.Authentication
{
    /// <summary>
    /// Base class for authenticators.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.Authentication.IAuthenticator" />
    public abstract class AuthenticatorBase : IAuthenticator
    {
        protected AuthenticatorBase(
            AuthenticationProviderType providerType,
            IAuthenticationDelegate authenticationDelegate)
        {
            ProviderType = providerType;
            AuthenticationDelegate = authenticationDelegate;
        }

        public IAuthenticationDelegate AuthenticationDelegate { get; }

        public OAuth2Authenticator Authenticator { get; protected set; }

        public AuthenticationProviderType ProviderType { get; }

        public abstract Task LogOut();

        public void OnPageLoading(Uri uri)
        {
            Authenticator.OnPageLoading(uri);
        }

        public abstract Task<bool> UserIsAuthenticatedAndValidAsync(Account account);
    }
}
