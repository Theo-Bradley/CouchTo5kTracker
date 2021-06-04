using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace CouchTo5kTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        SKPaint outlinePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 3,
            Color = SKColors.Black
        };

        SKPaint arcPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 25,
            Color = SKColors.Red,
            StrokeCap = SKStrokeCap.Round
        };

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKRect rect = new SKRect(100, 100, 400, 400);
            float startAngle = 10f;
            float sweepAngle = (float)SweepSlider.Value;

            canvas.DrawOval(rect, outlinePaint);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                //arcPaint.PathEffect = SKPathEffect.CreateCorner(90f);
                canvas.DrawPath(path, arcPaint);
            }
        }

        private void SweepSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            canvasView.InvalidateSurface();
        }
    }
}