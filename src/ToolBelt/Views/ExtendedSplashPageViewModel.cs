using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ExtendedSplashPageViewModel : BaseViewModel
    {
        private readonly IContainerRegistry _containerRegistry;
        private readonly IUserDialogs _dialogService;
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public ExtendedSplashPageViewModel(
            INavigationService navigationService,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IUserDialogs dialogService,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
        {
            _containerRegistry = containerRegistry;
            _dialogService = dialogService;

            Initialize = ReactiveCommand.CreateFromTask(async () =>
            {
                if (firebaseAuthService.IsUserSigned())
                {
                    var currentUserId = firebaseAuthService.GetCurrentUserId();
                    if (string.IsNullOrWhiteSpace(currentUserId))
                    {
                        await firebaseAuthService.Logout().ConfigureAwait(false);
                        await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
                    }

                    // try to get the user from our database
                    var user = await userDataStore.GetUserById(currentUserId).ConfigureAwait(false);
                    if (user == null)
                    {
                        await firebaseAuthService.Logout().ConfigureAwait(false);
                        await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        _containerRegistry.RegisterInstance<IUserService>(new UserService(user));

                        await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
                }
            });

            // when the command is executing, update the busy state
            this.WhenAnyObservable(x => x.Initialize.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);
        }

        public ReactiveCommand Initialize { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;
    }
}