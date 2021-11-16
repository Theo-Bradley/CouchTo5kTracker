using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using CouchTo5kTracker.Classes;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CouchTo5kTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentRunPage : ContentPage
    {
        private bool back = false;
        private float progress = 0.5f; //test value, later set to 0.01f (minimum defined by the clamp)
        private float screenWidth;
        private bool runPasued = false;


        SKPaint backPaint = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill, //set the renderer to draw the outline and the inside of the path
            StrokeWidth = 35, //outline width
            Color = SKColor.FromHsl(0, 0, 91), //light grey color
            StrokeCap = SKStrokeCap.Round, //round ends
            IsAntialias = true //enable antialiasing
        };

        SKPaint barPaint = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill, //set renderer to draw the outline and the inside of the path
            StrokeWidth = 35, //outline width
            Color = SKColors.Red, //default color
            StrokeCap = SKStrokeCap.Round, //round ends
            IsAntialias = true //enable antialiasing
        };

        public CurrentRunPage()
        {
            InitializeComponent();
            BackButton.ImageSource = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.backBlue)); //load up the back arrow image for the top left
            screenWidth = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width; //get the width of the screen in pixels
            canvasView.WidthRequest = screenWidth; //expand the canvas to fill the width of the screen in pixels
            canvasView.HeightRequest = 64; //set the progress bar canvas to the minimum possible height
            //BackButton.Scale = 0.4;
            //<Image x:Name="BackImage" AbsoluteLayout.LayoutBounds="0.1, 0.1, 0.4, 0.37" AbsoluteLayout.LayoutFlags="All"/>
        }

        private void resetBackAsync()
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                back = false; //set back to not pressed
                return false; // True = Repeat again, False = Stop the timer
            });
        }

        protected override bool OnBackButtonPressed()
        {
            resetBackAsync(); //restart the back timer
            if (!back)
            {
                back = true; //set back to pressed once
                //add press back again to exit notification popup/toast here
            }
            else
                Navigation.PopModalAsync(true); //go back a page

            return true;
        }

        private void BackButton_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true); //go back a page if the top back arrow image is pressed
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args) //when canvas is meant to be rendered
        {
            SKSurface surface = args.Surface; //reference to the xaml skiasharp surface
            SKCanvas canvas = surface.Canvas; //reference to the drawing canvas
            SKColor[] gradColors = new SKColor[2]; //array of the gradient colours
            gradColors[0] = SKColor.FromHsl(356, 83, 45); //left bar colour
            gradColors[1] = SKColor.FromHsl(209, 83, 45); //right bar colour
            canvas.Clear(); //clears the canvas, ready for drawing (unnecessary)


            SKRect baseRect = new SKRect(35, 40, screenWidth - 35, 64); //makes a rectagle on which the rounded rectangle for the background bar is based
            SKRect barRect = new SKRect(35, 40, ((screenWidth - 70) * Mathf.Clamp<float>(progress, 0.01f, 1f)) + 35.1f, 64); //same as above, but for the foreground bar, which scales between 1% (prevents glitch with renderer) and 100% (0.01-1)
            SKRoundRect backBar = new SKRoundRect(baseRect, 90f); //constructs appropriate bar for the background of the progress bar
            SKRoundRect frontBar = new SKRoundRect(barRect, 90f); //constructs appropriate bar for the foreground of the progress bar

            using (SKPath basePath = new SKPath()) //start drawing the backgound path
            {
                basePath.AddRoundRect(backBar); //add the background bar
                canvas.DrawPath(basePath, backPaint); //draw to canvas
            }

            barPaint.Shader = SKShader.CreateLinearGradient(new SKPoint(baseRect.Left, baseRect.MidY), new SKPoint(baseRect.Right, baseRect.MidY), gradColors, SKShaderTileMode.Clamp); //create a gradient on the foreground bar
       
            using (SKPath barPath = new SKPath()) //start drawing the foreground path
            {
                barPath.AddRoundRect(frontBar); //add the foreground bar
                barPath.AddCircle(baseRect.Left + 6, baseRect.MidY, 12); //add a circle to keep the bar visually appealing at very low progress
                canvas.DrawPath(barPath, barPaint); //draw both to canvas 
            }

            PrepareCanvas(); //tell canvas it needs to be redrawn
        }

        private void PrepareCanvas()
        {
            canvasView.InvalidateSurface(); //tell the canvas it needs to be redrawn
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args) //testing
        {
            double value = args.NewValue; //get the new slider value and assign it to a double
            progress = (float)value; //cast the double to a float and assign it to the progress variable
            System.Console.WriteLine("Value: " + value.ToString()); //print the slider value to the console
            System.Console.WriteLine("Position: " + ((screenWidth - 70) * Mathf.Clamp<float>(progress, 0.01f, 1f)) + 35.1f); //print the position of the end of the bar (calculated in the same way as the end of the bar)
            PrepareCanvas(); //tell the canvas it needs to be redrawn
        }

        private void PauseButton_Pressed(object sender, EventArgs e) //called when the play/paused label was pressed
        {
            if (!runPasued) //if the run is ongoing
            {
                PausePlayButton.Text = "Resume"; //change text
                Console.WriteLine("Paused. \n"); //debug message
                PausePlayFrame.BackgroundColor = Color.FromHex("FF0000"); //change to better red colour
                PausePlayFrame.BorderColor = Color.FromHex("FF0000"); //change to better red colour
                runPasued = true; //change paused flip-flop bool
            }
            else //if the run is paused
            {
                PausePlayButton.Text = "Pause"; //change text
                Console.WriteLine("Resumed. \n"); //debug message
                PausePlayFrame.BackgroundColor = Color.FromHex("1476D2"); //set background colour to default blue
                PausePlayFrame.BorderColor = Color.FromHex("1476D2"); //set border colour to default blue
                runPasued = false; //change paused flip-flop bool
            }
        }

        private void StopButton_Pressed(object sender, EventArgs e) //is stop button pressed
        {
            Navigation.PopModalAsync(); //Remove most recently added page (Current Run page)
        }
    }
}