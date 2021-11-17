using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Newtonsoft.Json;
using CouchTo5kTracker.Classes;

namespace CouchTo5kTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
#if __IOS__
const var resourcePrefix = "CouchTo5kTracker.iOS.";
#endif
#if __ANDROID__
const var resourcePrefix = "CouchTo5kTracker.Droid.";
#endif

        private float progress = 0f;
        private float screenWidth;

        public HomePage()
        {
            InitializeComponent(); //initialize the xaml components
            screenWidth = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width; //get the width of the screen in pixels
            canvasView.WidthRequest = screenWidth; //expand the canvas to fill the width of the screen in pixels
            canvasView.HeightRequest = screenWidth; //keep the canvas square
            string jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "savedRuns.json"); //set the path to the json file containing the saved runs
            //RunGrid.RowSpacing = 0f; //Push the next run up to the next run title

            List<Instruction> testInstructions = new List<Instruction> { };
            Run testRun = new Run("Test Run", new Date(12, 5), Run.WeekEnum.W4, Run.RunEnum.R2, new Time(21, 0), testInstructions); //create a test run to set as next run

            JSON.save(jsonPath, testRun); //save the test run to the saved runs file
            string nextRunJson = JSON.load(jsonPath); //load the next run from the saved runs file
            Run jsonRun = JSON.parse<Run>(nextRunJson); //parse the run and create a new local run from the parsed json

            setNextRun(jsonRun); //update the run info to the local run
        }

        public void SetProgress(float value) //interaction method
        {
            progress = value; //update progress
            PrepareCanvas(); //tell canvas to update
        }

        SKPaint arcPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke, //set renderer to draw outline
            StrokeWidth = 25, //bar width
            Color = SKColors.Red, //default color
            StrokeCap = SKStrokeCap.Round, //round ends
            IsAntialias = true //enable antialiasing
        };

        SKPaint backPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke, //set the renderer to draw outline
            StrokeWidth = 25, //bar width
            Color = SKColor.FromHsl(0, 0, 91), //light grey color
            StrokeCap = SKStrokeCap.Round, //round ends
            IsAntialias = true //enable antialiasing
        };

        private void setNextRun(Run nextRun)
        {
            setNextRunLabel(nextRun.GetWeek(), nextRun.GetRun()); //set next run label
            setNextRunDateLabel(nextRun.GetDate().GetDay(), nextRun.GetDate().GetMonth()); //set next run date label
            setWeekCounter(nextRun.GetWeek()); //set week label
            setNextRunTimeLabel(nextRun.GetLengthTime()); //set run time label
            float totalProgress = (((nextRun.GetWeek() - 1f) * 3f) + nextRun.GetRun()) / 15f; //set progress by getting current run ((week-1)*3 + run) / total runs (fixed number)
            SetProgress(totalProgress * 100f); //update progress bar
        }

        private void setNextRunLabel(int week, int run)
        {
            NextRunLabel.Text = String.Format("Next Run: Week {0}, Run {1}", week, run); //format the next run label to an appropriate format
        }

        private void setNextRunDateLabel(int day, int month)
        {
            NextRunDateLabel.Text = String.Format("{0}/{1}", day, month); //format the next run date label to an appropriate format
        }

        private void setWeekCounter(int week)
        {
            WeekCounter.Text = String.Format("{0}/9", week); //format the week label to an appropriate format
        }

        private void setNextRunTimeLabel(Time time)
        {
            int seconds = time.GetSeconds(); //set the local int seconds to the seconds from the class
            int minutes = time.GetMinutes(); //set the local int minutes to the minutes from the class
            RunTimeLabel.Text = String.Format("{0}:{1}", minutes, seconds.ToString("D2")); //format the run label to an appropriate format
        }

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
            //progress = 50; //set the progress% to 50 for testing
            float maxAngle = 360f - (2 * startAngle); //calculate the maximum sweep angle

            float sweepAngle = maxAngle * (progress/100f); //interpolate between the max and min angle by progress
            //float sweepAngle = 90f;

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