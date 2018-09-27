using Xamarin.Forms;

namespace ToolBelt.Effects
{
    /// <summary>
    /// An effect that can be used to change the line color of a <see cref="Editor"/>.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.RoutingEffect" />
    public class EditorLineColorEffect : RoutingEffect
    {
        public EditorLineColorEffect() : base(EffectSettings.EditorLineColor)
        {
        }
    }
}
