using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProjectsPage : ContentPageBase<MyProjectsPageViewModel>
    {
        public MyProjectsPage()
        {
            using (this.Log().Perf($"{nameof(MyProjectsPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(MyProjectsPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Projects, v => v._lstProjects.ItemsSource)
                        .DisposeWith(disposable);

                    _lstProjects
                        .ItemTappedToCommandBehavior(ViewModel, vm => vm.ViewProjectDetails)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}