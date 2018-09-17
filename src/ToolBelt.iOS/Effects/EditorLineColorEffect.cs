using ToolBelt.Controls;
using ToolBelt.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(EditorLineColorEffect), nameof(EditorLineColorEffect))]

namespace ToolBelt.iOS.Effects
{
    public class EditorLineColorEffect : BaseLineColorEffect<ExtendedEditor>
    {
    }
}