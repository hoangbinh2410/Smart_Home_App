using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
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

        #endregion Property

        #region Contructor

        public ICommand SfButtonYesCommand { get; private set; }
        public ICommand SfButtonNoCommand { get; private set; }

        public RotatorModel(INavigationService navigationService, string lbTabIndex, string lbQuestions, string textBtnYes, string textBtnNo, string lbTextPage)
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
