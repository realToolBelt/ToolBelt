﻿using CoreAnimation;
using CoreGraphics;
using System;
using System.ComponentModel;
using System.Linq;
using ToolBelt.Controls;
using ToolBelt.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace ToolBelt.iOS.Renderers
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        public ExtendedEditor ExtendedEditorElement => Element as ExtendedEditor;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            LineLayer lineLayer = GetOrAddLineLayer();
            lineLayer.Frame = new CGRect(0, Frame.Size.Height - LineLayer.LineHeight, Control.Frame.Size.Width, LineLayer.LineHeight);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                //if (Control != null)
                //{
                // TODO: Do we need to add this for the editor like we do for an Entry?
                //    Control.BorderStyle = UIKit.UITextBorderStyle.None;
                //}

                UpdateLineColor();
                UpdateCursorColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEditor.LineColorToApply)))
            {
                UpdateLineColor();
            }
            else if (e.PropertyName.Equals(Entry.TextColorProperty.PropertyName))
            {
                UpdateCursorColor();
            }
        }

        private LineLayer GetOrAddLineLayer()
        {
            var lineLayer = Control.Layer.Sublayers?.OfType<LineLayer>().FirstOrDefault();

            if (lineLayer == null)
            {
                lineLayer = new LineLayer();

                Control.Layer.AddSublayer(lineLayer);
                Control.Layer.MasksToBounds = true;
            }

            return lineLayer;
        }

        private void UpdateCursorColor()
        {
            Control.TintColor = Element.TextColor.ToUIColor();
        }

        private void UpdateLineColor()
        {
            LineLayer lineLayer = GetOrAddLineLayer();
            lineLayer.BorderColor = ExtendedEditorElement.LineColorToApply.ToCGColor();
        }

        private class LineLayer : CALayer
        {
            public static nfloat LineHeight = 2f;

            public LineLayer()
            {
                BorderWidth = LineHeight;
            }
        }
    }
}