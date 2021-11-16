using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using BA_MobileGPS.Utilities.Extensions;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
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

        private MessageSupportRespone currentSelected;

        public MessageSupportRespone CurrentSelected
        {
            get => currentSelected;
            set => SetProperty(ref currentSelected, value);
        }
        private SupportCategoryRespone _objSupport = new SupportCategoryRespone();

        public SupportCategoryRespone ObjSupport
        {
            get => _objSupport;
            set => SetProperty(ref _objSupport, value);
        }

        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }
        public ICommand SelectedAnswerCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;
        private INavigationService _navigationService;

        public SupportErrorsSignalPageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService)
            : base(navigationService)
        {
            Title = MobileResource.SupportClient_Label_Title;
            _navigationService = navigationService;
            _iSupportCategoryService = iSupportCategoryService;
            SelectedAnswerCommand = new DelegateCommand<AnswerSupport>(SelectedAnswer);
            BackPageCommand = new DelegateCommand(BackPageClicked);
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
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
            if (obj.MessageSupports != null && obj.MessageSupports.Count > 0)
            {
                MapData(obj.MessageSupports, obj);
            }
            else
            {
                RunOnBackground(async () =>
                {
                    return await _iSupportCategoryService.GetMessagesSupport(obj.ID);
                }, (result) =>
                {
                    MapData(result, obj);
                });
            }
        }

        private void MapData(List<MessageSupportRespone> lstData, SupportCategoryRespone obj)
        {
            var lstSource = new List<MessageSupportRespone>();

            if (lstData != null && lstData.Count > 0)
            {
                foreach (var item in lstData)
                {
                    var model = new MessageSupportRespone();
                    model.FK_SupportCategoryID = item.FK_SupportCategoryID;
                    model.ID = item.ID;
                    model.Questions = item.Questions;
                    model.OrderNo = item.OrderNo;
                    model.Guides = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + item.Guides;
                    if (model.OrderNo == 2 && obj.Code == "MTH")
                    {
                        var lstOption = new List<AnswerSupport>()
                            {
                                new AnswerSupport()
                                {
                                    IsAnswer=true,
                                    Name = MobileResource.SupportClient_Text_Unfinished
                                },
                                new AnswerSupport()
                                {
                                    IsAnswer=false,
                                    Name = MobileResource.SupportClient_Text_Accomplished
                                }
                            };
                        model.Options = lstOption;
                    }
                    else
                    {
                        var lstOption = new List<AnswerSupport>()
                            {
                                new AnswerSupport()
                                {
                                    IsAnswer=true,
                                    Name = MobileResource.SupportClient_Text_Yes
                                },
                                new AnswerSupport()
                                {
                                    IsAnswer=false,
                                    Name = MobileResource.SupportClient_Text_No
                                }
                            };
                        model.Options = lstOption;
                    }
                    model.OrderNo = item.OrderNo + 1;
                    lstSource.Add(model);
                }
                PageCarouselData = lstSource.ToObservableCollection();
            }
        }

        private void SelectedAnswer(AnswerSupport obj)
        {
            if (obj != null)
            {
                SafeExecute(() =>
                {
                    if (CurrentSelected != null)
                    {
                        CurrentSelected.IsSelected = true;
                        foreach (var item in CurrentSelected.Options)
                        {
                            if (item.Selected)
                            {
                                item.Selected = false;
                            }
                        }
                        obj.Selected = true;
                        if (obj.Selected == obj.IsAnswer)
                        {
                            CurrentSelected.IsShowGuides = true;
                        }
                        else
                        {
                            CurrentSelected.IsShowGuides = false;
                            var index = PageCarouselData.ToList().FindIndex(CurrentSelected);
                            if (index >= PageCarouselData.Count - 1)
                            {
                                var ischeckall = PageCarouselData.FirstOrDefault(x => x.IsShowGuides == true);
                                if (ischeckall == null)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        NavigationFeedbackPage();
                                    });
                                }
                            }
                            else
                            {
                                var isSelectedAll = PageCarouselData.FirstOrDefault(x => x.IsSelected == false);
                                if (isSelectedAll != null)
                                {
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await Task.Delay(500);
                                        CurrentSelected = PageCarouselData[index + 1];
                                    });
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        NavigationFeedbackPage();
                                    });
                                }
                            }
                        }
                    }
                });
            }
        }

        private void NavigationFeedbackPage()
        {
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