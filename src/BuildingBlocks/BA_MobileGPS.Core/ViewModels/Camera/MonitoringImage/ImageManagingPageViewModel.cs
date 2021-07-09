using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageManagingPageViewModel : ViewModelBase
    {
        private readonly IRealmBaseService<LastViewVehicleRealm, LastViewVehicleRespone> _lastViewVehicleRepository;

        private readonly IStreamCameraService _streamCameraService;

        public ICommand TapItemsCommand { get; set; }

        public ICommand TabCommandDetail { get; set; }

        public ICommand TabCommandFavorites { get; set; }

        public ICommand TapCommandListGroup { get; set; }

        public ICommand LoadMoreItemsCommand { get; set; }

        public ICommand RefeshCommand { get; set; }

        public DelegateCommand SelectVehicleImageCommand { get; private set; }

        public DelegateCommand HelpImageCommand { get; private set; }

        public ImageManagingPageViewModel(INavigationService navigationService,
            IStreamCameraService streamCameraService,
            IRealmBaseService<LastViewVehicleRealm, LastViewVehicleRespone> lastViewVehicleRepository) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            _lastViewVehicleRepository = lastViewVehicleRepository;
            SpanCount = 1;
            ListHeight = 1;
            TapItemsCommand = new DelegateCommand<object>(TapItems);
            TabCommandDetail = new DelegateCommand<object>(TabDetail);
            TabCommandFavorites = new DelegateCommand<object>(TabFavorites);
            TapCommandListGroup = new DelegateCommand<ItemTappedEventArgs>(TapListGroup);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            RefeshCommand = new DelegateCommand(RefeshImage);
            SelectVehicleImageCommand = new DelegateCommand(SelectVehicleImage);
            HelpImageCommand = new DelegateCommand(HelpImage);
            CarSearch = string.Empty;
            PageCount = 5;
            PageIndex = 0;
        }

        private void HelpImage()
        {
            SafeExecute(async () =>
            {
                await PageDialog.DisplayAlertAsync("Thông báo", "Các xe sử dụng gói cước không tích hợp tính năng xem hình ảnh sẽ không được hiển thị trên tính năng này", "Bỏ qua");
            });
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetVehicleString();
            ShowLastView();
            if (!parameters.ContainsKey(ParameterKey.Vehicle))
            {
                ShowImage();
            }

            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.TrackingVehiclesByImage,
                Type = UserBehaviorType.Start
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.TrackingVehiclesByImage,
                Type = UserBehaviorType.End
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                ValidateVehicleCamera(vehiclePlate);
                ShowImage();
            }
            else if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;
                CarSearch = string.Empty;
                GetVehicleString();
                PageIndex = 0;
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

        private bool isRefesh = false;

        public bool IsRefesh { get => isRefesh; set => SetProperty(ref isRefesh, value); }

        private ObservableCollection<CaptureImageData> listGroup = new ObservableCollection<CaptureImageData>();

        public ObservableCollection<CaptureImageData> ListGroup
        {
            get { return listGroup; }
            set
            {
                SetProperty(ref listGroup, value);
                RaisePropertyChanged();
            }
        }

        private bool isShowLastViewVehicle = true;
        public bool IsShowLastViewVehicle { get => isShowLastViewVehicle; set => SetProperty(ref isShowLastViewVehicle, value); }

        private ObservableCollection<string> listLastView;

        public ObservableCollection<string> ListLastView { get => listLastView; set => SetProperty(ref listLastView, value); }

        private List<string> mVehicleString { get; set; }

        private void ValidateVehicleCamera(Vehicle vehicle)
        {
            var listVehicleCamera = StaticSettings.ListVehilceCamera;
            if (listVehicleCamera != null)
            {
                var model = StaticSettings.ListVehilceCamera.FirstOrDefault(x => x.VehiclePlate == vehicle.VehiclePlate + "_C");
                if (model != null)
                {
                    CarSearch = model.VehiclePlate;
                }
                else
                {
                    CarSearch = vehicle.VehiclePlate;
                }
            }
            else
            {
                CarSearch = vehicle.VehiclePlate;
            }
        }

        private void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                PageIndex++;
                ShowImageLoadMore();
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
            if (mVehicleString.Count < PageIndex * PageCount || IsMaxLoadMore || IsRefesh)
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
                    await NavigationService.NavigateAsync("ImageDetailPage", parameters, useModalNavigation: false, true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void TabDetail(object obj)
        {
            //chuyên trang danh sách camera
            SafeExecute(async () =>
            {
                var vehiclecam = StaticSettings.ListVehilceCamera?.FirstOrDefault(x => x.VehiclePlate == (string)obj);
                if (vehiclecam != null)
                {
                    var vehicle = new Vehicle
                    {
                        VehicleId = vehiclecam.VehicleId,
                        VehiclePlate = vehiclecam.VehiclePlate,
                        PrivateCode = vehiclecam.VehiclePlate,
                        Imei = vehiclecam.Imei
                    };

                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.Vehicle, vehicle }
                    };

                    await NavigationService.NavigateAsync("NavigationPage/ListCameraVehicle", parameters, useModalNavigation: true, true);
                }
                else
                {
                    var vehicleOnline = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehiclePlate == (string)obj);
                    if (vehicleOnline != null)
                    {
                        var vehicle = new Vehicle
                        {
                            VehicleId = vehicleOnline.VehicleId,
                            VehiclePlate = vehicleOnline.VehiclePlate,
                            PrivateCode = vehicleOnline.PrivateCode,
                            Imei = vehicleOnline.Imei
                        };

                        var parameters = new NavigationParameters
                        {
                            { ParameterKey.Vehicle, vehicle }
                        };

                        await NavigationService.NavigateAsync("NavigationPage/ListCameraVehicle", parameters, useModalNavigation: true, true);
                    }
                }
            });
        }

        private void TabFavorites(object obj)
        {
            try
            {
                string VehiclePlate = (string)obj;
                if (Settings.FavoritesVehicleImage == string.Empty) // lần đầu
                {
                    Settings.FavoritesVehicleImage = VehiclePlate;
                }
                else
                {
                    var split = Settings.FavoritesVehicleImage.Split(',');

                    string[] temp = new string[split.Length];

                    var splitVehicle = split.Where(x => x == VehiclePlate).ToArray();

                    if (splitVehicle.Length > 0) // đã có trong list thì xóa đi
                    {
                        split = split.Where(x => x != VehiclePlate).ToArray();

                        temp = new string[split.Length];

                        for (int i = 0; i < split.Length; i++)
                        {
                            temp[i] = split[i];
                        }
                    }
                    else
                    {
                        temp = new string[split.Length + 1];

                        for (int i = 0; i < split.Length; i++)
                        {
                            temp[i] = split[i];
                        }

                        temp[split.Length] = VehiclePlate;
                    }

                    Settings.FavoritesVehicleImage = string.Join(",", temp);
                }

                //

                var lst = ListGroup.Where(x => x.VehiclePlate == VehiclePlate).ToList();
                if (lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        if (Settings.FavoritesVehicleImage.Contains(item.VehiclePlate))
                        {
                            item.IsFavorites = true;
                        }
                        else
                        {
                            item.IsFavorites = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void TapItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            try
            {
                // truyền key xử lý ở đây
                CarSearch = (string)listview.ItemData;

                ShowImageSearch();
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
                var mlistVehicle = new List<string>();
                var mlistVehicleFavorites = new List<string>();
                var lstplate = new List<string>();
                mVehicleString = new List<string>();
                var listVehicleCamera = StaticSettings.ListVehilceCamera;
                var listOnline = StaticSettings.ListVehilceOnline.Where(x => x.MessageId != 65 && x.MessageId != 254 && x.MessageId != 128).ToList();
                if (VehicleGroups != null && VehicleGroups.Length > 0)
                {
                    lstplate = listOnline.FindAll(v => v.GroupIDs.Split(',').ToList()
                        .Exists(g => VehicleGroups.Contains(Convert.ToInt32(g))))
                        .Select(x => x.VehiclePlate).ToList();
                }
                else
                {
                    lstplate = listOnline.Select(x => x.VehiclePlate).ToList();
                }

                // Lấy danh sách ưa thích
                foreach (var item in lstplate)
                {
                    string plate = item;
                    if (listVehicleCamera != null)
                    {
                        var model = StaticSettings.ListVehilceCamera.FirstOrDefault(x => x.VehiclePlate == item + "_C");
                        if (model != null)
                        {
                            plate = model.VehiclePlate;
                        }
                    }
                    if (Settings.FavoritesVehicleImage.Contains(plate))
                    {
                        mlistVehicleFavorites.Add(plate);
                    }
                    else
                    {
                        mlistVehicle.Add(plate);
                    }
                }

                if (mlistVehicleFavorites.Count > 0 && mlistVehicleFavorites != null)
                {
                    mVehicleString.AddRange(mlistVehicleFavorites);
                }
                if (mlistVehicle.Count > 0 && mlistVehicle != null)
                {
                    mVehicleString.AddRange(mlistVehicle);
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

        private void RefeshImage()
        {
            TryExecute(() =>
            {
                IsRefesh = true;
                if (CarSearch == string.Empty)
                {
                    PageIndex = 0;
                    ListGroup = new ObservableCollection<CaptureImageData>();
                    ShowImageLoad();
                    LoadMoreItemsCommand.Execute(new Syncfusion.ListView.XForms.SfListView());
                }
                else
                {
                    ShowImageSearch();
                }
                IsRefesh = false;
            });
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

                PageIndex = 0;
                IsMaxLoadMore = true;

                var response = await _streamCameraService.GetListCaptureImage(request);

                if (response != null && response.Count > 0)
                {
                    ListGroup = new ObservableCollection<CaptureImageData>(response);
                    foreach (var item in ListGroup)
                    {
                        if (Settings.FavoritesVehicleImage.Contains(item.VehiclePlate))
                        {
                            item.IsFavorites = true;
                        }
                    }
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
                if (mVehicleString != null && mVehicleString.Count > 0)
                {
                    var request = new StreamImageRequest();
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
                    }

                    var response = await _streamCameraService.GetListCaptureImage(request);

                    if (response != null && response.Count > 0)
                    {
                        foreach (var item in response)
                        {
                            if (Settings.FavoritesVehicleImage.Contains(item.VehiclePlate))
                            {
                                item.IsFavorites = true;
                            }
                        }
                        ListGroup = new ObservableCollection<CaptureImageData>(response);
                    }
                    else
                    {
                        ListGroup = new ObservableCollection<CaptureImageData>();
                    }
                }
            });
        }

        private void ShowImageLoadMore()
        {
            TryExecute(async () =>
            {
                if (mVehicleString != null && mVehicleString.Count > 0)
                {
                    var request = new StreamImageRequest();
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

                        var response = await _streamCameraService.GetListCaptureImage(request);

                        if (response != null && response.Count > 0)
                        {
                            if (ListGroup.Count > 0 && ListGroup != null)
                            {
                                foreach (var item in response)
                                {
                                    if (Settings.FavoritesVehicleImage.Contains(item.VehiclePlate))
                                    {
                                        item.IsFavorites = true;
                                    }
                                }

                                ListGroup.AddRange(response);
                            }
                            else
                            {
                                ListGroup = new ObservableCollection<CaptureImageData>(response);
                            }
                        }
                        else
                        {
                            PageIndex++;
                            ShowImageLoadMore();
                        }
                    }
                    else
                    {
                        IsMaxLoadMore = true;
                    }
                }
            });
        }

        private List<string> GetListPage(List<string> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }

        private void ShowLastView()
        {
            var userId = StaticSettings.User.UserId;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userId = Settings.CurrentCompany.UserId;
            }

            var lst = _lastViewVehicleRepository.All()?.Where(item => item.UserId == userId.ToString()).OrderByDescending(x => x.Id).ToList();

            // nếu lần đầu vào chưa có lịch sử xem gần nhất
            if (lst.Count > 0 && lst != null)
            {
                IsShowLastViewVehicle = true;

                var view = GetImageLastView(lst.Count);

                SpanCount = view[0]; // số lượng xếp.

                Settings.ShowViewVehicleImage = view[0] * 2; // số lượng show

                ListHeight = view[1]; // gán chiều cao cho listview

                ListLastView = new ObservableCollection<string>();
                for (int i = 0; i < lst.Count; i++)
                {
                    ListLastView.Add(lst[i].VehiclePlate);
                }
            }
            else
            {
                // ẩn cột đi
                IsShowLastViewVehicle = false;
            }
        }

        public int[] GetImageLastView(int count)
        {
            var respone = new int[2];
            var width = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
            if (width > 1200)
            {
                respone[0] = 5;
                respone[1] = count > 5 ? 80 : 40;
            }
            else if (width <= 1200 && width > 768)
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

        private void SelectVehicleImage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", animated: true, useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleImage },
                            {  ParameterKey.VehicleGroupsSelected, VehicleGroups},
                            {  ParameterKey.VehicleStatusSelected, ListVehicleStatus}
                        });
            });
        }
    }
}