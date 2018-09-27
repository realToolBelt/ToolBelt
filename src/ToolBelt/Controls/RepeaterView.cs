using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace ToolBelt.Controls
{
    /// <summary>
    /// A simple repeater that can be used to display a collection of items with a common item template.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.StackLayout" />
    public class RepeaterView : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(ICollection),
            typeof(RepeaterView),
            null,
            BindingMode.OneWay,
            propertyChanged: ItemsChanged);

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(RepeaterView),
            default(DataTemplate));

        public RepeaterView()
        {
            Spacing = 0;
        }

        /// <summary>
        /// Gets or sets the source of items to template and display.
        /// </summary>
        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate" /> to apply to the <see cref="ItemsSource" />.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Creates the view for the given <paramref name="item" />.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The view for the item</returns>
        protected virtual View ViewFor(object item)
        {
            View view = null;

            if (ItemTemplate != null)
            {
                var content = ItemTemplate.CreateContent();

                view = content is View contentView ? contentView : ((ViewCell)content).View;

                view.BindingContext = item;
            }

            return view;
        }

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is RepeaterView control))
            {
                return;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
            {
                oldObservableCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
            }

            if (newValue is INotifyCollectionChanged newObservableCollection)
            {
                newObservableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
            }

            control.Children.Clear();

            var items = (ICollection)newValue;
            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                control.Children.Add(control.ViewFor(item));
            }

            control.UpdateChildrenLayout();
            control.InvalidateLayout();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var invalidate = false;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // if the collection is reset, clear the children and recreate them
                Children.Clear();
                if (sender is ICollection collection)
                {
                    foreach (var item in collection)
                    {
                        Children.Add(ViewFor(item));
                    }
                }

                invalidate = true;
            }
            else
            {
                if (e.OldItems != null)
                {
                    Children.RemoveAt(e.OldStartingIndex);
                    invalidate = true;
                }

                if (e.NewItems != null)
                {
                    for (var i = 0; i < e.NewItems.Count; ++i)
                    {
                        var item = e.NewItems[i];
                        var view = ViewFor(item);

                        Children.Insert(i + e.NewStartingIndex, view);
                    }

                    invalidate = true;
                }
            }

            // update layout if necessary
            if (invalidate)
            {
                UpdateChildrenLayout();
                InvalidateLayout();
            }
        }
    }
}
