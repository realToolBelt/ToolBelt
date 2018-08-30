using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Authentication
{
    public class SignupPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;
        private readonly ObservableAsPropertyHelper<bool> _isValid;
        private bool _agreeWithTermsAndConditions;
        private string _email;
        private string _name;
        private string _password;
        private string _passwordConfirm;

        public SignupPageViewModel(Prism.Navigation.INavigationService navigationService) : base(navigationService)
        {
            Title = "Sign Up";

            this
                .WhenAnyValue(n => n.Name, e => e.Email, p => p.Password, p => p.PasswordConfirm, a => a.AgreeWithTermsAndConditions,
                    (name, emailAddress, password, passwordConfirm, agreeWithTerms) =>
                        !string.IsNullOrEmpty(name)
                        && !string.IsNullOrEmpty(emailAddress)
                        && !string.IsNullOrEmpty(password)
                        && !string.IsNullOrEmpty(passwordConfirm)
                        && password.Equals(passwordConfirm, StringComparison.Ordinal)
                        && agreeWithTerms)
                .ToProperty(this, v => v.IsValid, out _isValid);

            var canExecuteLogin = this
                .WhenAnyValue(x => x.IsBusy, x => x.IsValid, (isBusy, isValid) => !isBusy && isValid);

            Register = ReactiveCommand.CreateFromTask(
              async _ =>
              {
                  var random = new Random();
                  await Task.Delay(random.Next(400, 2000));

                  this.Log().Debug("User registered!");

                  await NavigationService.NavigateAsync($"/Root/Details/{nameof(DashboardPage)}");
              },
              canExecuteLogin);

            // TODO: Switch to "Subscribe Safe"
            Register.ThrownExceptions.Subscribe(error => Debug.Write($"Error: {error}"));

            this.WhenAnyObservable(x => x.Register.IsExecuting)
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);
        }

        public bool AgreeWithTermsAndConditions
        {
            get => _agreeWithTermsAndConditions;
            set => this.RaiseAndSetIfChanged(ref _agreeWithTermsAndConditions, value);
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        public bool IsBusy => _isBusy?.Value ?? false;

        public bool IsValid => _isValid?.Value ?? false;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set => this.RaiseAndSetIfChanged(ref _passwordConfirm, value);
        }

        public ReactiveCommand Register { get; }
    }
}
