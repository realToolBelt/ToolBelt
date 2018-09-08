using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms;

namespace ToolBelt.Views
{
    public class RootNavigationPage : ReactiveNavigationPage<ModalNavigationPageViewModel>, INavigationPageOptions
    {
        public RootNavigationPage()
        {
            ToolbarItems.Add(
                new ToolbarItem
                {
                    Text = "Search",
                    Icon = "ic_action_search.png"
                });
        }

        /// <summary>
        /// The INavigationService uses the result of this property to determine if the NavigationPage should clear the NavigationStack when navigating to a new Page.
        /// </summary>
        /// <remarks>This is equivalent to calling PopToRoot, and then replacing the current Page with the target Page being navigated to.</remarks>
        public bool ClearNavigationStackOnNavigation => false;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                // bind the "close" command to the command on the tool-bar item
                this
                    .BindCommand(ViewModel, vm => vm.Close, v => v.ToolbarItems[0])
                    .DisposeWith(disposable);
            });
        }
    }
}
