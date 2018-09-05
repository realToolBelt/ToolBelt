using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPageBase<ProjectsPageViewModel>
    {
        public ProjectsPage()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Projects, v => v._lstProjects.ItemsSource)
                    .DisposeWith(disposable);

                _lstProjects
                    .ItemTappedToCommandBehavior(ViewModel, vm => vm.ViewProjectDetails)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._lstProjects.IsRefreshing)
                    .DisposeWith(disposable);

                this
                    .WhenAnyValue(x => x.ViewModel)
                    .Where(vm => vm != null)
                    .ToSignal()
                    .Merge(
                        _lstProjects
                            .Events()
                            .Refreshing
                            .ToSignal()
                    )
                    .InvokeCommand(this, x => x.ViewModel.LoadProjects)
                    .DisposeWith(disposable);
            });
        }
    }
}