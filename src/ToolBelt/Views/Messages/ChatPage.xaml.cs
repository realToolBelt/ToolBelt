using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;
using System;
using Xamarin.Forms;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace ToolBelt.Views.Messages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPageBase<ChatPageViewModel>
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.Messages, v => v._lstMessages.ItemsSource)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.OutgoingText, v => v._txtOutgoingText.Text)
                    .DisposeWith(disposable);

                this
                    .BindCommand(ViewModel, vm => vm.Send, v => v._btnSend)
                    .DisposeWith(disposable);

                ViewModel.Messages.ItemsAdded
                    .StartWith(ViewModel.Messages.LastOrDefault())
                    .Where(message => message != null)
                    .Subscribe(message => _lstMessages.ScrollTo(message, ScrollToPosition.End, true))
                    .DisposeWith(disposable);

                Observable.Merge(
                    _lstMessages.Events().ItemSelected.Select(_ => Unit.Default),
                    _lstMessages.Events().ItemTapped.Select(_ => Unit.Default))
                    .Subscribe(_ => _lstMessages.SelectedItem = null)
                    .DisposeWith(disposable);

                if (Device.RuntimePlatform == Device.Android)
                {
                    var platformConfig = Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>();
                    var softInputMode = platformConfig.GetWindowSoftInputModeAdjust();
                    platformConfig.UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
                    Disposable.Create(() =>
                    {
                        // restore the original soft input mode
                        platformConfig.UseWindowSoftInputModeAdjust(softInputMode);
                    })
                    .DisposeWith(disposable);

                }
            });
        }
    }
}