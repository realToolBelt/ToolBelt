using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using ToolBelt.ViewModels;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomImageCell : ReactiveViewCell<IImageCellViewModel>, IEnableLogger
    {
        public CustomImageCell()
        {
            using (this.Log().Perf($"{nameof(CustomImageCell)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(CustomImageCell)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Text, v => v._lblText.Text)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Detail, v => v._lblDetail.Text)
                        .DisposeWith(disposable);

                    // TODO: Bind to image source
                }
            });
        }
    }
}