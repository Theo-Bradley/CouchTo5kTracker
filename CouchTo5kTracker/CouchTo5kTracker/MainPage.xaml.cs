using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CouchTo5kTracker
{
    public partial class MainPage : ContentPage
    {
        public string colorProperty;

        bool buttonFlipFlop = false;
        public MainPage()
        {
            InitializeComponent();
            int initialColor = (int)Math.Round(ColorSlider.Value);
            TestButton.BackgroundColor = Color.FromRgb(initialColor, initialColor, initialColor);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!buttonFlipFlop)
            {
                (sender as Button).BackgroundColor = Color.FromRgb(50, 50, 50);
                buttonFlipFlop = true;
            }
            else
            {
                (sender as Button).BackgroundColor = Color.FromRgb(150, 150, 150);
                buttonFlipFlop = false;
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int colorValue = (int)Math.Round(ColorSlider.Value);
            TestButton.BackgroundColor = Color.FromRgb(colorValue, colorValue, colorValue);
            System.Diagnostics.Debug.WriteLine(colorValue.ToString());
        }
    }
}