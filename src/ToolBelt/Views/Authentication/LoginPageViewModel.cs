using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;
using ToolBelt.Views.Authentication.Registration;

namespace ToolBelt.Views.Authentication
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IContainerRegistry _containerRegistry;
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public LoginPageViewModel(
            INavigationService navigationService,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IUserDialogs dialogs,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
        {
            Title = "Login";
            _containerRegistry = containerRegistry;

            // we want to disable all commands when any one is running. We'll do that by using a
            // behavior subject to play all of the command events through
            BehaviorSubject<bool> canExecute = new BehaviorSubject<bool>(true);

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (await firebaseAuthService.SignInWithGoogle())
                {
                    var userId = firebaseAuthService.GetCurrentUserId();
                    var account = await new FakeUserDataStore().GetUserById(userId);
                    account = null; // TODO: Remove this

                    if (account == null)
                    {
                        // if the account doesn't exist, prompt the user to register
                        var shouldSignUp = await dialogs.ConfirmAsync(
                            new ConfirmConfig
                            {
                                Message = "The account does not appear to exist.  Would you like to sign up?",
                                OkText = "Sign Up!"
                            });
                        if (shouldSignUp)
                        {
                            await NavigationService
                                .NavigateAsync(
                                    nameof(RegistrationTypeSelectionPage),
                                    new NavigationParameters
                                    {
                                        { "user_id", userId }
                                    }).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        // the account exists. Head to the home page
                        _containerRegistry.RegisterInstance<IUserService>(new UserService(account));

                        await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    await dialogs.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Error",
                            Message = "Login failed"
                        }).ConfigureAwait(false);

                    // TODO: Should call log out just to be safe?
                }
            },
            canExecute);

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                await dialogs.AlertAsync("Coming Soon!").ConfigureAwait(false);
            },
            canExecute);

            SignInWithTwitter = ReactiveCommand.CreateFromTask(async () =>
            {
                await dialogs.AlertAsync("Coming Soon!").ConfigureAwait(false);
            },
            canExecute);

            var commandsExecuting = this.WhenAnyObservable(
                x => x.SignInWithGoogle.IsExecuting,
                x => x.SignInWithFacebook.IsExecuting,
                x => x.SignInWithTwitter.IsExecuting,
                (googleExecuting, facebookExecuting, twitterExecuting) => googleExecuting || facebookExecuting || twitterExecuting)
                .DistinctUntilChanged()
                .Publish()
                .RefCount();

            // when any of the commands are executing, update the busy state
            commandsExecuting
                .StartWith(false)
                .ToProperty(this, x => x.IsBusy, out _isBusy, scheduler: RxApp.MainThreadScheduler);

            // when any of the commands are executing, update the "can execute" state
            commandsExecuting
                .Select(isExecuting => !isExecuting)
                .Subscribe(canExecute);

            // When an exception is thrown from a command, log the error and let the user handle the exception
            SignInWithGoogle.ThrownExceptions
                .Merge(SignInWithFacebook.ThrownExceptions)
                .Merge(SignInWithTwitter.ThrownExceptions)
                .SelectMany(exception =>
                {
                    this.Log().ErrorException("Error logging in", exception);

                    //return SharedInteractions.Error.Handle(exception);
                    return Observable.Return(Unit.Default);
                })
                .Subscribe();
        }

        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }

        public ReactiveCommand<Unit, Unit> SignInWithTwitter { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            // TODO: Move this to ExtendedSplashView
            base.OnNavigatedTo(parameters);

            // clear the current authentication data from the container
            _containerRegistry.RegisterInstance<IUserService>(null);
        }
    }
}
