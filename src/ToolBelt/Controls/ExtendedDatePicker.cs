using System;
using Xamarin.Forms;

namespace ToolBelt.Controls
{
    /// <summary>
    /// An extended date picker that supports additional features like nullable dates, validation,
    /// and more.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.DatePicker" />
    public class ExtendedDatePicker : DatePicker
    {
        public static readonly BindableProperty FocusLineColorProperty = BindableProperty.Create(
            nameof(FocusLineColor),
            typeof(Color),
            typeof(ExtendedDatePicker),
            Color.Default);

        public static readonly BindableProperty InvalidLineColorProperty = BindableProperty.Create(
            nameof(InvalidLineColor),
            typeof(Color),
            typeof(ExtendedDatePicker),
            Color.Default);

        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(
            nameof(IsValid),
            typeof(bool),
            typeof(ExtendedDatePicker),
            true);

        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
            nameof(LineColor),
            typeof(Color),
            typeof(ExtendedDatePicker),
            Color.Default);

        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(
            nameof(NullableDate),
            typeof(DateTime?),
            typeof(ExtendedDatePicker),
            null,
            defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(
            nameof(PlaceHolder),
            typeof(string),
            typeof(ExtendedDatePicker),
            "/ . / . /");

        private Color _lineColorToApply;

        public ExtendedDatePicker()
        {
            Format = "d";

            Focused += OnFocused;
            Unfocused += OnUnfocused;

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

        public DateTime? NullableDate
        {
            get => (DateTime?)GetValue(NullableDateProperty);
            set
            {
                SetValue(NullableDateProperty, value);
                UpdateDate();
            }
        }

        public string OriginalFormat
        {
            get;
            private set;
        }

        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }

        public void AssignValue()
        {
            NullableDate = Date;
            UpdateDate();
        }

        public void CleanDate()
        {
            NullableDate = null;
            UpdateDate();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                if (OriginalFormat == null)
                {
                    OriginalFormat = Format;
                }

                UpdateDate();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused && (Date.ToString("d") == DateTime.Now.ToString("d"))))
            {
                AssignValue();
            }

            if (propertyName == NullableDateProperty.PropertyName && NullableDate.HasValue)
            {
                Date = NullableDate.Value;
                if (Date.ToString(OriginalFormat) == DateTime.Now.ToString(OriginalFormat))
                {
                    // this code was done because when date selected is the actual date the
                    // "DateProperty" does not raise
                    UpdateDate();
                }
            }

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

        private void UpdateDate()
        {
            if (NullableDate != null)
            {
                if (OriginalFormat != null)
                {
                    Format = OriginalFormat;
                }
            }
            else
            {
                Format = PlaceHolder;
            }
        }
    }
}
