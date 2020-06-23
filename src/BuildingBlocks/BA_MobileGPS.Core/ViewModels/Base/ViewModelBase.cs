using BA_MobileGPS.Entities;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using Shiny.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.ViewModels.Base
{
    public class ViewModelBase : BindableBase, INavigationAware, IInitialize, IInitializeAsync, IDestructible, IApplicationLifecycleAware, IDisposable
    {
        public INavigationService NavigationService { get; }
        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        public virtual void Destroy()
        {
            
        }

        public virtual void Dispose()
        {
            
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            
        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return  Task.Run(()=> { });
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        public virtual void OnResume()
        {
            
        }

        public virtual void OnSleep()
        {
            
        }

        private LoginResponse userInfo;

        public LoginResponse UserInfo
        {
            get
            {
                if (StaticSettings.User != null)
                {
                    return StaticSettings.User;
                }
                return userInfo;
            }
            set
            {
                StaticSettings.User = value;
                SetProperty(ref userInfo, value);
               
            }
        }
    }
}
