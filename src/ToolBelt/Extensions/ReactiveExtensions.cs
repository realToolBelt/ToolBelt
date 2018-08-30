namespace System.Reactive.Linq
{
    public static class ReactiveExtensions
    {
        /// <summary>
        /// Projects the elements of the sequence to a <see cref="Unit" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="this">Observable sequence to subscribe to.</param>
        /// <returns>An observable sequence whose elements are <see cref="Unit" /> s.</returns>
        public static IObservable<Unit> ToSignal<T>(this IObservable<T> @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this.Select(_ => Unit.Default);
        }
    }
}
