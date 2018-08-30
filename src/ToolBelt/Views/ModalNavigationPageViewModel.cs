using Prism.Navigation;
using ReactiveUI;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ModalNavigationPageViewModel : BaseViewModel
    {
        public ModalNavigationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Close = ReactiveCommand.CreateFromTask(() => NavigationService.GoBackAsync(useModalNavigation: true));
        }

        /// <summary>
        /// Gets the command used to close the modal.
        /// </summary>
        public ReactiveCommand Close { get; }
    }
}
