using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListDriverPageViewModel : ViewModelBase
    {

        public ICommand SelectDriverCommand { get; }
        public ListDriverPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectDriverCommand = new DelegateCommand<object>(SelectDriver);
        }

        private void SelectDriver(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (DriverModel)agrs.ItemData;
            }
        }

       

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetDriverSource();
        }

        private void SetDriverSource()
        {
            for (int i = 0; i < 10; i++)
            {
                ListDriver.Add(new DriverModel());
            }
        }


        private ObservableCollection<DriverModel> listDriver;
        public ObservableCollection<DriverModel> ListDriver
        {
            get { return listDriver; }
            set { SetProperty(ref listDriver, value); }
        }
    }

    public class DriverModel
    {
        public readonly Guid Id = new Guid();
        public string AvatarUrl { get; set; } = "avatar_default";
        public string FullName { get; set; } = " A B C D";
        public DateTime BirthDay { get; set; } = DateTime.Today.AddDays(-1800);
        public string IdentityNumber { get; set; } = "123123123";
        public string PhoneNum { get; set; } = "12354353534";
        public string Address { get; set; } = " Tỉnh A Phố B";
        public DriverLicenseEnum DriverLicenseType { get; set; } = DriverLicenseEnum.A3;
        public string DriverLicense { get; set; } = "123123123544";
        public DateTime LicenseEffectiveDate { get; set; } = DateTime.Today.AddDays(-800);
        public DateTime LicenseExpriedDate { get; set; } = DateTime.Today.AddDays(-300);
        public string DriverLicenseTypeName
        {
            get
            {
                return string.Format("Bằng lái {0}", DriverLicenseType.ToString());
            }
        }
    }

    public enum DriverLicenseEnum
    {
        A1,
        A2,
        A3,
        A4,
        B1,B2,C,D,E,F
    }
}
