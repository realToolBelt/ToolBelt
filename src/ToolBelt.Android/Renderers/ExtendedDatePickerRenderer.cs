using Android.App;
using Android.Content;
using Android.Widget;
using System;
using System.ComponentModel;
using ToolBelt.Controls;
using ToolBelt.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]

namespace ToolBelt.Droid.Renderers
{
    // TODO: An upcoming version of Xamarin will allow us to derive from DatePickerRenderer and override the "CreateDialog" method.  Until then, we have this...
    // SEE: https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Platform.Android/Renderers/DatePickerRenderer.cs
    public class ExtendedDatePickerRenderer : ViewRenderer<ExtendedDatePicker, EditText>
    {
        private DatePickerDialog _dialog;

        public ExtendedDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override EditText CreateNativeControl()
        {
            return new EditText(Context) { Focusable = false, Clickable = true, Tag = this };
        }

        protected override void Dispose(bool disposing)
        {
            if (_dialog != null)
            {
                _dialog.Hide();
                _dialog.Dispose();
                _dialog = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedDatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var textField = CreateNativeControl();
                textField.SetOnClickListener(TextFieldClickHandler.Instance);
                SetNativeControl(textField);
            }

            if (Control == null || e.NewElement == null)
            {
                return;
            }

            UpdateMinimumDate();
            UpdateMaximumDate();

            var entry = Element;

            Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            Control.InputType |= Android.Text.InputTypes.TextFlagNoSuggestions;
            UpdateLineColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = Element;
                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                    return;
                }
            }
            else if (e.PropertyName == ExtendedDatePicker.MinimumDateProperty.PropertyName)
            {
                UpdateMinimumDate();
            }
            else if (e.PropertyName == ExtendedDatePicker.MaximumDateProperty.PropertyName)
            {
                UpdateMaximumDate();
            }

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColorToApply)))
            {
                UpdateLineColor();
            }
        }

        private void CreateDatePickerDialog(int year, int month, int day)
        {
            var view = Element;
            _dialog = new DatePickerDialog(
                Context,
                Resource.Style.DatePickerSpinnerDialogStyle,
                (o, e) =>
                {
                    view.Date = e.Date;
                    ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    Control.ClearFocus();

                    _dialog = null;
                },
                year,
                month,
                day);

            _dialog.SetButton("Done", (sender, e) =>
            {
                Element.Format = Element.OriginalFormat;
                SetDate(_dialog.DatePicker.DateTime);
                Element.AssignValue();
            });

            _dialog.SetButton2("Clear", (sender, e) =>
            {
                Element.CleanDate();
                Control.Text = Element.Format;
            });
        }

        private void OnTextFieldClicked()
        {
            ExtendedDatePicker view = Element;
            view.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            CreateDatePickerDialog(view.Date.Year, view.Date.Month - 1, view.Date.Day);

            UpdateMinimumDate();
            UpdateMaximumDate();

            _dialog.Show();
        }

        private void SetDate(DateTime date)
        {
            Control.Text = date.ToString(Element.Format);
            Element.Date = date;
        }

        private void UpdateLineColor()
        {
            Control?.Background?.SetColorFilter(Element.LineColorToApply.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
        }

        private void UpdateMaximumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MaxDate = (long)Element.MaximumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        private void UpdateMinimumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MinDate = (long)Element.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        private class TextFieldClickHandler : Java.Lang.Object, IOnClickListener
        {
            public static readonly TextFieldClickHandler Instance = new TextFieldClickHandler();

            public void OnClick(Android.Views.View v)
            {
                ((ExtendedDatePickerRenderer)v.Tag).OnTextFieldClicked();
            }
        }
    }
}