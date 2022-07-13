using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurnLampView : ContentPage
    {
        public TurnLampView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {

            }
        }
    }
}