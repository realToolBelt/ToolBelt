using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using ToolBelt.Models;
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
                //    this
                //        .OneWayBind(ViewModel, vm => vm.Project.Name, v => v._lblProjectName.Text)
                //        .DisposeWith(disposable);

                    //this
                    //    .OneWayBind(ViewModel, vm => vm.Project.StartStatus, v => v._lblStartDate.Text, startStatus =>
                    //    {
                    //        switch (startStatus)
                    //        {
                    //            case ProjectStartStatus.ReadyNow:
                    //                return "Ready Now";

                    //            case ProjectStartStatus.OneToTwoWeeks:
                    //                return "1 - 2 Weeks";

                    //            case ProjectStartStatus.ThreeToFourWeeks:
                    //                return "3 - 4 Weeks";

                    //            case ProjectStartStatus.FiveOrMoreWeeks:
                    //                return "5+ Weeks";

                    //            default:
                    //                return "Unknown";
                    //        }
                    //    })
                    //    .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.Project.Description, v => v._lblDescription.Text)
                        .DisposeWith(disposable);

                    //this
                    //    .OneWayBind(ViewModel, vm => vm.Project.SkillsRequired, v => v._lblSkillsRequired.Text)
                    //    .DisposeWith(disposable);

                    //this
                    //    .OneWayBind(ViewModel, vm => vm.Project.PaymentRate, v => v._lblPayRate.Text)
                    //    .DisposeWith(disposable);

                    //this
                    //    .OneWayBind(ViewModel, vm => vm.Project.PaymentType, v => v._lblPayType.Text)
                    //    .DisposeWith(disposable);
                }
            });
        }
    }
}