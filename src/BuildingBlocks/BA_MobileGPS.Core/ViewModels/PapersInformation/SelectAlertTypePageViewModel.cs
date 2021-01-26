using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities.Enums;
using BA_MobileGPS.Utilities.Extensions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectAlertTypePageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
      
        public ICommand SelectAlertTypeCommand { get; }
        public SelectAlertTypePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectAlertTypeCommand = new DelegateCommand<object>(SelectAlertType);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListAlert();
        }

        private void GetListAlert()
        {
            var allEnum = Enum.GetValues(typeof(PaperAlertTypeEnum)).Cast<PaperAlertTypeEnum>();
            var list = new List<PaperAlertTypeModel>();
            foreach (var item in allEnum)
            {
                var temp = new PaperAlertTypeModel()
                {
                    Name = item.ToDescription(),
                    EnumType = item
                };
                list.Add(temp);
            }
            ListAlerts = list;
        }


        private void SelectAlertType(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs agrs)
            {
                var item = (PaperAlertTypeModel)agrs.ItemData;
                var param = new NavigationParameters();
                param.Add("AlertType", item.EnumType);
                SafeExecute(async () =>
                {
                    var a = await NavigationService.GoBackAsync(param, true, true);
                });
            }
        }

        private List<PaperAlertTypeModel> listAlerts;
        public List<PaperAlertTypeModel> ListAlerts
        {
            get { return listAlerts; }
            set
            {
                SetProperty(ref listAlerts, value);
            }
        }

    }

    public class PaperAlertTypeModel
    {
        public string Name { get; set; }
        public PaperAlertTypeEnum EnumType { get; set; }
    }


   
}
