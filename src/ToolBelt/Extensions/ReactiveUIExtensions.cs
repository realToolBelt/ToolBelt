using System.Collections.Generic;

namespace ReactiveUI
{
    /// <summary>
    /// Extensions for ReactiveUI.
    /// </summary>
    public static class ReactiveUIExtensions
    {
        /// <summary>
        /// Clears all items from the list, and adds the new collection of items to the list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="this">The list to reset.</param>
        /// <param name="items">
        /// The new items to be added to the list after the current items are cleared.
        /// </param>
        public static void Reset<T>(this IReactiveList<T> @this, IEnumerable<T> items)
        {
            using (@this.SuppressChangeNotifications())
            {
                @this.Clear();
                if (items != null)
                {
                    @this.AddRange(items);
                }
            }
        }
    }
}