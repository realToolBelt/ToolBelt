using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Services;
using ToolBelt.ViewModels;

// TODO: https://github.com/muak/AiForms.Renderers

namespace ToolBelt.Views.Projects
{
    public enum DateComparisonType
    {
        Before,
        After
    }

    public class ProjectFilter
    {
        public DateTime? StartDate { get; set; }

        public DateComparisonType StartDateComparison { get; set; }

        public List<Trade> Trades { get; } = new List<Trade>();
    }

    public class ProjectFilterPageViewModel : BaseViewModel
    {
        private ProjectFilter _filter;
        private DateTime? _selectedEndDate;
        private DateComparisonType _selectedEndDateComparisonType = DateComparisonType.Before;
        private DateTime? _selectedStartDate;
        private DateComparisonType _selectedStartDateComparisonType = DateComparisonType.After;
        private IEnumerable<Trade> _selectedTrades;

        public ProjectFilterPageViewModel(
            INavigationService navigationService,
            IProjectDataStore projectDataStore) : base(navigationService)
        {
            Title = "Filter";

            Apply = ReactiveCommand.CreateFromTask(async () =>
            {
                _filter.StartDate = _selectedStartDate;
                _filter.StartDateComparison = _selectedStartDateComparisonType;

                _filter.Trades.Clear();
                _filter.Trades.AddRange(_selectedTrades);

                await NavigationService.GoBackAsync(new NavigationParameters
                {
                    { "filter", _filter }
                },
                useModalNavigation: true).ConfigureAwait(false);
            });

            Cancel = ReactiveCommand.CreateFromTask(() => NavigationService.GoBackAsync(useModalNavigation: true));

            NavigatedTo
                .Where(args => args.ContainsKey("filter"))
                .Take(1)
                .Select(args => (ProjectFilter)args["filter"])
                .Subscribe(filter => _filter = filter);

            NavigatedTo
                .Where(args => args.ContainsKey("selected_items"))
                .Select(args => (IEnumerable<SelectionViewModel>)args["selected_items"])
                .Subscribe(selections => _selectedTrades = selections.Select(x => x.Item).Cast<Trade>());

            // begin loading the trade data
            var trades = projectDataStore.GetTradesAsync();

            SelectTrades = ReactiveCommand.CreateFromTask(async () =>
            {
                var args = new NavigationParameters();
                args.Add(
                    "items",
                    (await trades).Select(specialty => new SelectionViewModel<Trade>(specialty)
                    {
                        DisplayValue = specialty.Name,
                        IsSelected = _selectedTrades?.Contains(specialty) == true
                    }));

                await NavigationService.NavigateAsync(nameof(MultiSelectListViewPage), args).ConfigureAwait(false);
            });
        }

        public ReactiveCommand Apply { get; }

        public ReactiveCommand Cancel { get; }

        public ReactiveList<string> DateComparisonOptions { get; } = new ReactiveList<string>(Enum.GetNames(typeof(DateComparisonType)));

        public DateTime? SelectedEndDate
        {
            get => _selectedEndDate;
            set => this.RaiseAndSetIfChanged(ref _selectedEndDate, value);
        }

        public DateComparisonType SelectedEndDateComparisonType
        {
            get => _selectedEndDateComparisonType;
            set => this.RaiseAndSetIfChanged(ref _selectedEndDateComparisonType, value);
        }

        public DateTime? SelectedStartDate
        {
            get => _selectedStartDate;
            set => this.RaiseAndSetIfChanged(ref _selectedStartDate, value);
        }

        public DateComparisonType SelectedStartDateComparisonType
        {
            get => _selectedStartDateComparisonType;
            set => this.RaiseAndSetIfChanged(ref _selectedStartDateComparisonType, value);
        }

        public ReactiveCommand SelectTrades { get; }
    }
}