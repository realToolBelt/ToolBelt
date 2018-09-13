using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProjectViewCell : ReactiveViewCell<MyProjectViewModel>, IEnableLogger
    {
        public MyProjectViewCell()
        {
            using (this.Log().Perf($"{nameof(MyProjectViewCell)}: Initialize component."))
            {
                InitializeComponent();
            }

            // on iOS, waiting to bind until "WhenActivated" causes the list items to be sized
            // incorrectly. We can bind here, then add the composite disposable to the "activated" disposable
            CompositeDisposable disposables = new CompositeDisposable(
                this
                    .OneWayBind(ViewModel, vm => vm.Project.Name, v => v._lblProjectName.Text),
                this
                    .OneWayBind(ViewModel, vm => vm.Project.Description, v => v._lblProjectDescription.Text),
                this
                    .WhenAnyValue(v => v.ViewModel.Project.EstimatedStartDate, v => v.ViewModel.Project.EstimatedEndDate, (start, end) => $"{start:d} - {end:d}")
                    .BindTo(this, v => v._lblDateRange.Text));

            // handle when the view is activated
            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(MyProjectViewCell)}: Activate."))
                {
                    disposables.DisposeWith(disposable);

                    // handle settings that don't affect display here
                    _tgrEdit
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Edit)
                        .DisposeWith(disposable);

                    _tgrClose
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Close)
                        .DisposeWith(disposable);

                    _tgrDelete
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Delete)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}