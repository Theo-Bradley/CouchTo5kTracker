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
            screenWidth = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width; //get the width of the screen in pixels
            canvasView.WidthRequest = screenWidth; //expand the canvas to fill the width of the screen in pixels
            canvasView.HeightRequest = screenWidth; //keep the canvas square
            //RunGrid.RowSpacing = 0f; //Push the next run up to the next run title
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
            StrokeCap = SKStrokeCap.Round, //round ends
            IsAntialias = true //enable antialiasing
        };

        SKPaint backPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 25, //bar width
            Color = SKColor.FromHsl(0, 0, 91), //light grey color
            StrokeCap = SKStrokeCap.Round, //round ends
            IsAntialias = true //enable antialiasing
        };

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args) //when canvas is meant to be rendered
        {
            SKSurface surface = args.Surface; //reference to the xaml skiasharp surface
            SKCanvas canvas = surface.Canvas; //reference to the drawing canvas
            SKColor[] sweepColors = new SKColor[2];  //array of the colors to be blended by gradient
            sweepColors[0] = SKColor.FromHsl(356, 83, 45); //initial color
            sweepColors[1] = SKColor.FromHsl(209, 83, 45); //final color
            canvas.Clear(); //clear the canvas of drawings

            SKRect rect = new SKRect(100, 100, screenWidth - 100, screenWidth - 100); // rectangle the bars occupy, made to fit available space
            float startAngle = 40f; //initial angle from the bottom for the bars
            progress = 50; //set the progress% to 50 for testing
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e) //Next run frame/date clicked
        {
            await Navigation.PushAsync(new RunsPage()); //open the RunsPage
        }

        private async void TapGestureRecognizer_Tapped_Run(object sender, EventArgs e) //start run button tapped
        {
            await Navigation.PushModalAsync(new CurrentRunPage()); //change TimerPage to the relevant page for this run, then a back button to PopModalAsync
            //on clicking back have a dialogue before using PopModalAsync to prevent accidental closures: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/pop-ups
        }
    }
}