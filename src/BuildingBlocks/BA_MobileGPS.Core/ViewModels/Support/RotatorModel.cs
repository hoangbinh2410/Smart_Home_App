using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels.Support
{
    public class RotatorModel : ViewModelBase
    {
        #region Property

        private String _lbTabIndex;

        public String LbTabIndex
        {
            get { return _lbTabIndex; }
            set { _lbTabIndex = value; }
        }

        private String _lbTextPage;

        public String LbTextPage
        {
            get { return _lbTextPage; }
            set { _lbTextPage = value; }
        }

        private String _lbTextStep1;

        public String LbTextStep1
        {
            get { return _lbTextStep1; }
            set { _lbTextStep1 = value; }
        }

        private String _lbTextStep2;

        public String LbTextStep2
        {
            get { return _lbTextStep2; }
            set { _lbTextStep2 = value; }
        }

        private String _lbTextIf;

        public String LbTextIf
        {
            get { return _lbTextIf; }
            set { _lbTextIf = value; }
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

        #endregion Property

        #region Contructor

        public ICommand SfButtonYesCommand { get; private set; }
        public ICommand SfButtonNoCommand { get; private set; }

        public RotatorModel(INavigationService navigationService, string lbTabIndex, string lbTextPage, string textBtnYes, string textBtnNo, string lbTextStep1, string lbTextStep2, string lbTextIf)
            : base(navigationService)
        {
            LbTabIndex = lbTabIndex;
            LbTextPage = lbTextPage;
            TextBtnYes = textBtnYes;
            TextBtnNo = textBtnNo;
            LbTextStep1 = lbTextStep1;
            LbTextStep2 = lbTextStep2;
            LbTextIf = lbTextIf;
            IsVisibleYesNo = false;
            SfButtonYesCommand = new DelegateCommand(SfButtonYesClicked);
            SfButtonNoCommand = new DelegateCommand(SfButtonNoClicked);
        }

        #endregion Contructor

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
            Xamarin.Forms.MessagingCenter.Send<RotatorModel>(this, "NavigationPage");
        }
    }
}
