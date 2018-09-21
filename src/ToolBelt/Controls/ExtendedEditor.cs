using System.Runtime.CompilerServices;
using ToolBelt.Effects;
using Xamarin.Forms;

namespace ToolBelt.Controls
{
    /// <summary>
    /// A derivation of the <see cref="Editor" /> that allows for additional behavior.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Editor" />
    public class ExtendedEditor : Editor, IDynamicLineColorControl
    {
        public static readonly BindableProperty FocusLineColorProperty = BindableProperty.Create(
            nameof(FocusLineColor),
            typeof(Color),
            typeof(ExtendedEditor),
            Color.Default);

        public static readonly BindableProperty InvalidLineColorProperty = BindableProperty.Create(
            nameof(InvalidLineColor),
            typeof(Color),
            typeof(ExtendedEditor),
            Color.Default);

        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(
            nameof(IsValid),
            typeof(bool),
            typeof(ExtendedEditor),
            true);

        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
            nameof(LineColor),
            typeof(Color),
            typeof(ExtendedEditor),
            Color.Default);

        private Color _lineColorToApply;

        public ExtendedEditor()
        {
            Focused += OnFocused;
            Unfocused += OnUnfocused;

            Effects.Add(new EditorLineColorEffect());

            ResetLineColor();
        }

        /// <summary>
        /// Gets or sets the color of the line when the entry is focused.
        /// </summary>
        public Color FocusLineColor
        {
            get => (Color)GetValue(FocusLineColorProperty);
            set => SetValue(FocusLineColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the color of the line when the entry is in the invalid state.
        /// </summary>
        public Color InvalidLineColor
        {
            get => (Color)GetValue(InvalidLineColorProperty);
            set => SetValue(InvalidLineColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the data in the entry is valid.
        /// </summary>
        /// <value><c>true</c> if the value in the entry is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        /// <summary>
        /// Gets or sets the default color of the line.
        /// </summary>
        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        /// <summary>
        /// Gets the line color to apply.
        /// </summary>
        public Color LineColorToApply
        {
            get => _lineColorToApply;
            private set
            {
                _lineColorToApply = value;
                OnPropertyChanged(nameof(LineColorToApply));
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsValidProperty.PropertyName)
            {
                CheckValidity();
            }
        }

        private void CheckValidity()
        {
            if (!IsValid)
            {
                LineColorToApply = InvalidLineColor;
            }
        }

        private Color GetNormalStateLineColor()
        {
            return LineColor != Color.Default
                    ? LineColor
                    : TextColor;
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            IsValid = true;
            LineColorToApply = FocusLineColor != Color.Default
                ? FocusLineColor
                : GetNormalStateLineColor();
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            ResetLineColor();
        }

        private void ResetLineColor()
        {
            LineColorToApply = GetNormalStateLineColor();
        }
    }
}
