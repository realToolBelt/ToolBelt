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

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ProjectSummaryCellView)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Name, v => v._lblProjectName.Text)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.EstimatedStartDate, v => v._lblProjectStartDate.Text, date => $"{date:d}")
                        .DisposeWith(disposable);
                }
            });
        }
    }
}