using System.Collections.Generic;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.ItemsSource = new List<Page>() {new HomePage(), new HomePage(), new HomePage() };
        }
    }
}
