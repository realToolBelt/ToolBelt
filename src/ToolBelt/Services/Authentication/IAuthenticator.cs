using System;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace ToolBelt.Services.Authentication
{
    public interface IAuthenticator
    {
        /// <summary>
        /// Gets the authenticator associated with this instance.
        /// </summary>
        OAuth2Authenticator Authenticator { get; }

        /// <summary>
        /// Gets the type of the provider.
        /// </summary>
        AuthenticationProviderType ProviderType { get; }

        /// <summary>
        /// Logs the user out of the provider.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        Task LogOut();

        /// <summary>
        /// Called by the device projects when a page is loading to continue the OAuth flow.
        /// </summary>
        /// <param name="uri">The URI of the page being loaded..</param>
        void OnPageLoading(Uri uri);

        /// <summary>
        /// Determines whether the user is authenticated and is valid.
        /// </summary>
        /// <param name="account">The account to authenticate.</param>
        /// <returns><c>true</c> if the user is authenticated and valid; otherwise, <c>false</c>.</returns>
        Task<bool> UserIsAuthenticatedAndValidAsync(Account account);
    }
}
