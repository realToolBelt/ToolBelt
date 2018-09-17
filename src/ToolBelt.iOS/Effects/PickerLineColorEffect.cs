using ToolBelt.Controls;
using ToolBelt.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(PickerLineColorEffect), nameof(PickerLineColorEffect))]

namespace ToolBelt.iOS.Effects
{
    public class PickerLineColorEffect : BaseLineColorEffect<ExtendedPicker>
    {
    }
}