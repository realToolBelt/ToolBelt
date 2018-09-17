using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectDetailsPage : ContentPageBase<ProjectDetailsPageViewModel>
    {
        public ProjectDetailsPage()
        {
            using (this.Log().Perf($"{nameof(ProjectDetailsPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ProjectDetailsPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Project.Name, v => v._lblProjectName.Text)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Project.EstimatedStartDate, v => v._lblStartDate.Text, date => $"{date:d}")
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Project.EstimatedEndDate, v => v._lblEndDate.Text, date => $"{date:d}")
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Project.Description, v => v._lblDescription.Text)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Project.SkillsRequired, v => v._lblSkillsRequired.Text)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}