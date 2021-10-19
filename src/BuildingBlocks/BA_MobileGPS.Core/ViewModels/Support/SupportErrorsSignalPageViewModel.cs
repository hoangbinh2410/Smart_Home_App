using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    internal class SupportErrorsSignalPageViewModel : ViewModelBase
    {
        #region Property

        private int _selectedIndex =0;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private ObservableCollection<RotatorModel> _pageCollection = new ObservableCollection<RotatorModel>();

        public ObservableCollection<RotatorModel> PageCollection
        {
            get { return _pageCollection; }
            set { _pageCollection = value; }
        }

        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }
        private IMessageSuportService _iMessageSuportService;
        INavigationService _navigationService;
        public SupportErrorsSignalPageViewModel(INavigationService navigationService, IMessageSuportService iMessageSuportService)
            : base(navigationService)
        {
            Xamarin.Forms.MessagingCenter.Subscribe<RotatorModel>(this, "NavigationPage", (sender) =>
            {
                switch(SelectedIndex)
                {
                    case 0:
                        SelectedIndex = 1;
                        break;
                    case 1:
                        SelectedIndex = 2;
                        break;
                    case 2:
                        SelectedIndex = 0;
                        SafeExecute(async () =>
                        {
                            await NavigationService.NavigateAsync("FeedbackErrorsSignalPage");
                        });
                        break;
                    default:
                        SelectedIndex = 0;
                        break;
                }
            });
            Title = MobileResource.SupportClient_Label_Title;
            _navigationService = navigationService;
            _iMessageSuportService = iMessageSuportService;
            BackPageCommand = new DelegateCommand(BackPageClicked);
        }

        #endregion Contructor

        #region PrivateMethod

        private void BackPageClicked()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportClientPage");
            });
        }

        private void GetCollectionPage(SupportCategoryRespone obj)
        {
            SafeExecute(async () =>
            {
                List<MessageSupportRespone> items = await _iMessageSuportService.GetMessagesSupport(obj.ID);
                int index = 0;
                if (items.Count>0)
                {
                    foreach (var item in items)
                    {
                        index++;
                        PageCollection.Add(new RotatorModel(_navigationService, index.ToString(), item.Questions, "Có", "Không", item.Guides));
                    }    
                }    
            });
        }
        #endregion PrivateMethod

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone obj)
            {
                GetCollectionPage(obj);
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
    }
}