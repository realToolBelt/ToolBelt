using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ToolBelt.Models;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Messages
{
    public class MessagesPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public MessagesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Messages";

            var random = new Random();

            Messages.AddRange(
                Enumerable.Range(0, 20).Select(i =>
                    new ChatMessage
                    (
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel leo convallis",
                        DateTime.Today.AddDays(random.Next(0, 25) * -1),
                        $"User {i}",
                        isIncoming: true
                    )));

            ViewMessage = ReactiveCommand.CreateFromTask<ChatMessage, Unit>(async message =>
            {
                await Task.Delay(random.Next(100, 400));
                await navigationService.NavigateAsync(nameof(ChatPage));
                return Unit.Default;
            });
        }

        public bool IsBusy => _isBusy?.Value ?? false;

        public ReactiveList<ChatMessage> Messages { get; } = new ReactiveList<ChatMessage>();

        public ReactiveCommand<ChatMessage, Unit> ViewMessage { get; }
    }
}
