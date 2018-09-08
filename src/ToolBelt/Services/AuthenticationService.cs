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
        // TODO: Should be INPC...I think...
    public class User
    {
        public int Id { get; set; }

        public AuthToken Token { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }
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
            return Task.FromResult((User)null);

            return Task.FromResult(new User
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@fakeemail.com"
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
}
