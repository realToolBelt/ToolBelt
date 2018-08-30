using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using ToolBelt.Models;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageSummaryViewCell : ReactiveViewCell<ChatMessage>
    {
        public MessageSummaryViewCell()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.From, v => v._lblFrom.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Text, v => v._lblText.Text)
                    .DisposeWith(disposable);
            });
        }
    }
}