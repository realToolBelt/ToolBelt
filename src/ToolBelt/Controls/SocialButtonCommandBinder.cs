using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using ToolBelt.Extensions;
using Xamarin.Forms;

namespace ToolBelt.Controls
{
    /// <summary>
    /// Command binder that knows how to bind a <see cref="SocialButton" /> to an <see cref="ICommand" />.
    /// </summary>
    /// <seealso cref="ReactiveUI.ICreatesCommandBinding" />
    public sealed class SocialButtonCommandBinder : ICreatesCommandBinding
    {
        public IDisposable BindCommandToObject(ICommand command, object target, IObservable<object> commandParameter)
        {
            var socialButton = (SocialButton)target;
            var disposables = new CompositeDisposable();

            socialButton
                .TapGestureRecognizer
                .Events()
                .Tapped
                .Where(_ => socialButton.IsEnabled)
                .SubscribeSafe(_ => command.Execute(null))
                .DisposeWith(disposables);

            Observable
                .FromEventPattern(
                    x => command.CanExecuteChanged += x,
                    x => command.CanExecuteChanged -= x)
                .Select(_ => command.CanExecute(null))
                .StartWith(command.CanExecute(null))
                .SubscribeSafe(canExecute => socialButton.IsEnabled = canExecute)
                .DisposeWith(disposables);

            return disposables;
        }

        public IDisposable BindCommandToObject<TEventArgs>(ICommand command, object target, IObservable<object> commandParameter, string eventName)
        {
            throw new NotImplementedException();
        }

        public int GetAffinityForObject(Type type, bool hasEventTarget)
        {
            return type.GetTypeInfo().IsAssignableFrom(typeof(SocialButton).GetTypeInfo()) ? 100 : 0;
        }
    }
}
