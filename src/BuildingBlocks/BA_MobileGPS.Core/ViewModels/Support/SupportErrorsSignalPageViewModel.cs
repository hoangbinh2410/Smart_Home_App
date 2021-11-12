using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    internal class SupportErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property

        private bool _isPageCollection;
        public bool IsPageCollection
        {
            get => _isPageCollection;
            set => SetProperty(ref _isPageCollection, value);
        }

        private int _position = 0;
        public int Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private ObservableCollection<MessageSupportRespone> _pageCarouselData = new ObservableCollection<MessageSupportRespone>();
        public ObservableCollection<MessageSupportRespone> PageCarouselData
        {
            get { return _pageCarouselData; }
            set { SetProperty(ref _pageCarouselData, value); }
        }

        private Vehicle _vehicle = new Vehicle();
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private List<VehicleOnline> _listvehicle = new List<VehicleOnline>();
        public List<VehicleOnline> ListVehicle
        {
            get => _listvehicle;
            set => SetProperty(ref _listvehicle, value);
        }

        private SupportCategoryRespone _objSupport = new SupportCategoryRespone();
        public SupportCategoryRespone ObjSupport
        {
            get => _objSupport;
            set => SetProperty(ref _objSupport, value);
        }


        private MessageSupportRespone currentSelected;

        public MessageSupportRespone CurrentSelected
        {
            get => currentSelected;
            set => SetProperty(ref currentSelected, value);
        }
        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }
        public ICommand SfButtonYesCommand { get; private set; }
        public ICommand SfButtonNoCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;
        private INavigationService _navigationService;

        public SupportErrorsSignalPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            _navigationService = navigationService;
            _iSupportCategoryService = iSupportCategoryService;
            SfButtonYesCommand = new DelegateCommand<MessageSupportRespone>(SfButtonYesClicked);
            SfButtonNoCommand = new DelegateCommand<MessageSupportRespone>(SfButtonNoClicked);
            BackPageCommand = new DelegateCommand(BackPageClicked);
        }

       

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone objSupport)
                {
                    ObjSupport = objSupport;
                    GetCollectionPage(objSupport);
                }
                if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle && !string.IsNullOrEmpty(vehicle.VehiclePlate))
                {
                    Vehicle = vehicle;
                }
                else if (parameters.ContainsKey("ListVehicleSupport") && parameters.GetValue<List<VehicleOnline>>("ListVehicleSupport") is List<VehicleOnline> listvehicle)
                {
                    ListVehicle = listvehicle;
                }
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        #region PrivateMethod

        private void BackPageClicked()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackToRootAsync(null);
            });
        }

        private void GetCollectionPage(SupportCategoryRespone obj)
        {
            Task.Run(async () =>
            {
                return await _iSupportCategoryService.GetMessagesSupport(obj.ID);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var listObj = task.Result.ToObservableCollection();
                    if(listObj != null && listObj.Count>0)
                    {
                        foreach(var item in listObj)
                        {
                            if (item.OrderNo == 2 && obj.Code == "MTH")
                            {
                                item.TextBtnYes = MobileResource.SupportClient_Text_Unfinished;
                                item.TextBtnNo = MobileResource.SupportClient_Text_Accomplished;
                                item.OrderNo++;
                                item.BackgroundColorBtnYes = Color.White;
                                item.TextColorBtnYes = Color.DarkBlue;
                                item.BorderWidthBtnYes = 2;
                                PageCarouselData.Add(item);
                            }
                            else
                            {
                                item.TextBtnYes = MobileResource.SupportClient_Text_Yes;
                                item.TextBtnNo = MobileResource.SupportClient_Text_No;
                                item.OrderNo++;
                                item.BackgroundColorBtnYes = Color.White;
                                item.TextColorBtnYes = Color.DarkBlue;
                                item.BorderWidthBtnYes = 2;
                                PageCarouselData.Add(item);
                            }
                        }
                        IsPageCollection = PageCarouselData.Count == 0;
                    }    
                }
            }));
        }

        private void SfButtonYesClicked(MessageSupportRespone obj)
        {
            PageCarouselData.Where(x => x.ID == obj.ID)?.ToList()
            .Select(y => {
                y.IsVisibleYesNo = true; 
                y.BackgroundColorBtnYes = Color.DeepSkyBlue; 
                y.ISShowIconBtnYes = true; 
                y.TextColorBtnYes = Color.White; 
                y.BorderWidthBtnYes = 0; return y; 
            })?.ToList();
        }

        private void SfButtonNoClicked(MessageSupportRespone obj)
        {
            PageCarouselData.Where(x => x.ID == obj.ID)?.ToList()
            .Select(y => {
                y.IsVisibleYesNo = false;
                y.BackgroundColorBtnYes = Color.White;
                y.ISShowIconBtnYes = false;
                y.TextColorBtnYes = Color.DarkBlue;
                y.BorderWidthBtnYes = 2; return y;
            })?.ToList();
            switch (Position)
            {
                case 0:
                    if (Position == PageCarouselData.Count - 1)
                    {
                        NavigationFeedbackPage();
                        return;
                    }
                    break;

                case 1:
                    if (Position == PageCarouselData.Count - 1)
                    {
                        NavigationFeedbackPage();
                        return;
                    }
                    break;

                case 2:
                    if (Position == PageCarouselData.Count - 1)
                    {
                        NavigationFeedbackPage();
                        return;
                    }
                    break;

                default:
                    break;
            }
            Position++;
        }

        private void NavigationFeedbackPage()
        {
            Position = 0;
            var parameters = new NavigationParameters
            {
                { "Support", _objSupport },
                { ParameterKey.VehicleRoute, Vehicle },
                 {"ListVehicleSupport", ListVehicle}
            };
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("MessageSuportPage", parameters);
            });
        }

        #endregion PrivateMethod
    }
}