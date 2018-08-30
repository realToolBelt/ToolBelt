using Xamarin.Forms;

namespace ToolBelt.Controls
{
    /// <summary>
    /// A simple control that uses the ToolBelt "logo" style.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Label" />
    public class ToolBeltLabel : Label
    {
        public ToolBeltLabel()
        {
            FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span { Text = "TOOL", TextColor = Color.White },
                    new Span { Text = "BELT", TextColor = Color.Yellow }
                }
            };
        }
    }
}
