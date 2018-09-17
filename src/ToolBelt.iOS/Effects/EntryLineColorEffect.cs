using ToolBelt.Controls;
using ToolBelt.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(EntryLineColorEffect), nameof(EntryLineColorEffect))]

namespace ToolBelt.iOS.Effects
{
    public class EntryLineColorEffect : BaseLineColorEffect<ExtendedEntry>
    {
    }
}