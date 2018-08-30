using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ReactiveContentView<DashboardPageViewModel>
    {
        public DashboardPage(DashboardPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Projects, v => v._lstProjects.ItemsSource)
                    .DisposeWith(disposable);
                this
                    .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsVisible)
                    .DisposeWith(disposable);

                _lstProjects
                    .ItemTappedToCommandBehavior(ViewModel, vm => vm.ViewProjectDetails)
                    .DisposeWith(disposable);
            });
        }
    }
}