using System;
using System.IO;
using System.Reflection;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace AnorocMobileApp.Controls
{
    /// <summary>
    /// This is a helper class to render the SVG files. 
    /// </summary>
    public class SVGImage : ContentView
    {
        // Bindable property to set the SVG image path
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(
          nameof(Source), typeof(string), typeof(SVGImage), default(string), propertyChanged: RedrawCanvas);

        private readonly SKCanvasView canvasView = new SKCanvasView();

        public SVGImage()
        {
            this.Padding = new Thickness(0);
            this.BackgroundColor = Color.Transparent;
            this.Content = this.canvasView;
            this.canvasView.PaintSurface += this.CanvasView_PaintSurface;
        }

        // Property to set the SVG image path
        public string Source
        {
            get => (string)this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Method to invaldate the canvas to update the image
        /// </summary>
        /// <param name="bindable">The target canvas</param>
        /// <param name="oldValue">Previous state</param>
        /// <param name="newValue">Updated state</param>
        public static void RedrawCanvas(BindableObject bindable, object oldValue, object newValue)
        {
            var sVGImage = bindable as SVGImage;
            sVGImage?.canvasView.InvalidateSurface();
        }

        /// <summary>
        /// This method update the canvas area with teh image
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The arguments</param>
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var skCanvas = args.Surface.Canvas;
            skCanvas.Clear();

            if (string.IsNullOrEmpty(this.Source))
            {
                return;
            }

            // Get the assembly information to access the local image
            var assembly = typeof(SVGImage).GetTypeInfo().Assembly.GetName();

            // Update the canvas with the SVG image
            using (var stream = typeof(SVGImage).GetTypeInfo().Assembly.GetManifestResourceStream(assembly.Name + ".Images." + Source))
            {
                var skSVG = new SkiaSharp.Extended.Svg.SKSvg();
                skSVG.Load(stream);
                var imageInfo = args.Info;
                skCanvas.Translate(imageInfo.Width / 2f, imageInfo.Height / 2f);
                var rectBounds = skSVG.ViewBox;
                var xRatio = imageInfo.Width / rectBounds.Width;
                var yRatio = imageInfo.Height / rectBounds.Height;
                var minRatio = Math.Min(xRatio, yRatio);
                skCanvas.Scale(minRatio);
                skCanvas.Translate(-rectBounds.MidX, -rectBounds.MidY);
                skCanvas.DrawPicture(skSVG.Picture);
            }
        }
    }
}
