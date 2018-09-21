using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;
using ToolBelt.Views.Authentication.Registration;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel
    {
        private readonly IContainerRegistry _containerRegistry;
        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private readonly IUserDataStore _userDataStore;
        private bool _agreeWithTermsAndConditions;

        public SignupPageViewModel(
            INavigationService navigationService,
            IContainerRegistry containerRegistry,
            IUserDataStore userDataStore,
            IUserDialogs dialogService,
            IFirebaseAuthService firebaseAuthService) : base(navigationService)
        {
            Title = "Sign Up";
            _containerRegistry = containerRegistry;
            _userDataStore = userDataStore;

            // we want to disable all commands when any one is running. We'll do that by using a
            // behavior subject to play all of the command events through
            BehaviorSubject<bool> canExecute = new BehaviorSubject<bool>(true);

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await ShowTermsAndConditionsValidationMessage(dialogService).ConfigureAwait(false);
                    return;
                }

                if (await firebaseAuthService.SignInWithGoogle())
                {
                    var userId = firebaseAuthService.GetCurrentUserId();
                    var account = await new FakeUserDataStore().GetUserById(userId);

                    if (account != null)
                    {
                        // if the account is not null, the user is already registered. Just log them
                        // in and head to the home screen
                        _containerRegistry.RegisterInstance<IUserService>(new UserService(account));

                        await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        // The account does not yet exist. Go through the registration process
                        await NavigationService
                            .NavigateAsync(
                                nameof(RegistrationTypeSelectionPage),
                                new NavigationParameters
                                {
                                    { "user_id", userId }
                                }).ConfigureAwait(false);
                    }
                }
            },
            canExecute);

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await ShowTermsAndConditionsValidationMessage(dialogService).ConfigureAwait(false);
                    return;
                }

                await dialogService.AlertAsync("Coming Soon!").ConfigureAwait(false);
            },
            canExecute);

            SignInWithTwitter = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await ShowTermsAndConditionsValidationMessage(dialogService).ConfigureAwait(false);
                    return;
                }

                await dialogService.AlertAsync("Coming Soon!").ConfigureAwait(false);
            },
            canExecute);

            var commandsExecuting = Observable.CombineLatest(
                SignInWithGoogle.IsExecuting,
                SignInWithFacebook.IsExecuting,
                SignInWithTwitter.IsExecuting,
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
                    this.Log().ErrorException("Error signing up", exception);

                    //return SharedInteractions.Error.Handle(exception);
                    return Observable.Return(Unit.Default);
                })
                .Subscribe();
        }

        public bool AgreeWithTermsAndConditions
        {
            get => _agreeWithTermsAndConditions;
            set => this.RaiseAndSetIfChanged(ref _agreeWithTermsAndConditions, value);
        }

        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }

        public ReactiveCommand<Unit, Unit> SignInWithTwitter { get; }

        private static async Task ShowTermsAndConditionsValidationMessage(IUserDialogs dialogService)
        {
            await dialogService.AlertAsync(
                new AlertConfig
                {
                    Title = "Missing information",
                    Message = "You must agree to the terms and conditions",
                    OkText = "OK"
                }).ConfigureAwait(false);
        }
    }
}
