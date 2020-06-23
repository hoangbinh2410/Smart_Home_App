using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private async void Login()
        {
            await SetUserPermisisonData();
            await NavigationService.NavigateAsync("MainPage");
           
        }

        public ICommand LoginCommand { get; }

        private async Task SetUserPermisisonData()
        {
            if (StaticSettings.User == null)
            {
                StaticSettings.User = new LoginResponse();
            }
            StaticSettings.User.Permissions = new List<int>() { 0, 1, 6, 61, 161 ,86,91,96,106};

             

            var respose = await RequestData();
            var contentStream = await respose.Content.ReadAsStringAsync();
            var allItems = JsonConvert.DeserializeObject<List<HomeMenuItem>>(contentStream);
            var permisisons = UserInfo.Permissions.Distinct();

            StaticSettings.ListMenu = allItems.Where(x => permisisons.Contains(x.PermissionViewID) && x.MenuItemParentID != 0).ToList();
            foreach (var item in StaticSettings.ListMenu)
            {
                item.IconMobile = "http://api.bagroup.vn" + item.IconMobile;
            }
        }


        private async Task<HttpResponseMessage> RequestData()
        {
           
            var url = "http://api.bagroup.vn/api/v2/menu/getmenubyculture?culture=vi-vn&appID=0";
            var token = "-NDA--SDiw7mxixvrPrvRWEg-HR84tV1kZlgH_Mwm1xszh3tsXpohSghJjt9KT8Ri2Nvb71gBjIQadqz--U88Bb1_3bvRIrQ6ehAgEVJ8mgRyHNK0IiRP3E9qAzTe9b739NMNx3PrMlX-W6QcoB8KlfNFD90J3_I3_24U66W1aZFXmzuc6J10gAzgr6V3opue8fhbI5a2ZLxm21NxWuM-GPMvrMfTc7aEC73B0gY8tT1GzX7a23VMh-RZExTArvFUYS2lOrl578FXWMMcJ4owIMwi8C_0zGswuC-8c-tEd8j8S7gBbjxCM_1kla0ZJlH4ujRVTp-8IOzMa_IgnCElb5U5swb7MwwMDEl80KhayyDA4KHXb62bq-7zppRj181WC6y6gAkyqlafpMdSVmQTaa4Ml5ppwm69CsgbtEsuG4HytPaLTB-xiX8965M-QrRhbkLaoHMHRvjcdFn0hhchBPs6XWAiTzbkV-9AG2sAwOmlLkUcn3DBw5ofJbTD9wXEYa1LVVxRQWU8Eirv1rsGNYw-GONBg7ijPCp7bL-0nJO98Gna2IEUOFQn3ON6Q0GlP9t8IeET8tnYsipuUAvbq_vesm_8fPdBtAHKBQiKZ9Fl4WvaAJIbKP9qtIgyMZZmNabRd3QpdTL6_bICSjXdg";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }
    }
}
