using ToolBelt.Controls;
using ToolBelt.Droid.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(EntryLineColorEffect), nameof(EntryLineColorEffect))]

namespace ToolBelt.Droid.Effects
{
    public class EntryLineColorEffect : BaseLineColorEffect<ExtendedEditor>
    {
    }
}