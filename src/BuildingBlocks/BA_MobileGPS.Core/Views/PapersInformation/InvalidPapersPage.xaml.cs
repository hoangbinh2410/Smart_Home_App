using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;
using Prism.Ioc;
using System.Linq;

namespace BA_MobileGPS.Core.Views
{
    public partial class InvalidPapersPage : ContentPage, INavigationAware
    {
        private PaperCategoryTypeEnum currentPageType { get; set; }
        private readonly IPapersInforService papersInforService = Prism.PrismApplicationBase.Current.Container.Resolve<IPapersInforService>();
        public InvalidPapersPage()
        {
            InitializeComponent();
            papersName.Text = "Chọn loại giấy tờ";
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            using (new HUDService())
            {
                if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperCategory>(ParameterKey.PaperType) is PaperCategory paper)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        papersName.Text =  paper.PaperName;
                        currentPageType = (PaperCategoryTypeEnum)paper.PaperCategoryType;
                        switch (currentPageType)
                        {
                            case PaperCategoryTypeEnum.Registry:
                                papersChildView.Content = new RegistrationInfor();
                                break;

                            case PaperCategoryTypeEnum.Insurrance:
                                papersChildView.Content = new InsuranceInfor();
                                break;

                            case PaperCategoryTypeEnum.Sign:
                                papersChildView.Content = new CabSignInfor();
                                break;

                            case PaperCategoryTypeEnum.None:
                                papersChildView.Content = new Grid();
                                break;
                        }

                        if (!string.IsNullOrEmpty(entrySearch.Text))
                        {
                            var param = new NavigationParameters();
                            param.Add(ParameterKey.Vehicle, entrySearch.Text);
                            PageUtilities.OnNavigatedTo(papersChildView.Content, param);
                        }
                    });
                 
                }
                else if (parameters.ContainsKey(ParameterKey.DateResponse)
                      && parameters.GetValue<PickerDateTimeResponse>(ParameterKey.DateResponse) is PickerDateTimeResponse date)
                {
                    PageUtilities.OnNavigatedTo(papersChildView.Content, parameters);
                }
            }
        }

        private void entrySearch_TextChanged(object sender, TextChangedEventArgs e)         
        {
            if (!string.IsNullOrEmpty(entrySearch.Text) && currentPageType != PaperCategoryTypeEnum.None)
            {
                var param = new NavigationParameters();
                param.Add(ParameterKey.Vehicle, entrySearch.Text);
                PageUtilities.OnNavigatedTo(papersChildView.Content, param);
            }
        }
    }
}