using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CouchTo5kTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentRunPage : ContentPage
    {
        bool back = false;

        public CurrentRunPage()
        {
            InitializeComponent();
            BackButton.ImageSource = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.backBlue));
            //BackButton.Scale = 0.4;
            //<Image x:Name="BackImage" AbsoluteLayout.LayoutBounds="0.1, 0.1, 0.4, 0.37" AbsoluteLayout.LayoutFlags="All"/>
        }

        private void resetBackAsync()
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                back = false;
                return false; // True = Repeat again, False = Stop the timer
            });
        }

        protected override bool OnBackButtonPressed()
        {
            resetBackAsync();
            if (!back)
            {
                back = true;
                //add press back again to exit notification popup here
            }
            else
                Navigation.PopModalAsync(true);
            
            return true;
        }

        private void BackButton_Pressed(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
    }
}