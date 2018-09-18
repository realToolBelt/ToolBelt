using Android.Widget;
using System;
using System.ComponentModel;
using ToolBelt.Controls;
using Xamarin.Forms.Platform.Android;

namespace ToolBelt.Droid.Effects
{
    public class BaseLineColorEffect<T> : PlatformEffect
        where T : class, IDynamicLineColorControl
    {
        private EditText _control;
        private T _entry;

        protected override void OnAttached()
        {
            try
            {
                _control = Control as EditText;
                _entry = Element as T;

                UpdateLineColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot set property on attached control. Error: {ex.Message}");
            }
        }

        protected override void OnDetached()
        {
            _control = null;
            _entry = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(IDynamicLineColorControl.LineColorToApply))
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            try
            {
                if (_control != null && _entry != null)
                {
                    _control.Background?.SetColorFilter(_entry.LineColorToApply.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}