using Prism.Navigation;
using System.Threading.Tasks;
using ToolBelt.Views;
using ToolBelt.Views.Authentication;
using Xamarin.Forms;

namespace ToolBelt.Extensions
{
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Navigates to the home page asynchronously.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task NavigateHomeAsync(this INavigationService navigationService)
        {
            await navigationService
                .NavigateAsync($"/Root/Details/{nameof(MainPage)}")
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Navigates to login page asynchronously.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task NavigateToLoginPageAsync(this INavigationService navigationService)
        {
            await navigationService
                .NavigateAsync($"/{nameof(NavigationPage)}/{nameof(AuthenticationTabbedPage)}?createTab={nameof(LoginPage)}&createTab={nameof(SignupPage)}")
                .ConfigureAwait(false);
        }
    }
}
