using ToolBelt.Controls;
using ToolBelt.Droid.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(DatePickerLineColorEffect), nameof(DatePickerLineColorEffect))]

namespace ToolBelt.Droid.Effects
{
    public class DatePickerLineColorEffect : BaseLineColorEffect<ExtendedDatePicker>
    {
    }
}