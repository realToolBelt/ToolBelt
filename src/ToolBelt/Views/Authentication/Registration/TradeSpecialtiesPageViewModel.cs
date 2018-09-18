using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication.Registration
{
    // TODO: Most of this page should be moved into a content view rather than a page.  Then we could share it across the registration and profile editing pages.
    public class TradeSpecialtiesPageViewModel : BaseViewModel
    {
        private Account _user;

        public TradeSpecialtiesPageViewModel(
            INavigationService navigationService,
            IUserDialogs dialogService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Trade Specialties";

            SelectAll = ReactiveCommand.Create(() =>
            {
                foreach (var item in Items)
                {
                    item.IsSelected = true;
                }
            });

            SelectNone = ReactiveCommand.Create(() =>
            {
                foreach (var item in Items)
                {
                    item.IsSelected = false;
                }
            });

            Next = ReactiveCommand.CreateFromTask(async () =>
            {
                // if there are no items selected, they can't move on. Must select at least one specialty
                if (!Items.Any(item => item.IsSelected))
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Error",
                            Message = "You must select at least one trade specialty",
                            OkText = "OK"
                        }).ConfigureAwait(false);
                }

                // TODO: ...
                await NavigationService.NavigateHomeAsync().ConfigureAwait(false);
            });

            projectDataStore
                .GetTradeSpecialtiesAsync()
                .ContinueWith(t =>
                {
                    // TODO: Handle failure?
                    Items.AddRange(
                        t.Result.Select(
                            specialty => new SelectionViewModel<TradeSpecialty>(specialty)
                            {
                                DisplayValue = specialty.Name
                            }));
                });

            NavigatingTo
                .Select(args => (Account)args["user"])
                .Subscribe(user =>
                {
                    _user = user;
                });
        }

        public ReactiveList<SelectionViewModel<TradeSpecialty>> Items { get; } = new ReactiveList<SelectionViewModel<TradeSpecialty>>();

        public ReactiveCommand Next { get; }

        public ReactiveCommand SelectAll { get; }

        public ReactiveCommand SelectNone { get; }
    }
}