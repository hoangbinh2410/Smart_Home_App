using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class FishQuantityDetailViewModel : VMSBaseViewModel
    {
        private readonly IFishShipService fishShipService;

        private int selectedPosition;

        private FishTrip fishTrip;
        public FishTrip FishTrip { get => fishTrip; set => SetProperty(ref fishTrip, value); }

        private IList<FishTripQuantity> listFish;
        public IList<FishTripQuantity> ListFish { get => listFish; set => SetProperty(ref listFish, value); }

        public double TotalWeight => ListFish.Sum(f => f.Weight);

        public ICommand GetStartTimeCommand { get; private set; }
        public ICommand GetEndTimeCommand { get; private set; }
        public ICommand ChooseStartLocationCommand { get; private set; }
        public ICommand ChooseEndLocationCommand { get; private set; }
        public ICommand AddFishCommand { get; private set; }
        public ICommand ChooseActionCommand { get; private set; }
        public ICommand DeleteFishCommand { get; private set; }
        public ICommand SaveFishTripCommand { get; private set; }

        public FishQuantityDetailViewModel(INavigationService navigationService,
            IFishShipService fishShipService)
            : base(navigationService)
        {
            this.fishShipService = fishShipService;

            GetStartTimeCommand = new DelegateCommand(GetStartTime);
            GetEndTimeCommand = new DelegateCommand(GetEndTime);
            ChooseStartLocationCommand = new DelegateCommand(ChooseStartLocation);
            ChooseEndLocationCommand = new DelegateCommand(ChooseEndLocation);
            AddFishCommand = new DelegateCommand(AddFish);
            ChooseActionCommand = new DelegateCommand<FishTripQuantity>(ChooseAction);
            DeleteFishCommand = new DelegateCommand<FishTripQuantity>(DeleteFish);
            SaveFishTripCommand = new DelegateCommand(SaveFishTrip);

            FishTrip = new FishTrip
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Imei = Settings.LastImeiVMS,
                ShipPlate = Settings.LastDeviceVMS
            };

            ListFish = new ObservableCollection<FishTripQuantity>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(ParameterKey.FishTrip))
            {
                if (parameters.TryGetValue(ParameterKey.FishTrip, out FishTrip fishTrip))
                {
                    TryExecute(() =>
                    {
                        FishTrip = fishTrip.DeepCopy();

                        ListFish = fishShipService.GetFishTripDetail(fishTrip.Id).ToObservableCollection();

                        RaisePropertyChanged(nameof(TotalWeight));

                        ViewMode = PageMode.Edit;
                    });
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.Fish, out FishTripQuantity newFishTripQuantity))
            {
                if (ListFish.SingleOrDefault(f => f.Id == newFishTripQuantity.Id) is FishTripQuantity oldFishTripQuantity)
                {
                    ListFish[ListFish.IndexOf(oldFishTripQuantity)] = newFishTripQuantity;
                }
                else
                {
                    newFishTripQuantity.Id = ListFish.Count == 0 ? 1 : ListFish.Max(f => f.Id) + 1;

                    ListFish.Add(newFishTripQuantity);
                }

                RaisePropertyChanged(nameof(TotalWeight));
            }

            if (parameters.TryGetValue(ParameterKey.Position, out Position position))
            {
                if (selectedPosition == 1)
                {
                    FishTrip.StartLatitude = position.Latitude;
                    FishTrip.StartLongitude = position.Longitude;
                }
                else if (selectedPosition == 2)
                {
                    FishTrip.EndLatitude = position.Latitude;
                    FishTrip.EndLongitude = position.Longitude;
                }
            }
        }

        private async void GetStartTime()
        {
            //FishTrip.StartTime = DateTime.Now;

            var location = await LocationHelper.GetGpsLocation();

            if (location != null)
            {
                FishTrip.StartLatitude = location.Latitude;
                FishTrip.StartLongitude = location.Longitude;
            }
        }

        private async void GetEndTime()
        {
            FishTrip.EndTime = DateTime.Now;

            var location = await LocationHelper.GetGpsLocation();

            if (location != null)
            {
                FishTrip.EndLatitude = location.Latitude;
                FishTrip.EndLongitude = location.Longitude;
            }
        }

        private void ChooseStartLocation()
        {
            SafeExecute(async () =>
            {
                selectedPosition = 1;

                await NavigationService.NavigateAsync("LocationDergeeInputPage", parameters: new NavigationParameters
                {
                    { ParameterKey.Position, new Position(FishTrip.StartLatitude, FishTrip.StartLongitude) }
                });
            });
        }

        private void ChooseEndLocation()
        {
            SafeExecute(async () =>
            {
                selectedPosition = 2;

                await NavigationService.NavigateAsync("LocationDergeeInputPage", parameters: new NavigationParameters
                {
                    { ParameterKey.Position, new Position(FishTrip.EndLatitude, FishTrip.EndLongitude) }
                });
            });
        }

        private void AddFish()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("AddFishQuantityPage");
            });
        }

        private async void ChooseAction(FishTripQuantity args)
        {
            var action = await PageDialog.DisplayActionSheetAsync(args.FishName, "Huỷ", null, null, "Sửa", "Xoá");

            if ("Sửa".Equals(action))
            {
                await NavigationService.NavigateAsync("AddFishQuantityPage", parameters: new NavigationParameters
                {
                    { ParameterKey.PageMode, PageMode.Edit },
                    { ParameterKey.Fish, args }
                }, useModalNavigation: true,true);
            }
            else if ("Xoá".Equals(action))
            {
                DeleteFish(args);

                RaisePropertyChanged(nameof(TotalWeight));
            }
        }

        private void DeleteFish(FishTripQuantity args)
        {
            if (args.LastSynchronizationDate != null)
            {
                args.IsDeleted = true;
            }
            else
            {
                ListFish.Remove(args);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(FishTrip.ShipPlate))
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Vui lòng nhập biển hiệu", "Đóng");
                return false;
            }

            if (string.IsNullOrWhiteSpace(FishTrip.Imei))
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Vui lòng nhập Imei", "Đóng");
                return false;
            }

            if (FishTrip.StartLatitude == 0 && FishTrip.StartLongitude == 0)
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Vui lòng lấy vị trí bắt đầu", "Đóng");
                return false;
            }

            if (FishTrip.EndLatitude == 0 && FishTrip.EndLongitude == 0)
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Vui lòng lấy vị trí kết thúc", "Đóng");
                return false;
            }

            if (FishTrip.StartTime > FishTrip.EndTime)
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc", "Đóng");
                return false;
            }

            if (ListFish.Count <= 0)
            {
                PageDialog.DisplayAlertAsync("Thông báo", "Vui lòng thêm loài cá", "Đóng");
                return false;
            }

            return true;
        }

        private void SaveFishTrip()
        {
            if (!ValidateInputs())
                return;

            SafeExecute(async () =>
            {
                if (ViewMode == PageMode.Edit)
                {
                    fishShipService.UpdateFishTrip(FishTrip, ListFish);
                }
                else
                {
                    fishShipService.SaveFishTrip(FishTrip, ListFish);
                }
                await NavigationService.GoBackAsync();
            });
        }
    }
}