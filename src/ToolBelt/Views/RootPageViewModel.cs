using Prism.Navigation;
using ReactiveUI;
using System.Reactive;
using ToolBelt.Extensions;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.Services.Authentication;
using ToolBelt.ViewModels;
using ToolBelt.Views.About;
using ToolBelt.Views.Profile;

namespace ToolBelt.Views
{
    public class RootPageViewModel : BaseViewModel
    {
        public RootPageViewModel(
            INavigationService navigationService,
            IAuthenticator authenticator,
            IUserService userService) : base(navigationService)
        {
            User = userService.AuthenticatedUser;

            MenuItems = new ReactiveList<CustomMenuItem>
            {
                new CustomMenuItem
                {
                    Title = "Home",
                    IconSource = "\xf015",
                    TapCommand = CreateNavigationCommand($"Details/{nameof(MainPage)}?page={nameof(ProjectsPage)}")
                },
                new CustomMenuItem
                {
                    Title = "Messages",
                    IconSource = "\xf0e0",
                    TapCommand = CreateNavigationCommand($"Details/{nameof(MainPage)}?page={nameof(Messages.MessagesPage)}")
                },
                new CustomMenuItem
                {
                    Title = "Communities",
                    IconSource = "\xf0c0",
                    TapCommand = CreateNavigationCommand($"Details/{nameof(CommunitiesPage)}")
                },
                new CustomMenuItem
                {
                    Title = "Explore",
                    IconSource = "\xf002"
                },
                new CustomMenuItem
                {
                    Title = "Contact Us",
                    IconSource = "\xf007",
                    TapCommand = CreateNavigationCommand($"{nameof(ModalNavigationPage)}/{nameof(ContactUsPage)}", useModalNavigation: true)
                },
                new CustomMenuItem
                {
                    Title = "About Us",
                    IconSource = "\xf05a",
                    TapCommand = CreateNavigationCommand($"{nameof(ModalNavigationPage)}/{nameof(AboutUsPage)}", useModalNavigation: true)
                },
                new CustomMenuItem
                {
                    Title = "Privacy Policy",
                    IconSource = "\xf3ed",
                    TapCommand = CreateNavigationCommand($"{nameof(ModalNavigationPage)}/{nameof(PrivacyPolicyPage)}", useModalNavigation: true)
                },
                new CustomMenuItem
                {
                    Title = "Sign Out",
                    IconSource = "\xf2f5",
                    TapCommand = ReactiveCommand.CreateFromTask(async () =>
                    {
                        await authenticator.LogOut();
                        await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
                    })
                }
            };

            ViewProfile = ReactiveCommand.CreateFromTask(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "user", User }
                };

                await NavigationService
                    .NavigateAsync($"NavigationPage/{nameof(ProfileViewTabbedPage)}", parameters, useModalNavigation: true)
                    .ConfigureAwait(false);
            });

            ReactiveCommand<Unit, Unit> CreateNavigationCommand(string path, bool? useModalNavigation = null)
            {
                return ReactiveCommand.CreateFromTask(async () =>
                    await NavigationService
                        .NavigateAsync(path, useModalNavigation: useModalNavigation)
                        .ConfigureAwait(false));
            }
        }

        public ReactiveList<CustomMenuItem> MenuItems { get; }

        public User User { get; }

        public ReactiveCommand ViewProfile { get; }
    }
}
