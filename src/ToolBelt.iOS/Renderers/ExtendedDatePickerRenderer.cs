using System;
using System.Collections.Generic;
using System.ComponentModel;
using ToolBelt.Controls;
using ToolBelt.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]

namespace ToolBelt.iOS.Renderers
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                AddClearButton();

                Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                Control.Layer.BorderWidth = 1;

                var entry = (ExtendedDatePicker)Element;
                if (!entry.NullableDate.HasValue)
                {
                    Control.Text = entry.PlaceHolder;
                }

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    Control.Font = UIFont.SystemFontOfSize(25);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the property we are updating is the format property
            if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName)
            {
                var entry = (ExtendedDatePicker)Element;

                // If we are updating the format to the placeholder then just update the text and return
                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void AddClearButton()
        {
            if (Control.InputAccessoryView is UIToolbar originalToolbar && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem(
                    "Clear",
                    UIBarButtonItemStyle.Plain,
                    (sender, ev) =>
                    {
                        Element.Unfocus();
                        Element.Date = DateTime.Now;

                        if (Element is ExtendedDatePicker baseDatePicker)
                        {
                            baseDatePicker.CleanDate();
                        }
                    });

                var newItems = new List<UIBarButtonItem>();
                foreach (var item in originalToolbar.Items)
                {
                    newItems.Add(item);
                }

                newItems.Insert(0, clearButton);

                originalToolbar.Items = newItems.ToArray();
                originalToolbar.SetNeedsDisplay();
            }
        }
    }
}