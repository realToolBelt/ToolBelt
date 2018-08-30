using ReactiveUI.XamForms;
using ToolBelt.Models;
using Xamarin.Forms.Xaml;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMenuItemCellView : ReactiveViewCell<CustomMenuItem>
    {
        public CustomMenuItemCellView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Title, v => v._lblTitle.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.IconSource, v => v._lblIcon.Text)
                    .DisposeWith(disposable);
            });
        }
    }
}