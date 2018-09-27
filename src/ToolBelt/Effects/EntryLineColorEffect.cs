using Xamarin.Forms;

namespace ToolBelt.Effects
{
    /// <summary>
    /// An effect that can be used to change the line color of a <see cref="Entry"/>.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.RoutingEffect" />
    public class EntryLineColorEffect : RoutingEffect
    {
        public EntryLineColorEffect() : base(EffectSettings.EntryLineColor)
        {
        }
    }
}
