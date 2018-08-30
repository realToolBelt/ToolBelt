using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Xamarin.Forms
{
    /// <summary>
    /// Extensions for Xamarin Forms.
    /// </summary>
    public static class FormsExtensions
    {
        /// <summary>
        /// Adds a behavior to a <see cref="ListView" /> that will invoke a command when an item is
        /// tapped and clear any currently selected item.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="this">The <see cref="ListView" /> to attach the behavior to.</param>
        /// <param name="target">The root object which has the Command.</param>
        /// <param name="commandProperty">The expression to reference the Command.</param>
        /// <returns>An object that, when disposed, disconnects the behavior.</returns>
        /// <exception cref="ArgumentNullException">this</exception>
        public static IDisposable ItemTappedToCommandBehavior<TTarget>(
            this ListView @this,
            TTarget target,
            System.Linq.Expressions.Expression<Func<TTarget, ICommand>> commandProperty)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this
                .Events()
                .ItemSelected
                .Select(args => args.SelectedItem)
                .Where(item => item != null)
                .Do(_ => @this.SelectedItem = null)
                .InvokeCommand(target, commandProperty);
        }
    }
}
