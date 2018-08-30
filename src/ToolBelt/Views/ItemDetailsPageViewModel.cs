using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ItemDetailsPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        private MarketplaceItemFull _item;

        public ItemDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Item Details";

            var loadCommand = ReactiveCommand.CreateFromTask<int, Unit>(async id =>
            {
                var random = new Random();
                await Task.Delay(random.Next(400, 2000));
                Item = new MarketplaceItemFull
                {
                    Id = id,
                    Title = $"Item {id}",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel leo convallis dui tempus mattis. Nulla vel posuere massa. Quisque vitae orci eu magna vulputate suscipit. Aenean ac sapien quis nunc congue venenatis suscipit ut sapien. Nullam eu condimentum justo. Praesent varius, quam et auctor dictum, turpis magna rutrum dui, sed congue purus mi eu felis. In hac habitasse platea dictumst. Sed scelerisque condimentum rhoncus.

Ut pretium nisi mattis nunc lobortis, vitae rhoncus odio sollicitudin.Maecenas ac ipsum dui.Etiam velit nunc, blandit eu venenatis non, consequat eu erat.Fusce eget nunc lacinia, aliquet risus eget, rutrum mauris.Proin elementum accumsan suscipit.Nulla a eros non purus viverra semper.Mauris auctor nec libero quis dictum.Nam ultricies enim sed odio cursus lobortis.Vestibulum a quam risus.Nam nec mattis arcu, at mollis elit.Ut lacinia, metus vel sodales cursus, magna elit hendrerit tellus, eget egestas tellus nisi ac justo.

Proin aliquet porta scelerisque.Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Praesent et enim erat. Maecenas aliquam tristique sem nec viverra. Maecenas cursus aliquam ante, a rutrum ante convallis nec. Proin nec ornare nisi, sit amet ullamcorper erat.Vivamus leo arcu, viverra eget dui non, commodo iaculis mauris. In sapien diam, malesuada ac pulvinar eu, accumsan efficitur augue. Aliquam id erat ex. Nam consequat ex sed maximus lacinia. Duis nec faucibus urna.

Nulla non nisi tincidunt, varius dui eget, sagittis nunc.Nullam suscipit eros ac tellus scelerisque suscipit.Donec fermentum iaculis massa in porttitor.Suspendisse vitae tincidunt nunc, non ornare ipsum. Pellentesque eget nulla rhoncus, placerat arcu sit amet, rhoncus massa. Maecenas et ex dui. Praesent euismod a velit eget interdum. Cras iaculis tellus accumsan velit gravida placerat.Aliquam sed mauris semper, elementum ex ac, pharetra ante.

Vestibulum dignissim ex ante, mollis porta massa lobortis dapibus. Suspendisse nunc neque, finibus vitae eleifend vel, maximus pharetra nulla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed tempor non odio et tempor. Donec vitae ultricies tellus. Maecenas quis velit orci. Vestibulum elementum interdum lacinia. Phasellus ut aliquet ipsum, at mattis dolor. Donec pellentesque tellus nunc, eu sollicitudin leo pretium at. Curabitur sit amet urna sit amet dui dictum mattis vitae sed quam.",
                    Price = (decimal)random.NextDouble(),
                    IconSource = "xamarin_logo.png"
                };

                return Unit.Default;
            });

            loadCommand.IsExecuting
              .ToProperty(this, x => x.IsBusy, out _isBusy);

            NavigatedTo
                .Select(parameter => (int)parameter["id"])
                .InvokeCommand(loadCommand);
        }

        public bool IsBusy => _isBusy?.Value ?? false;

        public MarketplaceItemFull Item
        {
            get => _item;
            private set => this.RaiseAndSetIfChanged(ref _item, value);
        }
    }

    public class MarketplaceItemFull : MarketplaceItemBase
    {
        private int _numberOfReviews;

        public int NumberOfReviews
        {
            get => _numberOfReviews;
            set => this.RaiseAndSetIfChanged(ref _numberOfReviews, value);
        }
    }
}
