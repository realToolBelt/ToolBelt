using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.Services;
using ToolBelt.Validation;
using ToolBelt.Validation.Rules;
using ToolBelt.ViewModels;

namespace ToolBelt.Views
{
    public class ContactUsPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public ContactUsPageViewModel(
            INavigationService navigationService,
            IAnalyticService analyticService,
            IUserService userService) : base(navigationService)
        {
            Title = "Contact Us";
            analyticService.TrackScreen("contact-us-page");
            AddValidationRules();

            Submit = ReactiveCommand.CreateFromTask(
                async _ =>
                {
                    if (!IsValid())
                    {
                        return;
                    }

                    analyticService.TrackTapEvent("submit");

                    var random = new Random();
                    await Task.Delay(random.Next(400, 2000));
                    await NavigationService.GoBackAsync(useModalNavigation: true).ConfigureAwait(false);
                });

            this.WhenAnyObservable(x => x.Submit.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);

            NavigatingTo
                .Take(1)
                .Subscribe(_ =>
                {
                    Email.Value = userService.AuthenticatedUser.EmailAddress;
                });
        }

        public ValidatableObject<string> Email { get; } = new ValidatableObject<string>();

        public bool IsBusy => _isBusy?.Value ?? false;

        public ValidatableObject<string> Message { get; } = new ValidatableObject<string>();

        public ValidatableObject<string> Name { get; } = new ValidatableObject<string>();

        public ReactiveCommand Submit { get; }

        private void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Email cannot be empty" });
            Email.Validations.Add(new EmailRule { ValidationMessage = "Email should be an email address" });

            Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Name cannot be empty" });
            Message.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Message cannot be empty" });
        }

        private bool IsValid()
        {
            // NOTE: Validate each control individually so we get error indicators for all
            Email.Validate();
            Name.Validate();
            Message.Validate();

            return Email.IsValid
                && Name.IsValid
                && Message.IsValid;
        }
    }
}
