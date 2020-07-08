using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Microsoft.AppCenter;
using Prism.Commands;
using Prism.Common;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Realms.Sync;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VMS_MobileGPS.Views;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
   public class OnlineCarInfoViewModel : BindableBase
    {
        #region Contractor
       

        public OnlineCarInfoViewModel(BA_MobileGPS.Entities.VehicleOnline a) 
        {
            CarActive = a;
        }

        #endregion
        private VehicleOnline carActive;

        public VehicleOnline CarActive
        {
            get { return carActive; }
            set
            {
                carActive = value;
                RaisePropertyChanged();
            }
        }

    }
}
