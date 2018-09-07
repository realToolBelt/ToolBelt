using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ContactUsPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private readonly ObservableAsPropertyHelper<bool> _isValid;
        private string _email;
        private string _message;
        private string _name;

        public ContactUsPageViewModel(
            INavigationService navigationService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "Contact Us";
            analyticService.TrackScreen("contact-us-page");

            this
                .WhenAnyValue(x => x.Name, x => x.Email, x => x.Message, (name, email, message) =>
                    !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(message))
                .ToProperty(this, x => x.IsValid, out _isValid);

            var canExecuteSubmit = this
                .WhenAnyValue(x => x.IsBusy, x => x.IsValid, (isBusy, isValid) => !isBusy && isValid);

            Submit = ReactiveCommand.CreateFromTask(
                async _ =>
                {
                    analyticService.TrackTapEvent("submit");

                    var random = new Random();
                    await Task.Delay(random.Next(400, 2000));
                    await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
                },
                canExecuteSubmit);

            this.WhenAnyObservable(x => x.Submit.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public bool IsBusy => _isBusy?.Value ?? false;

        public bool IsValid => _isValid?.Value ?? false;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public ReactiveCommand Submit { get; }
    }
}
