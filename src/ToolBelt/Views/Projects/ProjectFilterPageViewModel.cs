using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

// TODO: https://github.com/muak/AiForms.Renderers

namespace ToolBelt.Views.Projects
{
    public class ProjectFilterPageViewModel : BaseViewModel
    {
        private DateTime _selectedEndDate = DateTime.Today.AddDays(14);
        private string _selectedEndDateComparisonType = "Before";
        private DateTime _selectedStartDate = DateTime.Today;
        private string _selectedStartDateComparisonType = "After";

        public ProjectFilterPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Filter";

            Apply = ReactiveCommand.CreateFromTask(async () =>
            {
            });

            Cancel = ReactiveCommand.CreateFromTask(async () =>
            {
            });

            // begin loading the trade data
            var trades = projectDataStore.GetTradeSpecialtiesAsync();

            SelectTrades = ReactiveCommand.CreateFromTask(async () =>
            {
                var args = new NavigationParameters();
                args.Add(
                    "items",
                    (await trades).Select(specialty => new SelectionViewModel<TradeSpecialty>(specialty)
                    {
                        DisplayValue = specialty.Name
                    }));

                await NavigationService.NavigateAsync(nameof(MultiSelectListViewPage), args).ConfigureAwait(false);
            });
        }

        public ReactiveCommand Apply { get; }

        public ReactiveCommand Cancel { get; }

        public ReactiveList<string> DateComparisonOptions { get; } = new ReactiveList<string>
        {
            "Before",
            "After"
        };

        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set => this.RaiseAndSetIfChanged(ref _selectedEndDate, value);
        }

        public string SelectedEndDateComparisonType
        {
            get => _selectedEndDateComparisonType;
            set => this.RaiseAndSetIfChanged(ref _selectedEndDateComparisonType, value);
        }

        public DateTime SelectedStartDate
        {
            get => _selectedStartDate;
            set => this.RaiseAndSetIfChanged(ref _selectedStartDate, value);
        }

        public string SelectedStartDateComparisonType
        {
            get => _selectedStartDateComparisonType;
            set => this.RaiseAndSetIfChanged(ref _selectedStartDateComparisonType, value);
        }

        public ReactiveCommand SelectTrades { get; }
    }
}