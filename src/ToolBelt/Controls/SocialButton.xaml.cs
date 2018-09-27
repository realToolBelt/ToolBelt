using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Controls
{
    /// <summary>
    /// This class represents a social login button that can be used to display a common look for
    /// different social logins available (twitter, google, facebook, etc...)
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Frame" />
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

        /// <summary>
        /// Gets or sets the text to use for the icon. This is intended to use a font (eg.
        /// FontAwesome) that can translate the given text into the appropriate social icon.
        /// </summary>
        public string IconText
        {
            get => (string)GetValue(IconTextProperty);
            set => SetValue(IconTextProperty, value);
        }

        /// <summary>
        /// Gets the tap gesture recognizer used to detect taps on the button.
        /// </summary>
        public TapGestureRecognizer TapGestureRecognizer { get; }

        /// <summary>
        /// Gets or sets the text to display on the button. This is typically the social providers
        /// name (eg. Twitter, Facebook, Google, ...).
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}