using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using ToolBelt.Models;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Messages
{
    // TODO: Add infinite scroll type feature

    public class ChatPageViewModel : BaseViewModel
    {
        private string _outgoingText;

        public ChatPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Chat";

            var random = new Random();

            Messages.AddRange(
                Enumerable.Range(0, 20).Select(i =>
                    new ChatMessage
                    (
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel leo convallis",
                        DateTime.Today.AddDays(random.Next(0, 25) * -1),
                        "Jane Doe",
                        isIncoming: i % 2 == 0
                    )));

            Send = ReactiveCommand.Create(() =>
            {
            // TODO: fill this out

            Messages.Add(new ChatMessage(OutgoingText, DateTime.Now, "John Doe", false));

                OutgoingText = string.Empty;
            },
            this.WhenAnyValue(x => x.OutgoingText, text => !string.IsNullOrEmpty(text)));
        }

        public ReactiveList<ChatMessage> Messages { get; } = new ReactiveList<ChatMessage>();

        public string OutgoingText
        {
            get => _outgoingText;
            set => this.RaiseAndSetIfChanged(ref _outgoingText, value);
        }

        public ReactiveCommand Send { get; }
    }
}
