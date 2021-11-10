using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListVehicleSupportPageViewModel : ViewModelBase
    {
        #region Property
        private ObservableCollection<VehicleGroupModel> listVehicleGroup = new ObservableCollection<VehicleGroupModel>();
        public ObservableCollection<VehicleGroupModel> ListVehicleGroup 
        { 
            get => listVehicleGroup; 
            set => SetProperty(ref listVehicleGroup, value); 
        }
        #endregion
        #region Contructor
        public ListVehicleSupportPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        public ICommand SelectedTapGroupCommand
        {
            get
            {
                return new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>((args) =>
                {
                    try
                    {
                        if (args == null)
                            return;

                        var seleted = (args.ItemData as VehicleGroupModel);
                        if (seleted != null)
                        {
                            if (seleted.IsSelected)
                            {
                                seleted.IsSelected = false;
                            }
                            else
                            {
                                seleted.IsSelected = true;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                });
            }
        }
        #endregion
        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
           
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
           
        }

        #endregion Lifecycle

        #region PrivateMenthod

        #endregion
    }
}
