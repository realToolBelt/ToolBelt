using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using System.Reactive;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IContainerRegistry _containerRegistry;

        public LoginPageViewModel(
            INavigationService navigationService,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IUserDialogs dialogs,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
        {
            Title = "Login";
            _containerRegistry = containerRegistry;

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (await firebaseAuthService.SignInWithGoogle())
                {
                    // TODO: load the user
                    _containerRegistry.RegisterInstance<IUserService>(new UserService(
                        await new FakeUserDataStore().GetUserById("1234")));

                    await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
                }
                else
                {
                    await dialogs.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Error",
                            Message = "Login failed"
                        }).ConfigureAwait(false);

                    // TODO: Should log out?
                }
            });

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                // TODO:...
            });
        }

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            // TODO: Move this to ExtendedSplashView
            base.OnNavigatedTo(parameters);

            // clear the current authentication data from the container
            _containerRegistry.RegisterInstance<IUserService>(null);
        }
    }
}
