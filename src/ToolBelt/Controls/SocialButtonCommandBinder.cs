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
        /// <summary>
        /// Bind an ICommand to a UI object, in the "default" way. The meaning of this is dependent
        /// on the implementation. Implement this if you have a new type of UI control that doesn't
        /// have Command/CommandParameter like WPF or has a non-standard event name for "Invoke".
        /// </summary>
        /// <param name="command">The command to bind</param>
        /// <param name="target">The target object, usually a UI control of some kind</param>
        /// <param name="commandParameter">
        /// An IObservable source whose latest value will be passed as the command parameter to the
        /// command. Hosts will always pass a valid IObservable, but this may be Observable.Empty
        /// </param>
        /// <returns>An IDisposable which will disconnect the binding when disposed.</returns>
        public IDisposable BindCommandToObject(ICommand command, object target, IObservable<object> commandParameter)
        {
            var socialButton = (SocialButton)target;
            var disposables = new CompositeDisposable();

            // when the button is tapped and enabled, execute the associated command
            socialButton
                .TapGestureRecognizer
                .Events()
                .Tapped
                .Where(_ => socialButton.IsEnabled)
                .SubscribeSafe(_ => command.Execute(null))
                .DisposeWith(disposables);

            // when the "CanExecute" state of the command is changed, update the enabled state of the button
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

        /// <summary>
        /// Bind an ICommand to a UI object to a specific event. This event may be a standard .NET
        /// event, or it could be an event derived in another manner (i.e. in MonoTouch).
        /// </summary>
        /// <param name="command">The command to bind</param>
        /// <param name="target">The target object, usually a UI control of some kind</param>
        /// <param name="commandParameter">
        /// An IObservable source whose latest value will be passed as the command parameter to the
        /// command. Hosts will always pass a valid IObservable, but this may be Observable.Empty
        /// </param>
        /// <param name="eventName">The event to bind to.</param>
        /// <returns></returns>
        /// <returns>An IDisposable which will disconnect the binding when disposed.</returns>
        public IDisposable BindCommandToObject<TEventArgs>(ICommand command, object target, IObservable<object> commandParameter, string eventName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a positive integer when this class supports BindCommandToObject for this
        /// particular Type. If the method isn't supported at all, return a non-positive integer.
        /// When multiple implementations return a positive value, the host will use the one which
        /// returns the highest value. When in doubt, return '2' or '0'
        /// </summary>
        /// <param name="type">The type to query for.</param>
        /// <param name="hasEventTarget">If true, the host intends to use a custom event target.</param>
        /// <returns>A positive integer if BCTO is supported, zero or a negative value otherwise</returns>
        public int GetAffinityForObject(Type type, bool hasEventTarget)
        {
            return type.GetTypeInfo().IsAssignableFrom(typeof(SocialButton).GetTypeInfo()) ? 100 : 0;
        }
    }
}
