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

            ImageSource homeImage = ImageSource.FromStream(() => new MemoryStream(Properties.Resources.home));
            FlyoutHomeTab.Icon = homeImage;
        }
    }
}