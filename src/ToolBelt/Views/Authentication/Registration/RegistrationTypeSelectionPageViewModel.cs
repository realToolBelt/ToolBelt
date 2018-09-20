using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication.Registration
{
    public class RegistrationTypeSelectionPageViewModel : BaseViewModel
    {
        private string _userId;

        public RegistrationTypeSelectionPageViewModel(
            INavigationService navigationService,
            IAnalyticService analyticService,
            IFirebaseAuthService authService,
            IUserDialogs dialogs) : base(navigationService)
        {
            Title = "Registration Type";
            analyticService.TrackScreen("registration-type");

            Tradesman = ReactiveCommand.CreateFromTask(async () =>
            {
                await dialogs.AlertAsync("Coming Soon!").ConfigureAwait(false);
                analyticService.TrackTapEvent("register-as-tradesman");

                //await navigationService.NavigateAsync(nameof(TradesmentRegistrationPage)).ConfigureAwait(false);
            });

            Contractor = ReactiveCommand.CreateFromTask(async () =>
            {
                analyticService.TrackTapEvent("register-as-contractor");
                await navigationService.NavigateAsync(
                    nameof(ContractorRegistrationPage),
                    new NavigationParameters { { "user_id", _userId } }).ConfigureAwait(false);
            });

            GoBack = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                var result = await dialogs.ConfirmAsync(
                    new ConfirmConfig
                    {
                        Title = "Cancel Account Creation?",
                        Message = "Are you sure you want to cancel account creation?  This will discard any information you have entered so far",
                        OkText = "Yes",
                        CancelText = "No"
                    });
                if (result)
                {
                    analyticService.TrackTapEvent("cancel-account-creation");

                    // make sure we log out so the user has to log in again
                    await authService.Logout();

                    await NavigationService.NavigateToLoginPageAsync().ConfigureAwait(false);
                }

                return Unit.Default;
            });

            NavigatingTo
                .Where(args => args.ContainsKey("user_id"))
                .Select(args => args["user_id"].ToString())
                .BindTo(this, x => x._userId);
        }

        /// <summary>
        /// Gets the command used to register the new account as a contractor.
        /// </summary>
        public ReactiveCommand Contractor { get; }

        /// <summary>
        /// Gets the command used to navigate back.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GoBack { get; }

        /// <summary>
        /// Gets the command used to register the new account as a tradesman.
        /// </summary>
        public ReactiveCommand Tradesman { get; }
    }
}