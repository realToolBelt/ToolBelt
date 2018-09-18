using ToolBelt.Controls;
using ToolBelt.Droid.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(EditorLineColorEffect), nameof(EditorLineColorEffect))]

namespace ToolBelt.Droid.Effects
{
    public class EditorLineColorEffect : BaseLineColorEffect<ExtendedEditor>
    {
    }
}