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
                OnFisrtAppearing();
                firstAppear = false;
            }

            foreach (var item in tabview.Items)
            {
                if (item.IsSelected)
                {
                    PageUtilities.OnNavigatedTo(item.Content, parameters);
                }
            }
        }

        private void OnFisrtAppearing()
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
                foreach (var item in tabview.Items)
                {
                    PageUtilities.OnNavigatedTo(item.Content, param);
                }
            });

        }
    }
}
