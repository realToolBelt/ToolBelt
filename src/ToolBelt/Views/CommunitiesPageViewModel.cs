using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class CommunitiesPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private Community _selectedCommunity;

        public CommunitiesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Communities";

            Communities = new ReactiveList<Community>
            {
                new Community("Roofing Projects"),
                new Community("Roofing Tradesmen"),
                new Community("Roofing Materials"),
                new Community("Roofing Contractors"),

                new Community("Siding Projects"),
                new Community("Siding Tradesmen"),
                new Community("Siding Materials"),
                new Community("Siding Contractors"),

                new Community("Painting Projects"),
                new Community("Painting Tradesmen"),
                new Community("Painting Materials"),
                new Community("Painting Contractors"),
            };

            CompositeDisposable commandDisposable = null;
            LoadCommunityItems = ReactiveCommand.CreateFromTask(
                async _ =>
                {
                    commandDisposable?.Dispose();
                    commandDisposable = new CompositeDisposable();

                    Items.Clear();

                    var random = new Random();
                    await Task.Delay(random.Next(400, 2000));

                    Items.Reset(
                        Enumerable.Range(0, 20).Select(i =>
                            new MarketplaceItemSummary
                            {
                                Id = i,
                                Title = $"Item {i}",
                                Description = $"Some item numbered {i}",
                                Price = (decimal)random.NextDouble(),
                                IconSource = "xamarin_logo.png",
                                TapCommand = ReactiveCommand.CreateFromTask(
                                    async () =>
                                        await NavigationService.NavigateAsync(
                                            nameof(ItemDetailsPage),
                                            new NavigationParameters
                                            {
                                                { "id", i }
                                            })
                                        .ConfigureAwait(false))
                            }));

                    foreach (var item in Items)
                    {
                        commandDisposable.Add(item.TapCommand.ThrownExceptions.Subscribe(error => System.Diagnostics.Debug.WriteLine($"Error: {error}")));
                    }
                });

            LoadCommunityItems.ThrownExceptions.Subscribe(error => System.Diagnostics.Debug.WriteLine($"Error: {error}"));

            this.WhenAnyObservable(x => x.LoadCommunityItems.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);

            this
                .WhenAnyValue(x => x.SelectedCommunity)
                .ToSignal()
                .InvokeCommand(LoadCommunityItems);

            // by default, select the first community
            SelectedCommunity = Communities[0];
        }

        public ReactiveList<Community> Communities { get; }

        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveList<MarketplaceItemSummary> Items { get; } = new ReactiveList<MarketplaceItemSummary>();

        public ReactiveCommand LoadCommunityItems { get; }

        public Community SelectedCommunity
        {
            get => _selectedCommunity;
            set => this.RaiseAndSetIfChanged(ref _selectedCommunity, value);
        }
    }

    public class Community
    {
        public Community(string name)
        {
            Name = name;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; }
    }

    // TODO: This should be immutable
    public class MarketplaceItemBase : ReactiveObject
    {
        private string _description;
        private string _iconSource;
        private int _id;
        private decimal _price;
        private string _title;

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public string IconSource
        {
            get => _iconSource;
            set => this.RaiseAndSetIfChanged(ref _iconSource, value);
        }

        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public decimal Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }
    }

    public class MarketplaceItemSummary : MarketplaceItemBase
    {
        private ReactiveCommand<Unit, Unit> _tapCommand;

        public ReactiveCommand<Unit, Unit> TapCommand
        {
            get => _tapCommand;
            set => this.RaiseAndSetIfChanged(ref _tapCommand, value);
        }
    }
}
