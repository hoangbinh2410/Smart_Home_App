using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System;


using System.Windows.Input;

using System.Collections.ObjectModel;
using BA_MobileGPS.Utilities;
using System.Reflection;
using BA_MobileGPS.Entities;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;
using BA_MobileGPS.Core.Constant;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageManagingPageViewModel : ViewModelBase
    {

        private readonly IStreamCameraService _streamCameraService;

        public ICommand TapItemsCommand { get; set; }

        public ICommand TabCommandDetail { get; set; }

        public ICommand TapCommandListGroup { get; set; }

        public ICommand TabCommandFavorites { get; set; }

        public ICommand LoadMoreItemsCommand { get; set; }


        public ImageManagingPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            SpanCount = 1;
            ListHeight = 1;
            TapItemsCommand = new DelegateCommand<object>(TapItems);
            TabCommandDetail = new DelegateCommand<object>(TabDetail);
            TapCommandListGroup = new DelegateCommand<ItemTappedEventArgs>(TapListGroup);
            TabCommandFavorites = new DelegateCommand<object>(TabFavorites);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            CarSearch = string.Empty;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            ShowImage();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                CarSearch = vehiclePlate.VehiclePlate;
                ShowImage();
            }
            else if (parameters.ContainsKey(ParameterKey.Company) && parameters.GetValue<Company>(ParameterKey.Company) is Company company)
            {

            }
            else if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;
                CarSearch = string.Empty;
            }
            ShowLastView();

        }

        private void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                PageIndex++;

                ShowImage();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                listview.IsBusy = false;
                IsBusy = false;
            }
        }

        private bool CanLoadMoreItems(object obj)
        {
            //if (ListNotice.Count < PageIndex * PageCount || IsMaxLoadMore)
            //    return false;
            return true;
        }

        private async void TapListGroup(ItemTappedEventArgs args)
        {
            try
            {
                if (!(args.ItemData is CaptureImageData item) || string.IsNullOrEmpty(item.Url))
                    return;
                else
                {
                    // chuyen dang detail
                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.ImageCamera, item },
                        { ParameterKey.VehiclePlate, item.VehiclePlate }
                    };
                    await NavigationService.NavigateAsync("ImageDetailPage", parameters, useModalNavigation: false);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void TabDetail(object obj)
        {
            //chuyên trang detail
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.VehiclePlate, obj }
                };
                await NavigationService.NavigateAsync("ImageDetailPage", parameters, useModalNavigation: false);
            });
        }

        private void TabFavorites(object obj)
        {
            //chuyên trang detail
        }

        private async void TapItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            try
            {
                // truyền key xử lý ở đây
                var VehiclePlate = ((LastViewVehicleImageModel)listview.ItemData).Name;

                // chuyen dang detail
                var parameters = new NavigationParameters
                {
                    { ParameterKey.VehiclePlate, VehiclePlate }
                };
                await NavigationService.NavigateAsync("ImageDetailPage", parameters, useModalNavigation: false);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private int spanCount;
        public int SpanCount { get => spanCount; set => SetProperty(ref spanCount, value); }

        private int listHeight;
        public int ListHeight { get => listHeight; set => SetProperty(ref listHeight, value); }

        private string carSearch;
        public string CarSearch { get => carSearch; set => SetProperty(ref carSearch, value); }

        private int pageIndex;

        public int PageIndex { get => pageIndex; set => SetProperty(ref pageIndex, value); }

        private int pageCount;

        public int PageCount { get => pageCount; set => SetProperty(ref pageCount, value); }

        private bool isMaxLoadMore;

        public bool IsMaxLoadMore { get => isMaxLoadMore; set => SetProperty(ref isMaxLoadMore, value); }

        private ObservableCollection<LastViewVehicleImageModel> listLastView;

        public ObservableCollection<LastViewVehicleImageModel> ListLastView { get => listLastView; set => SetProperty(ref listLastView, value); }

        private List<LastViewVehicleImageModel> lstVehicleString { get; set; }

        private List<string> mVehicleString
        {
            get
            {
                if (StaticSettings.ListVehilceOnline != null)
                {
                    //nếu khóa BAP rồi thì ko hiển thị trên Map nữa
                    return StaticSettings.ListVehilceOnline.Select(x => x.VehiclePlate).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        private ObservableCollection<CaptureImageData> listGroup;

        public ObservableCollection<CaptureImageData> ListGroup { get => listGroup; set => SetProperty(ref listGroup, value); }

        private bool isShowLastViewVehicle = true;
        public bool IsShowLastViewVehicle { get => isShowLastViewVehicle; set => SetProperty(ref isShowLastViewVehicle, value); }

        private void ShowImage()
        {
            using (new HUDService())
            {
                TryExecute(async () =>
                {
                    var request = new StreamImageRequest()
                    {
                        xnCode = 7644,
                        VehiclePlates = "79B03279,29B15081"
                    };

                    //var request = new StreamImageRequest()
                    //{
                    //    xnCode = 999,
                    //    VehiclePlates = "QCPASS_HIEUDT,TESTTBI,QCPASSTHAIVV"
                    //};

                    var response = await _streamCameraService.GetListCaptureImage(request);

                    if (response != null && response.Count > 0)
                    {
                        ListGroup = new ObservableCollection<CaptureImageData>(response);
                    }
                    else
                    {
                        ListGroup = new ObservableCollection<CaptureImageData>();
                    }
                });
            }
        }

        private List<string> GetListPage(List<string> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }

        private void ShowLastView()
        {
            // nếu lần đầu vào chưa có lịch sử xem gần nhất
            if (string.IsNullOrEmpty(Settings.LastViewVehicleImage))
            {
                // ẩn cột đi
                IsShowLastViewVehicle = false;
            }
            else
            {
                IsShowLastViewVehicle = true;

                var split = Settings.LastViewVehicleImage.Split(',');

                var view = GetImageLastView(split.Length);

                SpanCount = view[0]; // số lượng xếp.

                Settings.ShowViewVehicleImage = view[0] * 2; // số lượng show

                ListHeight = view[1]; // gán chiều cao cho listview

                ListLastView = new ObservableCollection<LastViewVehicleImageModel>();
                for (int i = 0; i < split.Length; i++)
                {
                    ListLastView.Add(new LastViewVehicleImageModel
                    {
                        Name = split[i]
                    });
                }
            }
        }

        public int[] GetImageLastView(int count)
        {
            var respone = new int[2];
            var width = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
            if (width > 1080)
            {
                respone[0] = 5;
                respone[1] = count > 5 ? 80 : 40;
            }
            else if (width <= 1080 && width > 768)
            {
                respone[0] = 4;
                respone[1] = count > 4 ? 80 : 40;
            }
            else // < 768
            {
                respone[0] = 3;
                respone[1] = count > 3 ? 80 : 40;
            }
            return respone;
        }

    }

    public class LastViewVehicleImageModel
    {
        public string Name { get; set; }
    }
}
