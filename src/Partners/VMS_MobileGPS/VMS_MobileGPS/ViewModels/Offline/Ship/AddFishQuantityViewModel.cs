using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Commands;
using Prism.Navigation;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace VMS_MobileGPS.ViewModels
{
    public class AddFishQuantityViewModel : VMSBaseViewModel
    {
        private readonly IFishShipService fishShipService;

        private long selectedID;
        private int selectedFishID;

        private List<Fish> listFish;
        public List<Fish> ListFish { get => listFish; set => SetProperty(ref listFish, value); }

        private Fish selectedFish;
        public Fish SelectedFish { get => selectedFish; set => SetProperty(ref selectedFish, value); }

        private double weight;
        public double Weight { get => weight; set => SetProperty(ref weight, value); }

        private string errorText;
        public string ErrorText { get => errorText; set => SetProperty(ref errorText, value); }

        public ICommand AcceptCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public AddFishQuantityViewModel(INavigationService navigationService,
            IFishShipService fishShipService)
            : base(navigationService)
        {
            this.fishShipService = fishShipService;

            AcceptCommand = new DelegateCommand(Accept);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters == null)
                return;

            if (parameters.ContainsKey(ParameterKey.PageMode))
            {
                ViewMode = parameters.GetValue<PageMode>(ParameterKey.PageMode);
            }

            if (parameters.TryGetValue(ParameterKey.Fish, out FishTripQuantity fishTrip))
            {
                selectedID = fishTrip.Id;
                selectedFishID = fishTrip.FK_FishID;
                Weight = fishTrip.Weight;
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            GetListFish();
        }

        private async void GetListFish()
        {
            ListFish = await fishShipService.GetListFish();

            SelectedFish = (ListFish.Find(f => f.PK_FishID == selectedFishID)) ?? ListFish.FirstOrDefault();

            //Task.Run(async () =>
            //{
            //    return await fishShipService.GetListFish();
            //}).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            //{
            //    if (task.Status == TaskStatus.RanToCompletion)
            //    {
            //        if (task.Result != null)
            //        {
            //            ListFish = task.Result;

            //            SelectedFish = ListFish.Find(f => f.PK_FishID == selectedFishID);
            //        }
            //    }
            //}));
        }

        private async void Accept()
        {
            if (SelectedFish == null)
            {
                ErrorText = "Vui lòng chọn loài cá";
                return;
            }

            if (Weight == 0)
            {
                ErrorText = "Vui lòng nhập khối lượng";
                return;
            }

            var fishTrip = new FishTripQuantity
            {
                FK_FishID = SelectedFish.PK_FishID,
                FishName = SelectedFish.Name,
                Weight = Weight
            };

            if (ViewMode == PageMode.Edit)
            {
                fishTrip.Id = selectedID;
            }

            await NavigationService.GoBackAsync(useModalNavigation: true, parameters: new NavigationParameters
            {
                { ParameterKey.Fish, fishTrip }
            });
        }

        private async void Cancel()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}