using Xamarin.Forms;

namespace ToolBelt.Effects
{
    /// <summary>
    /// An effect that can be used to change the line color of a <see cref="DatePicker"/>.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.RoutingEffect" />
    public class DatePickerLineColorEffect : RoutingEffect
    {
        public DatePickerLineColorEffect() : base(EffectSettings.DatePickerLineColor)
        {
        }
    }
}
