using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToolBelt.Services.Authentication;
using Xamarin.Auth;
using Xamarin.Forms;

namespace ToolBelt.Services
{


    // Google Android Client ID: 257760628057-c9r0419lehhcbhqprcvhue87i86hl422.apps.googleusercontent.com
    // Facebook app id: 1324833817652478



    /*
        https://github.com/xamarin/xamarin-forms-samples/tree/master/WebServices/OAuthNativeFlow/OAuthNativeFlow
        https://github.com/halkar/xamarin-forms-oauth/blob/master/Xamarin.Forms.OAuth/OAuthToken.cs
        https://github.com/halkar/xamarin-forms-oauth/blob/master/Xamarin.Forms.OAuth/AuthToken.cs



        https://github.com/moljac/Xamarin.Auth.Samples.NugetReferences/blob/master/Xamarin.Forms/Evolve16Labs/Portable/MainPage.xaml.cs#L161-L206
    */



    // https://timothelariviere.com/2017/09/01/authenticate-users-through-google-with-xamarin-auth/
    // https://timothelariviere.com/2017/10/11/authenticate-users-through-facebook-using-xamarin-auth/

        // TODO: Should be INPC...I think...
    public class User
    {
        public int Id { get; set; }

        public AuthToken Token { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime? BirthDate { get; set; }
    }

    public interface IUserService
    {
        User AuthenticatedUser { get; }
    }

    public class UserService : IUserService
    {
        public User AuthenticatedUser { get; }

        public UserService(User user)
        {
            AuthenticatedUser = user;
        }
    }



    public interface IUserDataStore
    {
        Task<User> GetUserFromProvider(AuthenticationProviderUser providerUser);
    }

    public class FakeUserDataStore : IUserDataStore
    {
        public Task<User> GetUserFromProvider(AuthenticationProviderUser providerUser)
        {
            return Task.FromResult(new User
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@fakeemail.com",
                BirthDate = new DateTime(1985, 6, 15)
            });
        }
    }

    public interface IAuthenticatorFactory
    {
        IAuthenticator GetAuthenticationService(AuthenticationProviderType provider, IAuthenticationDelegate authenticationDelegate);
    }

    public class AuthenticatorFactory : IAuthenticatorFactory
    {
        public IAuthenticator GetAuthenticationService(AuthenticationProviderType provider, IAuthenticationDelegate authenticationDelegate)
        {
            switch (provider)
            {
                case AuthenticationProviderType.Google:
                    return new GoogleAuthenticator(
                        "257760628057-c9r0419lehhcbhqprcvhue87i86hl422.apps.googleusercontent.com",
                        "email",
                        "com.toolbelt.toolbelt:/oauth2redirect",
                        authenticationDelegate);

                case AuthenticationProviderType.Facebook:
                    return new FacebookAuthenticator(
                    "1324833817652478",
                    "email",
                    authenticationDelegate);

                default:
                    throw new InvalidEnumArgumentException(
                        nameof(provider),
                        (int)provider,
                        typeof(AuthenticationProviderType));
            }
        }
    }


    


    public class AuthenticationState
    {
        /// <summary>
        /// The authenticator.
        /// </summary>
        // TODO:
        // Oauth1Authenticator inherits from WebAuthenticator
        // Oauth2Authenticator inherits from WebRedirectAuthenticator
        public static IAuthenticator Authenticator;
    }

    

    
    public class FacebookAuthenticator : AuthenticatorBase
    {
        private const string AuthorizeUrl = "https://www.facebook.com/dialog/oauth/";
        private const string RedirectUrl = "https://www.facebook.com/connect/login_success.html";
        private const bool IsUsingNativeUI = false;
        
        
        public override async Task LogOut()
        {
            var store = AccountStore.Create();
            var account = store.FindAccountsForService(ProviderType.ToString()).FirstOrDefault();
            if (account != null)
            {
                await store.DeleteAsync(account, ProviderType.ToString()).ConfigureAwait(false);
            }
        }

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


        public AuthenticationProviderUser GetAuthenticatedUser(Account account)
        {
            //var value = JObject.Parse(JsonWebTokenConvert.Decode(account.Properties["id_token"], (byte[])null, verify: false));
            //return new AuthenticationProviderUser
            //{
            //    Id = value.GetValue("sub").Value<string>(),
            //    ProviderType = ProviderType
            //};
            return null;
        }

        
        public override Task<bool> UserIsAuthenticatedAndValidAsync(Account account)
        {
            // TODO: Try to refresh the token...
            return Task.FromResult(false);
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = new OAuthToken
                {
                    AccessToken = e.Account.Properties["access_token"]
                };

                AccountStore.Create().Save(e.Account, ProviderType.ToString());
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
