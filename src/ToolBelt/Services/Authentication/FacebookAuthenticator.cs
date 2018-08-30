using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace ToolBelt.Services.Authentication
{
    public class FacebookAuthenticator : AuthenticatorBase
    {
        private const string AuthorizeUrl = "https://www.facebook.com/dialog/oauth/";
        private const bool IsUsingNativeUI = false;
        private const string RedirectUrl = "https://www.facebook.com/connect/login_success.html";

        public FacebookAuthenticator(string clientId, string scope, IAuthenticationDelegate authenticationDelegate) : base(AuthenticationProviderType.Facebook, authenticationDelegate)
        {
            Authenticator = new OAuth2Authenticator(
                clientId,
                scope,
                new Uri(AuthorizeUrl),
                new Uri(RedirectUrl),
                null,
                IsUsingNativeUI);

            Authenticator.Completed += OnAuthenticationCompleted;
            Authenticator.Error += OnAuthenticationFailed;
        }

        public override async Task LogOut()
        {
            var store = AccountStore.Create();
            var account = store.FindAccountsForService(ProviderType.ToString()).FirstOrDefault();
            if (account != null)
            {
                await store.DeleteAsync(account, ProviderType.ToString()).ConfigureAwait(false);
            }
        }

        public override Task<bool> UserIsAuthenticatedAndValidAsync(Account account)
        {
            // TODO: Try to refresh the token...
            return Task.FromResult(false);
        }

        private async Task<string> GetFacebookUserIdAsync(Account account)
        {
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account);
            var userInfoResult = await request.GetResponseAsync();
            var obj = JObject.Parse(userInfoResult.GetResponseText());

            //this.Log().Info(obj.ToString());
            if (obj != null)
            {
                return obj.GetValue("id").ToString();
            }

            return null;
        }

        private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = new OAuthToken
                {
                    AccessToken = e.Account.Properties["access_token"]
                };

                AuthenticationProviderUser user = null;
                try
                {
                    user = new AuthenticationProviderUser
                    {
                        Id = await GetFacebookUserIdAsync(e.Account),
                        ProviderType = ProviderType
                    };

                    if (user == null)
                    {
                        AuthenticationDelegate?.OnAuthenticationFailed("Could not get user information", new Exception("Could not get user information"));
                    }
                }
                catch (Exception ex)
                {
                    AuthenticationDelegate?.OnAuthenticationFailed(ex.Message, ex);
                }

                AccountStore.Create().Save(e.Account, ProviderType.ToString());
                AuthenticationDelegate?.OnAuthenticationCompleted(token, user);
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
