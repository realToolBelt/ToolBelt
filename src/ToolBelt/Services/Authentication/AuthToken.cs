using System;

namespace ToolBelt.Services.Authentication
{
    public class AuthToken
    {
        public string AccessToken { get; set; }

        public DateTime ExpiresOn { get; set; }

        public string RefreshToken { get; set; }

        public static AuthToken FromOAuthToken(OAuthToken token)
        {
            return new AuthToken
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpiresOn = DateTime.Now.AddSeconds(token.ExpiresIn)
            };
        }

        public override string ToString()
        {
            return string.Format("ExpiresOn: {0}, AccessToken: {1}, RefreshToken: {2}", ExpiresOn, AccessToken, RefreshToken);
        }
    }
}
