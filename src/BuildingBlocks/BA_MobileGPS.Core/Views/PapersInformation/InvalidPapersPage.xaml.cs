using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using Prism;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class InvalidPapersPage : ContentPage, INavigationAware
    {
        private PaperCategoryTypeEnum currentPageType { get; set; } 
        private readonly INavigationService navigationService = PrismApplicationBase.Current.Container.Resolve<INavigationService>();
        private InvalidPapersPageViewModel vm { get; set; }
        public InvalidPapersPage()
        {
            InitializeComponent();
            vm = (InvalidPapersPageViewModel)BindingContext;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperCategory>(ParameterKey.PaperType) is PaperCategory paper)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    vm.SafeEx(() =>
                    {
                        papersName.Text = paper.PaperName;
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
                    });
                });
            }
            else if (parameters.ContainsKey(ParameterKey.DateResponse)
                && parameters.GetValue<PickerDateTimeResponse>(ParameterKey.DateResponse) is PickerDateTimeResponse date)
            {
                PageUtilities.OnNavigatedTo(papersChildView.Content, parameters);
            }
        }


        private void ChangePageType_Tapped(object sender, System.EventArgs e)
        {
            vm.SafeEx(async () =>
            {
                var param = new NavigationParameters()
                {
                   { ParameterKey.PaperType, new PaperModel(){ PaperType = currentPageType} }
                };
                await navigationService.NavigateAsync("NavigationPage/SelectPaperTypePage", param, true, true);
            });
        }
    }
}
