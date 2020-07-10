using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace VMS_MobileGPS.ViewModels
{
    public class FishQuantityInputViewModel : VMSBaseViewModel
    {
        private readonly IFishShipService fishShipService;

        private IList<FishTrip> listFishTrip = new ObservableCollection<FishTrip>();
        public IList<FishTrip> ListFishTrip { get => listFishTrip; set => SetProperty(ref listFishTrip, value); }

        public ICommand AddFishTripCommand { get; private set; }
        public ICommand ViewDetailCommand { get; private set; }
        public ICommand SyncFishTripCommand { get; private set; }

        public FishQuantityInputViewModel(INavigationService navigationService,
            IFishShipService fishShipService) : base(navigationService)
        {
            this.fishShipService = fishShipService;

            AddFishTripCommand = new Command(AddFishTrip);
            ViewDetailCommand = new Command<ItemTappedEventArgs>(ViewDetail);
            SyncFishTripCommand = new Command(SyncFishTrip);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            GetListFishTrip();
        }

        private void GetListFishTrip()
        {
            var result = fishShipService.GetListFishTrip().FindAll(f => !f.IsDeleted);

            if (result != null && result.ToList().Count > 0)
            {
                ListFishTrip = result.OrderByDescending(m => m.Id).ToObservableCollection();
            }
        }

        private void AddFishTrip()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync(PageNames.FishQuantityDetailPage.ToString());
            });
        }

        public void DeleteFishTrip(FishTrip fishTrip)
        {
            SafeExecute(() =>
            {
                fishShipService.DeleteFishTrip(fishTrip);
                ListFishTrip.Remove(fishTrip);
            });
        }

        private void ViewDetail(ItemTappedEventArgs args)
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync(PageNames.FishQuantityDetailPage.ToString(), parameters: new NavigationParameters
                {
                    { ParameterKey.PageMode, PageMode.Edit },
                    { ParameterKey.FishTrip, (FishTrip)args.ItemData }
                });
            });
        }

        private async void SyncFishTrip()
        {
            if (!IsConnected)
            {
                await PageDialog.DisplayAlertAsync("Vui lòng kết nối mạng để đồng bộ", "Mẻ Lưới", "Đóng");
                return;
            }

            if (!await PageDialog.DisplayAlertAsync("Bạn có muốn đồng bộ không?", "Mẻ lưới", "Đồng ý", "Bỏ qua"))
            {
                return;
            }

            // UserDialogs.Instance.ShowLoading("Đồng bộ mẻ lưới");

            SafeExecute(async () =>
            {
                var tasks = new List<Task>();

                var fishTrip = fishShipService.GetListFishTrip().FindAll(f => f.LastSynchronizationDate == null || f.LastSynchronizationDate != f.SysLastChangeDate);

                foreach (var item in fishTrip)
                {
                    item.StartTime = item.StartTime.ToLocalTime();
                    item.EndTime = item.EndTime.ToLocalTime();

                    tasks.Add(fishShipService.SyncFishTrip(item, fishShipService.GetFishTripDetail(item.Id)));
                }

                await Task.WhenAll(tasks);

                LoggerHelper.WriteLog("SyncFishTrip", "Đồng bộ thành công");
                //UserDialogs.Instance.Toast("Đồng bộ thành công");

                GetListFishTrip();
            }, onException: () =>
            {
                LoggerHelper.WriteLog("SyncFishTrip", "Xảy ra lỗi. Vui lòng thử lại trong ít phút.");
                //UserDialogs.Instance.Toast("Xảy ra lỗi. Vui lòng thử lại trong ít phút.");
            });

            // UserDialogs.Instance.HideLoading();
        }
    }
}