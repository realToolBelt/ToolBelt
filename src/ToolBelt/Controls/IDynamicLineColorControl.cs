using Xamarin.Forms;

namespace ToolBelt.Controls
{
    public interface IDynamicLineColorControl
    {
        /// <summary>
        /// Gets the line color to apply to the input control.
        /// </summary>
        Color LineColorToApply { get; }
    }
}
