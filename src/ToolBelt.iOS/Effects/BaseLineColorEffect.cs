using CoreAnimation;
using CoreGraphics;
using System;
using System.ComponentModel;
using System.Linq;
using ToolBelt.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ToolBelt.iOS.Effects
{
    public class BaseLineColorEffect<T> : PlatformEffect
        where T : VisualElement, IDynamicLineColorControl
    {
        private UITextField _control;
        private T _entry;

        protected override void OnAttached()
        {
            try
            {
                _control = Control as UITextField;
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
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == nameof(IDynamicLineColorControl.LineColorToApply)
                || args.PropertyName == nameof(VisualElement.Height))
            {
                Initialize();
                UpdateLineColor();
            }
        }

        private void Initialize()
        {
            if (Element is VisualElement entry)
            {
                Control.Bounds = new CGRect(0, 0, entry.Width, entry.Height);
            }
        }

        private void UpdateLineColor()
        {
            var lineLayer = _control.Layer.Sublayers.OfType<BorderLineLayer>().FirstOrDefault();
            if (lineLayer == null)
            {
                lineLayer = new BorderLineLayer
                {
                    MasksToBounds = true,
                    BorderWidth = 1.0f
                };

                _control.Layer.AddSublayer(lineLayer);
                _control.BorderStyle = UITextBorderStyle.None;
            }

            lineLayer.Frame = new CGRect(0f, Control.Frame.Height - 1f, Control.Bounds.Width, 1f);
            lineLayer.BorderColor = _entry.LineColorToApply.ToCGColor();
            _control.TintColor = _control.TextColor;
        }

        private class BorderLineLayer : CALayer
        {
        }
    }
}