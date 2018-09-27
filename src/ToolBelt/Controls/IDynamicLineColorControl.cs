using Xamarin.Forms;

namespace ToolBelt.Controls
{
    /// <summary>
    /// Interface defining common elements for the Extended* controls so that a common effect can be
    /// created that targets those controls.
    /// </summary>
    public interface IDynamicLineColorControl
    {
        /// <summary>
        /// Gets the line color to apply to the input control.
        /// </summary>
        Color LineColorToApply { get; }
    }
}
