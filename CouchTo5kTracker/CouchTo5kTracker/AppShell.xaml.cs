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
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetTabBarIsVisible(this, false);
            Shell.SetBackgroundColor(this, Color.FromHex("1476D2"));

            ImageSource menuImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.menu));
            FlyoutMain.FlyoutIcon = menuImage;

            ImageSource homeImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.home));
            FlyoutHomeTab.Icon = homeImage;

            ImageSource timerImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.timer));
            FlyoutTimerTab.FlyoutIcon = timerImage;

            ImageSource listImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.list));
            FlyoutRunsTab.FlyoutIcon = listImage;

            ImageSource percentImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.percent));
            FlyoutProgressTab.FlyoutIcon = percentImage;

            ImageSource calendarImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.calendar));
            FlyoutCalendarTab.FlyoutIcon = calendarImage;

            ImageSource settingsImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.settings));
            FlyoutSettingsTab.FlyoutIcon = settingsImage;
        }
    }
}