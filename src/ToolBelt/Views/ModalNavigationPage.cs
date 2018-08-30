using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms;

namespace ToolBelt.Views
{
    public class ModalNavigationPage : ReactiveNavigationPage<ModalNavigationPageViewModel>
    {
        public ModalNavigationPage()
        {
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Close",
                Icon = "ic_action_close.png",
                Priority = 100
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .BindCommand(ViewModel, vm => vm.Close, v => v.ToolbarItems[0])
                    .DisposeWith(disposable);
            });
        }
    }
}
