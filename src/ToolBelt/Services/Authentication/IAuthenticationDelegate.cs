using System;

namespace ToolBelt.Services.Authentication
{
    public interface IAuthenticationDelegate
    {
        /// <summary>
        /// Called when authentication is canceled.
        /// </summary>
        void OnAuthenticationCanceled();

        /// <summary>
        /// Called when authentication is completed.
        /// </summary>
        /// <param name="token">The token containing the authentication data.</param>
        /// <param name="providerUser">The user information for the user associated with the provider.</param>
        void OnAuthenticationCompleted(OAuthToken token, AuthenticationProviderUser providerUser);

        /// <summary>
        /// Called when authentication fails.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void OnAuthenticationFailed(string message, Exception exception);
    }
}
