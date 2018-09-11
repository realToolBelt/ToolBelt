using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using ToolBelt.Models;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
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

            // on iOS, waiting to bind until "WhenActivated" causes the list items to be sized
            // incorrectly. We can bind here, then add the composite disposable to the "activated" disposable
            CompositeDisposable disposables = new CompositeDisposable(
                this
                    .OneWayBind(ViewModel, vm => vm.Name, v => v._lblProjectName.Text),
                this
                    .WhenAnyValue(v => v.ViewModel.EstimatedStartDate, v => v.ViewModel.EstimatedEndDate, (start, end) => $"{start:d} - {end:d}")
                    .BindTo(this, v => v._lblProjectDates.Text));

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