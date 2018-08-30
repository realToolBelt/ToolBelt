using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace ToolBelt.Services.Authentication
{
    /*
     * "https://www.googleapis.com/auth/userinfo.email",
    "https://www.googleapis.com/auth/userinfo.profile"
    */

    public class GoogleAuthenticator : AuthenticatorBase
    {
        private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const bool IsUsingNativeUI = true;

        public GoogleAuthenticator(
            string clientId,
            string scope,
            string redirectUrl,
            IAuthenticationDelegate authenticationDelegate) : base(AuthenticationProviderType.Google, authenticationDelegate)
        {
            Store = AccountStore.Create();
            Authenticator = new OAuth2Authenticator(
                clientId,
                string.Empty,
                scope,
                new Uri(AuthorizeUrl),
                new Uri(redirectUrl),
                new Uri(AccessTokenUrl),
                null,
                IsUsingNativeUI);

            Authenticator.Completed += OnAuthenticationCompleted;
            Authenticator.Error += OnAuthenticationFailed;
        }

        private AccountStore Store { get; }

        public AuthenticationProviderUser GetAuthenticatedUser(Account account)
        {
            var value = JObject.Parse(JsonWebTokenConvert.Decode(account.Properties["id_token"], (byte[])null, verify: false));
            return new AuthenticationProviderUser
            {
                Id = value.GetValue("sub").Value<string>(),
                ProviderType = ProviderType
            };
        }

        public override async Task LogOut()
        {
            var account = (await Store.FindAccountsForServiceAsync(ProviderType.ToString()).ConfigureAwait(false)).FirstOrDefault();
            if (account != null)
            {
                await Store.DeleteAsync(account, ProviderType.ToString()).ConfigureAwait(false);
            }
        }

        public override async Task<bool> UserIsAuthenticatedAndValidAsync(Account account)
        {
            var refreshToken = account.Properties["refresh_token"];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return false;
            }

            try
            {
                var result = await Authenticator.RequestAccessTokenAsync(
                    new Dictionary<string, string>
                    {
                        {"refresh_token", refreshToken},
                        {"client_id", Authenticator.ClientId},
                        {"grant_type", "refresh_token"},
                        {"client_secret", Authenticator.ClientSecret},
                    });

                var tokens = new[]
                {
                    "access_token",
                    "refresh_token",
                    "token_type",
                    "expires_in",
                    "scope",
                    "id_token",
                };

                foreach (var tokenName in tokens)
                {
                    if (result.ContainsKey(tokenName))
                    {
                        account.Properties[tokenName] = result[tokenName];
                    }
                }

                var token = new OAuthToken
                {
                    TokenType = account.Properties["token_type"],
                    AccessToken = account.Properties["access_token"],
                    ExpiresIn = long.Parse(account.Properties["expires_in"]),
                    Resource = account.Properties["scope"],
                    IdToken = account.Properties["id_token"],
                    RefreshToken = account.Properties["refresh_token"]
                };

                await Store.SaveAsync(account, ProviderType.ToString()).ConfigureAwait(false);
                AuthenticationDelegate?.OnAuthenticationCompleted(token, GetAuthenticatedUser(account));

                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log the exception?
                AuthenticationDelegate?.OnAuthenticationFailed(ex.Message, ex);
                return false;
            }
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = new OAuthToken
                {
                    TokenType = e.Account.Properties["token_type"],
                    AccessToken = e.Account.Properties["access_token"],
                    ExpiresIn = long.Parse(e.Account.Properties["expires_in"]),
                    Resource = e.Account.Properties["scope"],
                    IdToken = e.Account.Properties["id_token"],
                    RefreshToken = e.Account.Properties["refresh_token"]
                };

                Store.Save(e.Account, ProviderType.ToString());

                AuthenticationDelegate?.OnAuthenticationCompleted(token, GetAuthenticatedUser(e.Account));
            }
            else
            {
                AuthenticationDelegate?.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            AuthenticationDelegate?.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}
