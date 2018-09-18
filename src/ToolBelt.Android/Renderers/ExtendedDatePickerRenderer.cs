using Android.App;
using Android.Content;
using System.ComponentModel;
using ToolBelt.Controls;
using ToolBelt.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]

namespace ToolBelt.Droid.Renderers
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        public ExtendedDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var view = (ExtendedDatePicker)Element;
            var dialog = new DatePickerDialog(
                Context,
                Resource.Style.DatePickerSpinnerDialogStyle,
                (o, e) =>
                {
                    view.Date = e.Date;
                    ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    Control.ClearFocus();
                },
                year,
                month,
                day);

            dialog.SetButton("Done", (sender, e) =>
            {
                Element.Format = view.OriginalFormat;
                Control.Text = dialog.DatePicker.DateTime.ToString(Element.Format);
                view.AssignValue();
            });

            dialog.SetButton2("Clear", (sender, e) =>
            {
                view.CleanDate();
                Control.Text = Element.Format;
            });

            return dialog;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            var extendedDatePicker = (ExtendedDatePicker)Element;
            Control.Text = !extendedDatePicker.NullableDate.HasValue ? extendedDatePicker.PlaceHolder : Element.Date.ToString(Element.Format);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName)
            {
                var entry = (ExtendedDatePicker)Element;
                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                }
            }
        }
    }
}