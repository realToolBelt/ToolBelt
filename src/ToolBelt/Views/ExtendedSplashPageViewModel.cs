using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    /// <summary>
    /// View-model for the extended splash screen. This view-model determines whether a user is
    /// already logged in or not and routes them to the appropriate screen (home screen, login, ...).
    /// </summary>
    /// <seealso cref="ToolBelt.ViewModels.BaseViewModel" />
    public class ExtendedSplashPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public ExtendedSplashPageViewModel(
            INavigationService navigationService,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
        {
            const int Home = 1;
            const int Login = 2;

            Initialize = ReactiveCommand.CreateFromTask(async () =>
            {
                // make sure we're running this on a background thread
                this.Log().Debug($"Loading projects on thread: {Thread.CurrentThread.ManagedThreadId}, IsBackground = {Thread.CurrentThread.IsBackground}");
                AssertRunningOnBackgroundThread();

                if (firebaseAuthService.IsUserSigned())
                {
                    var currentUserId = firebaseAuthService.GetCurrentUserId();
                    if (string.IsNullOrWhiteSpace(currentUserId))
                    {
                        await firebaseAuthService.Logout();
                        return Login;
                    }

                    // try to get the user from our database
                    var user = await userDataStore.GetUserById(currentUserId);
                    if (user == null)
                    {
                        await firebaseAuthService.Logout();
                        return Login;
                    }

                    containerRegistry.RegisterInstance<IUserService>(new UserService(user));
                    return Home;
                }

                return Login;
            });

            Initialize
                .SubscribeOn(RxApp.MainThreadScheduler)
                .SelectMany(async nextPage =>
                {
                    if (nextPage == Home)
                    {
                        await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
                    }

                    return Observable.Return(Unit.Default);
                })
                .Subscribe();

            // when the command is executing, update the busy state
            Initialize.IsExecuting
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);

            Initialize.ThrownExceptions
                .Subscribe(exception => System.Diagnostics.Debug.WriteLine($"Error: {exception}"));
        }

        public ReactiveCommand<Unit, int> Initialize { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;
    }
}