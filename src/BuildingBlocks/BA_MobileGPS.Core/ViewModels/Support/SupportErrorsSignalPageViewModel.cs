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

        private String _lbTabIndex;

        public String LbTabIndex
        {
            get { return _lbTabIndex; }
            set { _lbTabIndex = value; }
        }

        private String _lbQuestions;

        public String LbQuestions
        {
            get { return _lbQuestions; }
            set { _lbQuestions = value; }
        }

        private String _textBtnYes;

        public String TextBtnYes
        {
            get { return _textBtnYes; }
            set { _textBtnYes = value; }
        }

        private String _textBtnNo;

        public String TextBtnNo
        {
            get { return _textBtnNo; }
            set { _textBtnNo = value; }
        }

        private String _lbTextPage;

        public String LbTextPage
        {
            get { return _lbTextPage; }
            set { _lbTextPage = value; }
        }

        private bool _isVisibleYesNo;

        public bool IsVisibleYesNo
        {
            get => _isVisibleYesNo;
            set => SetProperty(ref _isVisibleYesNo, value);
        }

        private bool _isShowIconBtnYes;

        public bool ISShowIconBtnYes
        {
            get => _isShowIconBtnYes;
            set => SetProperty(ref _isShowIconBtnYes, value);
        }

        private Color _backgroundColorBtnYes = Color.White;

        public Color BackgroundColorBtnYes
        {
            get => _backgroundColorBtnYes;
            set => SetProperty(ref _backgroundColorBtnYes, value);
        }

        private Color _textColorBtnYes = Color.DarkBlue;

        public Color TextColorBtnYes
        {
            get => _textColorBtnYes;
            set => SetProperty(ref _textColorBtnYes, value);
        }

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        private ObservableCollection<SupportErrorsSignalPageViewModel> _pageCollection = new ObservableCollection<SupportErrorsSignalPageViewModel>();

        public ObservableCollection<SupportErrorsSignalPageViewModel> PageCollection
        {
            get { return _pageCollection; }
            set { _pageCollection = value; }
        }

        #endregion Property

        #region Contructor

        public ICommand BackPageCommand { get; private set; }
        public ICommand SfButtonYesCommand { get; private set; }
        public ICommand SfButtonNoCommand { get; private set; }
        private IMessageSuportService _iMessageSuportService;
        private INavigationService _navigationService;
        public SupportErrorsSignalPageViewModel(INavigationService navigationService, IMessageSuportService iMessageSuportService)
            : base(navigationService)
        { 
            Title = MobileResource.SupportClient_Label_Title;
            _navigationService = navigationService;
            _iMessageSuportService = iMessageSuportService;
            BackPageCommand = new DelegateCommand(BackPageClicked);
            Xamarin.Forms.MessagingCenter.Subscribe<SupportErrorsSignalPageViewModel>(this, "SelectedIndex", (sender) =>
            {
                switch (SelectedIndex)
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
        }
        public SupportErrorsSignalPageViewModel(INavigationService navigationService, string lbTabIndex, string lbQuestions, string textBtnYes, string textBtnNo, string lbTextPage)
           : base(navigationService)
        {
            LbTabIndex = lbTabIndex;
            LbQuestions = lbQuestions;
            TextBtnYes = textBtnYes;
            TextBtnNo = textBtnNo;
            LbTextPage = lbTextPage;
            IsVisibleYesNo = false;
            SfButtonYesCommand = new DelegateCommand(SfButtonYesClicked);
            SfButtonNoCommand = new DelegateCommand(SfButtonNoClicked);
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
        private void SfButtonYesClicked()
        {
            IsVisibleYesNo = true;
            BackgroundColorBtnYes = Color.DeepSkyBlue;
            ISShowIconBtnYes = true;
            TextColorBtnYes = Color.White;
        }

        private void SfButtonNoClicked()
        {
            IsVisibleYesNo = false;
            BackgroundColorBtnYes = Color.White;
            ISShowIconBtnYes = false;
            TextColorBtnYes = Color.DarkBlue;
            Xamarin.Forms.MessagingCenter.Send<SupportErrorsSignalPageViewModel>(this, "SelectedIndex");
        }
        private void GetCollectionPage(SupportCategoryRespone obj)
        {
            SafeExecute(async () =>
            {
                List<MessageSupportRespone> items = await _iMessageSuportService.GetMessagesSupport(obj.ID);
                int index = 0;
                if (items != null && items.Count>0)
                {
                    foreach (var item in items)
                    {
                        index++;
                        if(item.Questions.Trim() == "Quý khách đã thực hiện rút nguồn và cắm lại ?")
                        {
                            PageCollection.Add(new SupportErrorsSignalPageViewModel(_navigationService, index.ToString(), item.Questions, MobileResource.SupportClient_Text_Unfinished, MobileResource.SupportClient_Text_Accomplished, item.Guides));
                        }   
                        else
                        {
                            PageCollection.Add(new SupportErrorsSignalPageViewModel(_navigationService, index.ToString(), item.Questions, MobileResource.SupportClient_Text_Yes, MobileResource.SupportClient_Text_No, item.Guides));
                        }    
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