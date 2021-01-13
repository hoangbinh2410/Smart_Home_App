using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using Prism;
using Prism.Common;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class InvalidPapersPage : ContentPage, INavigationAware
    {
        private PaperEnum currentPageType { get; set; } = PaperEnum.A1;
        private readonly INavigationService navigationService = PrismApplicationBase.Current.Container.Resolve<INavigationService>();
        private InvalidPapersPageViewModel vm { get; set; }
        public InvalidPapersPage()
        {
            InitializeComponent();
            papersChildView.Content = new RegistrationInfor();
            papersName.Text = "Type A1";
            vm = (InvalidPapersPageViewModel)BindingContext;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperModel>(ParameterKey.PaperType) is PaperModel vehicle)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    vm.SafeEx(() =>
                    {

                        papersName.Text = vehicle.Name;
                        currentPageType = vehicle.PaperType;
                        switch (vehicle.PaperType)
                        {
                            case PaperEnum.A1:
                                papersChildView.Content = new RegistrationInfor();
                                break;
                            case PaperEnum.B1:
                                papersChildView.Content = new InsuranceInfor();
                                break;
                            case PaperEnum.C2:
                                papersChildView.Content = new CabSignInfor();
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
