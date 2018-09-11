namespace ToolBelt.Services.Authentication
{
    /// <summary>
    /// An authenticated user that comes from the authentication provider.
    /// </summary>
    public class AuthenticationProviderUser
    {
        /// <summary>
        /// Gets or sets the email address for the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the unique id for the user from the authentication provider.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the provider that the user comes from.
        /// </summary>
        public AuthenticationProviderType ProviderType { get; set; }
    }
}
