using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocialButton : Frame
    {
        public static readonly BindableProperty IconTextProperty = BindableProperty.Create(
            nameof(IconText),
            typeof(string),
            typeof(SocialButton),
            string.Empty);

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(SocialButton),
            string.Empty);

        public SocialButton()
        {
            InitializeComponent();

            TapGestureRecognizer = new TapGestureRecognizer();
            GestureRecognizers.Add(TapGestureRecognizer);
        }

        public string IconText
        {
            get => (string)GetValue(IconTextProperty);
            set => SetValue(IconTextProperty, value);
        }

        public TapGestureRecognizer TapGestureRecognizer { get; }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}