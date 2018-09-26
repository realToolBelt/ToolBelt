using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Extensions;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;
using ToolBelt.Views.About;
using ToolBelt.Views.Profile;
using ToolBelt.Views.Projects;

namespace ToolBelt.Views
{
    public class RootPageViewModel : BaseViewModel
    {
        public RootPageViewModel(
            INavigationService navigationService,
            IUserService userService,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
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

                //new CustomMenuItem
                //{
                //    Title = "Messages",
                //    IconSource = "\xf0e0",
                //    TapCommand = CreateNavigationCommand($"Details/{nameof(Messages.MessagesPage)}")
                //},
                //new CustomMenuItem
                //{
                //    Title = "Communities",
                //    IconSource = "\xf0c0",
                //    TapCommand = CreateNavigationCommand($"Details/{nameof(CommunitiesPage)}")
                //},
                //new CustomMenuItem
                //{
                //    Title = "Explore",
                //    IconSource = "\xf002"
                //},
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
                        await firebaseAuthService.Logout();
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
                    .NavigateAsync($"Details/{nameof(ProfileViewTabbedPage)}", parameters)
                    .ConfigureAwait(false);
            });

            IObservable<Exception> commandObservable = null;
            foreach (var item in MenuItems.Where(x => x.TapCommand != null))
            {
                if (commandObservable == null)
                {
                    commandObservable = item.TapCommand.ThrownExceptions;
                }
                else
                {
                    commandObservable = commandObservable.Merge(item.TapCommand.ThrownExceptions);
                }
            }

            if (commandObservable != null)
            {
                commandObservable
                    .SelectMany(exception =>
                    {
                        this.Log().ErrorException("Error in menu items", exception);
                        return SharedInteractions.Error.Handle(exception);
                    })
                    .Subscribe();
            }

            ReactiveCommand<Unit, Unit> CreateNavigationCommand(string path, bool? useModalNavigation = null)
            {
                return ReactiveCommand.CreateFromTask(async () =>
                    await NavigationService
                        .NavigateAsync(path, useModalNavigation: useModalNavigation)
                        .ConfigureAwait(false));
            }
        }

        public ReactiveList<CustomMenuItem> MenuItems { get; }

        public Account User { get; }

        public ReactiveCommand ViewProfile { get; }
    }
}
