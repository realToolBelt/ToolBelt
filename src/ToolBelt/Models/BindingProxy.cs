using Xamarin.Forms;

namespace ToolBelt.Models
{
    public class BindingProxy : BindableObject
    {
        public static readonly BindableProperty DataProperty = BindableProperty.Create(
            nameof(Data),
            typeof(object),
            typeof(BindingProxy),
            null,
            BindingMode.OneWay);

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
