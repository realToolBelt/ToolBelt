using ToolBelt.Controls;
using ToolBelt.Droid.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(PickerLineColorEffect), nameof(PickerLineColorEffect))]

namespace ToolBelt.Droid.Effects
{
    public class PickerLineColorEffect : BaseLineColorEffect<ExtendedPicker>
    {
    }
}