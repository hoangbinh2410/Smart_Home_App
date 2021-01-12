using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class InvalidPapersPage : ContentPage, INavigationAware
    {
        private PaperEnum currentPageType { get; set; } = PaperEnum.A1;
        private readonly INavigationService navigationService = PrismApplicationBase.Current.Container.Resolve<INavigationService>();
        public InvalidPapersPage()
        {
            InitializeComponent();
            papersChildView.Children.Add(new RegistrationInfor());
            papersName.Text = "Type A1";
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey(ParameterKey.PaperType) && parameters.GetValue<PaperModel>(ParameterKey.PaperType) is PaperModel vehicle)
            {
                papersChildView.Children.Clear();
                papersName.Text = vehicle.Name;
                currentPageType = vehicle.PaperType;
                switch (vehicle.PaperType)
                {
                    case PaperEnum.A1:
                        papersChildView.Children.Add(new RegistrationInfor());
                        break;
                    case PaperEnum.B1:
                        papersChildView.Children.Add(new InsuranceInfor());
                        break;
                    case PaperEnum.C2:
                        papersChildView.Children.Add(new CabSignInfor());
                        break;
                }
            }
        }

        private async void ChangePageType_Tapped(object sender, System.EventArgs e)
        {
            var param = new NavigationParameters()
            {
                { ParameterKey.PaperType, new PaperModel(){ PaperType = currentPageType} }
            };
           var res = await navigationService.NavigateAsync("BaseNavigationPage/SelectPaperTypePage", param, true, true);
        }
    }
}
