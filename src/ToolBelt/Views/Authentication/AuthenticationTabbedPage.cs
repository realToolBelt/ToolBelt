using Xamarin.Forms;

namespace ToolBelt.Views.Authentication
{
    /// <summary>
    /// Simple tab page to wrap the authentication pages so we can handle certain options.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.TabbedPage" />
    public class AuthenticationTabbedPage : TabbedPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationTabbedPage" /> class.
        /// </summary>
        public AuthenticationTabbedPage()
        {
            // turn off the navigation bar for this page.
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
