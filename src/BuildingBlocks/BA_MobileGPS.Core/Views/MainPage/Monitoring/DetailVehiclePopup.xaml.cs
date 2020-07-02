using Prism.Events;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Ioc;
using BA_MobileGPS.Core.Events;
using Rg.Plugins.Popup.Services;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailVehiclePopup : PopupPage
    {
        private readonly IEventAggregator _eventAggregator;
        public DetailVehiclePopup()
        {          
            InitializeComponent();
            _eventAggregator = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        }

        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            _eventAggregator.GetEvent<DetailVehiclePopupCloseEvent>().Publish();
            return true;
        }

    }
}