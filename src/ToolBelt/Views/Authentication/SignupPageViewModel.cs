using Acr.UserDialogs;
using Prism.Ioc;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel
    {
        private readonly IContainerRegistry _containerRegistry;
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

            SignInWithGoogle = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Missing information",
                            Message = "You must agree to the terms and conditions",
                            OkText = "OK"
                        }).ConfigureAwait(false);

                    return;
                }

                //await NavigationService
                //    .NavigateAsync(
                //        $"/NavigationPage/{nameof(TradeSpecialtiesPage)}",
                //        new NavigationParameters
                //        {
                //            { "user", user }
                //        }).ConfigureAwait(false);
                //if (await firebaseAuthService.SignUp(Username, Password))
                //{
                //}
            });

            SignInWithFacebook = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!AgreeWithTermsAndConditions)
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Missing information",
                            Message = "You must agree to the terms and conditions",
                            OkText = "OK"
                        }).ConfigureAwait(false);

                    return;
                }
            });

            SignInWithGoogle.ThrownExceptions.Subscribe(error => System.Diagnostics.Debug.WriteLine(error.ToString()));
        }

        public bool AgreeWithTermsAndConditions
        {
            get => _agreeWithTermsAndConditions;
            set => this.RaiseAndSetIfChanged(ref _agreeWithTermsAndConditions, value);
        }

        public ReactiveCommand<Unit, Unit> SignInWithFacebook { get; }

        public ReactiveCommand<Unit, Unit> SignInWithGoogle { get; }
    }
}
