using Xamarin.Forms;

namespace ToolBelt.Effects
{
    /// <summary>
    /// An effect that can be used to change the line color of a <see cref="Picker"/>.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.RoutingEffect" />
    public class PickerLineColorEffect : RoutingEffect
    {
        public PickerLineColorEffect() : base(EffectSettings.PickerLineColor)
        {
        }
    }
}
