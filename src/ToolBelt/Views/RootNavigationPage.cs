using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms;

namespace ToolBelt.Views
{
    public class RootNavigationPage : ReactiveNavigationPage<ModalNavigationPageViewModel>
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
