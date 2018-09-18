using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System.Reactive.Linq;
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
                await navigationService.NavigateAsync(nameof(ContractorRegistrationPage)).ConfigureAwait(false);
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
        /// Gets the command used to register the new account as a tradesman.
        /// </summary>
        public ReactiveCommand Tradesman { get; }
    }
}