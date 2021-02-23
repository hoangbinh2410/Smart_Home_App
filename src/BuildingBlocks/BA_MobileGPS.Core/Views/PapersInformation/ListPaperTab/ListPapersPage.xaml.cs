using BA_MobileGPS.Core.Controls;
using Prism;
using Prism.Common;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prism.Ioc;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Entities;
using System.Linq;
using BA_MobileGPS.Utilities.Enums;

namespace BA_MobileGPS.Core.Views
{
    public partial class ListPapersPage : ContentPage, INavigationAware
    {
        public ListPapersPage()
        {
            InitializeComponent();


        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            foreach (var item in tabview.Items)
            {
                if (item.IsSelected)
                {
                    PageUtilities.OnNavigatedFrom(item.Content, parameters);
                }
            }
        }


        bool firstAppear = true;
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (firstAppear)
            {
                GetData(parameters);
                firstAppear = false;
            }
            else if(parameters.ContainsKey("RefreshListPaper") && parameters.GetValue<bool>("RefreshListPaper") is bool refresh)
            {
                //Load lại dữ liệu khi com back từ trang thêm sửa thông tin
                if (refresh)
                {
                    GetData(parameters);
                }
               
            }
            else
            {
                // filter param ở từng trang
                foreach (var item in tabview.Items)
                {
                    if (item.IsSelected)
                    {
                        PageUtilities.OnNavigatedTo(item.Content, parameters);
                    }
                }
            }
         
        }

        private void GetData(INavigationParameters parameters)
        {           
            Task.Run(async () =>
            {
                var paperService = PrismApplicationBase.Current.Container.Resolve<IPapersInforService>();
                await Task.Delay(500);              
                var response = await paperService.GetListPaper(StaticSettings.User.CompanyId);
                var originSource = response.Where(x => !string.IsNullOrEmpty(x.VehiclePlate)).OrderBy(x => x.VehiclePlate).ToList();
                foreach (var item in originSource)
                {
                    item.UpdateData(CompanyConfigurationHelper.DayAllowRegister);
                }
                var param = new NavigationParameters();
                param.Add("paperSource", originSource);
                if (parameters.ContainsKey("AlertType") && parameters.GetValue<PaperAlertTypeEnum>("AlertType") is PaperAlertTypeEnum alertType)
                {
                    param.Add("AlertType", alertType);
                }
                foreach (var item in tabview.Items)
                {
                    PageUtilities.OnNavigatedTo(item.Content, param);
                }
            });

        }
    }
}
