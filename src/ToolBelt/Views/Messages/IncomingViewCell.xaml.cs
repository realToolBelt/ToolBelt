using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using ToolBelt.Models;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Messages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncomingViewCell : ReactiveViewCell<ChatMessage>
    {
        public IncomingViewCell()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Text, v => v._lblText.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.CreateDate, v => v._lblMessageDate.Text, dateTime => $"{dateTime::MM/dd/yyyy hh:mm tt}")
                    .DisposeWith(disposable);
            });
        }
    }
}