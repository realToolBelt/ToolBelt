using Acr.UserDialogs;
using Prism.Ioc;
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
        private Account _account;

        public TradeSpecialtiesPageViewModel(
            INavigationService navigationService,
            IUserDialogs dialogService,
            IContainerRegistry containerRegistry,
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
                var selectedItems = Items.Where(item => item.IsSelected).Select(item => item.Item);
                if (!selectedItems.Any())
                {
                    await dialogService.AlertAsync(
                        new AlertConfig
                        {
                            Title = "Error",
                            Message = "You must select at least one trade specialty",
                            OkText = "OK"
                        }).ConfigureAwait(false);

                    return;
                }

                if (_account is ContractorAccount contractor)
                {
                    contractor.Specialties.Clear();
                    contractor.Specialties.AddRange(selectedItems);
                }
                
                // TODO: Save the account...
                containerRegistry.RegisterInstance<IUserService>(new UserService(_account));
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
                .Select(args => (Account)args["account"])
                .Subscribe(user =>
                {
                    _account = user;
                });
        }

        public ReactiveList<SelectionViewModel<TradeSpecialty>> Items { get; } = new ReactiveList<SelectionViewModel<TradeSpecialty>>();

        public ReactiveCommand Next { get; }

        public ReactiveCommand SelectAll { get; }

        public ReactiveCommand SelectNone { get; }
    }
}