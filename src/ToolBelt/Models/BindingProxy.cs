using Xamarin.Forms;

namespace ToolBelt.Models
{
    /// <summary>
    /// Sometimes binding resolution doesn't work when binding from a <see cref="DataTemplate" /> to
    /// another control, particularly when that <see cref="DataTemplate" /> is defined as a resource.
    /// This proxy object allows you to correctly bind to the proxied item even from a resource.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.BindableObject" />
    public class BindingProxy : BindableObject
    {
        public static readonly BindableProperty DataProperty = BindableProperty.Create(
            nameof(Data),
            typeof(object),
            typeof(BindingProxy),
            null,
            BindingMode.OneWay);

        /// <summary>
        /// Gets or sets the data that can be bound to.
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
