using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
        private float progress;
        private float screenWidth;

        public HomePage()
        {
            InitializeComponent();
            screenWidth = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
            //float pageWidth = (float)Application.Current.MainPage.Width;
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength((screenWidth/2) - 200, GridUnitType.Absolute)});
            System.Diagnostics.Debug.WriteLine((screenWidth/2) - 200);
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star});
        }

        public void SetProgress(float value) //interaction method
        {
            progress = value; //update progress
            PrepareCanvas();
        }

        SKPaint arcPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 25, //bar width
            Color = SKColors.Red, //default color
            StrokeCap = SKStrokeCap.Round //round ends
        };

        SKPaint backPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 25, //bar width
            Color = SKColor.FromHsl(0, 0, 91), //light grey color
            StrokeCap = SKStrokeCap.Round //round ends
        };

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args) //when canvas is meant to be rendered
        {
            SKSurface surface = args.Surface; //reference to the xaml skiasharp surface
            SKCanvas canvas = surface.Canvas; //reference to the drawing canvas
            SKColor[] sweepColors = new SKColor[2];  //array of the colors to be blended by gradient
            sweepColors[0] = SKColor.FromHsl(203, 36, 33); //initial color
            sweepColors[1] = SKColor.FromHsl(345, 45, 48); //final color
            canvas.Clear(); //clear the canvas of drawings
            

            SKRect rect = new SKRect(100, 100, (float)screenWidth - 150, (float)screenWidth - 150); // rectangle the bars occupy, made to fit available space
            float startAngle = 40f; //initial angle from the bottom for the bars
            progress = (float)SweepSlider.Value; //set the progress% to a slider value for testing
            float maxAngle = 360f - (2 * startAngle); //calculate hte maximum sweep angle

            float sweepAngle = (maxAngle) * (progress/100); //interpolate between the canvas

            canvas.Save(); //save canvas's original state
            canvas.RotateDegrees(90f, rect.MidX, rect.MidY); //rotate the canvas about the middle of the progress bar

            using (SKPath basePath = new SKPath()) //start drawing the backgound path
            {
                basePath.AddArc(rect, startAngle, maxAngle); //draw arc in the same area as the foreground path, from the same start angle by the max angle
                canvas.DrawPath(basePath, backPaint); //put the background path onto the screen first
            }

            arcPaint.Shader = SKShader.CreateSweepGradient(new SKPoint(rect.MidX, rect.MidY), sweepColors); //create the sweep gradient (must be after canvas is rotated)

            using (SKPath path = new SKPath()) //start drawing the foreground path
            {
                path.AddArc(rect, startAngle, sweepAngle); //draw arc in the same area as the as the background path, from the start angle by the max angle
                canvas.DrawPath(path, arcPaint); //put the foreground onto the path after the background path
            }

            canvas.Restore(); //restore the canvas's state
        }

        private void SweepSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            PrepareCanvas();
        }

        private void PrepareCanvas()
        {
            canvasView.InvalidateSurface(); //tell the canvas it needs to be redrawn
        }
    }
}