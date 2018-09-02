using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using ToolBelt.Models;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectSummaryCellView : ReactiveViewCell<Project>, IEnableLogger
    {
        public ProjectSummaryCellView()
        {
            using (this.Log().Perf($"{nameof(ProjectSummaryCellView)}: Initialize component."))
            {
                InitializeComponent();
            }

            // on iOS, waiting to bind until "WhenActivated" causes the list items to
            // be sized incorrectly.  We can bind here, then add the composite disposable
            // to the "activated" disposable
            CompositeDisposable disposables = new CompositeDisposable(
                this.OneWayBind(ViewModel, vm => vm.Name, v => v._lblProjectName.Text),
                this.OneWayBind(ViewModel, vm => vm.EstimatedStartDate, v => v._lblProjectStartDate.Text, date => $"{date:d}"));

            // handle when the view is activated
            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ProjectSummaryCellView)}: Activate."))
                {
                    disposables.DisposeWith(disposable);
                }
            });
        }
    }
}