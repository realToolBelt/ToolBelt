using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Messages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ReactiveContentView<MessagesPageViewModel>
    {
        public MessagesPage(MessagesPageViewModel viewModel)
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
                    .Bind(ViewModel, vm => vm.IsBusy, v => v._activityIndicator.IsRunning)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Messages, v => v._lstMessages.ItemsSource)
                    .DisposeWith(disposable);

                _lstMessages
                    .ItemTappedToCommandBehavior(ViewModel, vm => vm.ViewMessage)
                    .DisposeWith(disposable);
            });
        }
    }
}