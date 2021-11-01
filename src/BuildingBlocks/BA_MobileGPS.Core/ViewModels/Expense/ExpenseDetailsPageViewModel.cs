using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.ViewModels.Expense
{
    public class ExpenseDetailsPageViewModel : ViewModelBase
    {
        #region Property

        public string vehiclePlate = string.Empty;

        public string VehiclePlate
        {
            get { return vehiclePlate; }
            set { SetProperty(ref vehiclePlate, value); }
        }

        public ExpenseDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            
        }

        #endregion Contructor

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

        #region PrivateMethod

        #endregion PrivateMethod
    }
}
