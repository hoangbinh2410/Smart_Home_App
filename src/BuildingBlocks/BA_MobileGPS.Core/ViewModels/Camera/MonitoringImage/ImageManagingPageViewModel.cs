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
using Syncfusion.Data.Extensions;

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

        public ICommand RefeshCommand { get; set; }

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
            RefeshCommand = new DelegateCommand(ShowImage);
            CarSearch = string.Empty;
            PageCount = 5;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetVehicleString();
            ShowLastView();
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
                GetVehicleString();
                ShowImage();
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

        private ObservableCollection<CaptureImageData> listGroup = new ObservableCollection<CaptureImageData>();

        public ObservableCollection<CaptureImageData> ListGroup { get => listGroup; set => SetProperty(ref listGroup, value); }

        private bool isShowLastViewVehicle = true;
        public bool IsShowLastViewVehicle { get => isShowLastViewVehicle; set => SetProperty(ref isShowLastViewVehicle, value); }

        private ObservableCollection<LastViewVehicleImageModel> listLastView;

        public ObservableCollection<LastViewVehicleImageModel> ListLastView { get => listLastView; set => SetProperty(ref listLastView, value); }

        private List<string> mVehicleString { get; set; }


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
            if (mVehicleString.Count < PageIndex * PageCount || IsMaxLoadMore)
                return false;
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

        private void GetVehicleString()
        {
            if (StaticSettings.ListVehilceOnline != null)
            {
                var listOnline = StaticSettings.ListVehilceOnline.Where(x => x.MessageId != 65 && x.MessageId != 254 && x.MessageId != 128).ToList();
                if (VehicleGroups != null && VehicleGroups.Length > 0)
                {
                    mVehicleString = listOnline.FindAll(v => v.GroupIDs.Split(',').ToList()
                        .Exists(g => VehicleGroups.Contains(Convert.ToInt32(g))))
                        .Select(x => x.VehiclePlate).ToList();
                }
                else
                {
                    mVehicleString = listOnline.Select(x => x.VehiclePlate).ToList();
                }
            }
            else
            {
                mVehicleString = new List<string>();
            }
        }

        private void ShowImage()
        {
            using (new HUDService())
            {
                TryExecute(() =>
                {
                    if (CarSearch != string.Empty)
                    {
                        ShowImageSearch();
                    }
                    else
                    {
                        ShowImageLoad();
                    }
                });
            }
        }

        private void ShowImageSearch()
        {
            TryExecute(async () =>
            {
                var request = new StreamImageRequest();

                var xnCode = StaticSettings.User.XNCode;

                if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                {
                    xnCode = Settings.CurrentCompany.XNCode;
                }
                request.xnCode = xnCode;
                request.VehiclePlates = CarSearch;

                var response = await _streamCameraService.GetListCaptureImage(request);
                PageIndex = 1;
                IsMaxLoadMore = true;

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

        private void ShowImageLoad()
        {
            TryExecute(async () =>
            {
                var request = new StreamImageRequest();
                if (mVehicleString != null && mVehicleString.Count > 0)
                {
                    var lst = GetListPage(mVehicleString, PageIndex, PageCount);
                    if (lst != null && lst.Count > 0)
                    {
                        var xnCode = StaticSettings.User.XNCode;

                        if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                        {
                            xnCode = Settings.CurrentCompany.XNCode;
                        }
                        request.xnCode = xnCode;

                        request.VehiclePlates = string.Join(",", lst);

                        IsMaxLoadMore = false;
                    }
                    else
                    {
                        IsMaxLoadMore = true;
                    }
                }

                var response = await _streamCameraService.GetListCaptureImage(request);

                if (response != null && response.Count > 0)
                {
                    var lst = ListGroup.ToList();

                    lst.AddRange(response);

                    ListGroup = lst.ToObservableCollection();
                }
            });
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
