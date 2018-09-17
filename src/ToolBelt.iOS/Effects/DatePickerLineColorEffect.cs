using ToolBelt.Controls;
using ToolBelt.Effects;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(DatePickerLineColorEffect), nameof(DatePickerLineColorEffect))]

namespace ToolBelt.iOS.Effects
{
    public class DatePickerLineColorEffect : BaseLineColorEffect<ExtendedDatePicker>
    {
    }
}