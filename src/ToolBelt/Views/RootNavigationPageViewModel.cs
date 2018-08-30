using Prism.Navigation;
using ReactiveUI;
using System.Reactive;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class RootNavigationPageViewModel : BaseViewModel
    {
        public RootNavigationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Search = ReactiveCommand.CreateFromTask(async () =>
            {
                return Unit.Default;
            });
        }

        public ReactiveCommand Search { get; }
    }
}
