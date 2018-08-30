using Prism.Navigation;
using System.Threading.Tasks;
using ToolBelt.Views.Authentication;

namespace ToolBelt.Extensions
{
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Navigates to login page asynchronously.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task NavigateToLoginPageAsync(this INavigationService navigationService)
        {
            await navigationService
                .NavigateAsync($"/TabbedPage?createTab={nameof(LoginPage)}&createTab={nameof(SignupPage)}")
                .ConfigureAwait(false);
        }
    }
}
