using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.AppModel;
using Prism.Commands;
using Prism.Common;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public abstract class ViewModelBase : ExtendedBindableObject, INavigationAware, IInitialize, IInitializeAsync, IDestructible, IApplicationLifecycleAware, IDisposable
    {
        protected INavigationService NavigationService { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }
        protected IDisplayMessage DisplayMessage { get; private set; }
        protected IPageDialogService PageDialog { get; private set; }

        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        private string title;
        public virtual string Title { get => title; set => SetProperty(ref title, value); }

        private bool isBusy = false;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        private bool hasData = true;
        public bool HasData { get => hasData; set => SetProperty(ref hasData, value); }

        public bool isNotFound;
        public bool IsNotFound { get => isNotFound; set => SetProperty(ref isNotFound, value); }

        public int[] _vehicleGroups;
        public int[] VehicleGroups { get => _vehicleGroups; set => SetProperty(ref _vehicleGroups, value); }

        public VehicleStatusGroup _vehicleStatusSelected = VehicleStatusGroup.All;
        public VehicleStatusGroup VehicleStatusSelected { get => _vehicleStatusSelected; set => SetProperty(ref _vehicleStatusSelected, value); }
        public List<VehicleOnline> ListVehicleStatus { get; set; }

        public DelegateCommand SelectCompanyCommand { get; private set; }

        public DelegateCommand SelectVehicleCommand { get; private set; }

        public DelegateCommand SelectVehicleRouterCommand { get; private set; }

        public DelegateCommand SelectVehicleGroupCommand { get; private set; }

        public ViewModelBase(INavigationService navigationService)
        {
            if (navigationService == null)
            {
                NavigationService = PrismApplicationBase.Current.Container.Resolve<INavigationService>();
            }
            else NavigationService = navigationService;

            EventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            PageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            DisplayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();

            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;

            SelectCompanyCommand = new DelegateCommand(SelectCompany);
            SelectVehicleCommand = new DelegateCommand(SelectVehicle);
            SelectVehicleRouterCommand = new DelegateCommand(SelectVehicleRouter);
            SelectVehicleGroupCommand = new DelegateCommand(SelectVehicleGroup);
            PushToAleartPageCommand = new DelegateCommand(PushToAlertPage);
            CallHotLineCommand = new DelegateCommand(CallHotLine);
        }

        ~ViewModelBase()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }

        public virtual async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnected));

            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                // await NavigationService.NavigateAsync("NetworkPage");
                await PopupNavigation.Instance.PushAsync(new NetworkPage());
            }
            else
            {
                //await NavigationService.GoBackAsync();
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public void Destroy()
        {
            OnDestroy();

            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        private bool viewHasAppeared;
        public bool ViewHasAppeared { get => viewHasAppeared; set => SetProperty(ref viewHasAppeared, value); }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!ViewHasAppeared)
            {
                OnPageAppearingFirstTime();

                ViewHasAppeared = true;
            }
        }

        public virtual void OnPageAppearingFirstTime()
        {
        }

        public virtual void OnPushed()
        {
        }

        public virtual void OnSleep()
        {
        }

        public virtual void OnResume()
        {
            if (IsConnected)
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    });
                }
            }
        }

        public virtual void Dispose()
        {
        }

        protected void SafeExecute(Action action)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async void SafeExecute(Func<Task> func)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await func?.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void TryExecute(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected async void TryExecute(Func<Task> func)
        {
            try
            {
                await func?.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected Task RunOnBackground(Action action, Action onComplete = null, Action<Exception> onError = null, Action finalAction = null, CancellationTokenSource cts = null, bool showLoading = false)
        {
            if (showLoading)
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("");

            return (cts != null ? Task.Run(action, cts.Token) : Task.Run(action)).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion && !task.IsCanceled && (cts == null ? true : !cts.IsCancellationRequested))
                {
                    onComplete?.Invoke();
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception);
                    onError?.Invoke(task.Exception);
                }

                if (showLoading)
                    Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();

                finalAction?.Invoke();
            }));
        }

        protected Task RunOnBackground<T>(Func<Task<T>> action, Action<T> onComplete = null, Action<Exception> onError = null, Action finalAction = null, CancellationTokenSource cts = null, bool showLoading = false)
        {
            if (showLoading)
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("");

            return (cts != null ? Task.Run(action, cts.Token) : Task.Run(action)).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion && !task.IsCanceled && (cts == null ? true : !cts.IsCancellationRequested))
                {
                    onComplete?.Invoke(task.Result);
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception);
                    onError?.Invoke(task.Exception);
                }

                if (showLoading)
                    Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();

                finalAction?.Invoke();
            }));
        }

        public ICommand ClosePageCommand
        {
            get
            {
                return new Command(() =>
                {
                    SafeExecute(async () =>
                    {
                       var res = await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
                    });
                });
            }
        }

        private void SelectCompany()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/CompanyLookUp", null, useModalNavigation: true, true);
            });
        }

        private void SelectVehicle()
        {
            SafeExecute(async () =>
            {
                var lstvehicle = new List<VehicleOnline>();
                if (VehicleStatusSelected != VehicleStatusGroup.All || (ListVehicleStatus != null && ListVehicleStatus.Count > 0) || (VehicleGroups != null && VehicleGroups.Length > 0))
                {
                    lstvehicle = ListVehicleStatus;
                }
                else
                {
                    if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                    {
                        lstvehicle = StaticSettings.ListVehilceOnline.Where(x => x.MessageId != 65 && x.MessageId != 254 && x.MessageId != 128).ToList();
                    }
                }
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleOnline },
                            {  ParameterKey.VehicleGroupsSelected, VehicleGroups},
                            {  ParameterKey.VehicleStatusSelected, lstvehicle}
                        });
            });
        }

        private void SelectVehicleRouter()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", animated: true, useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleRoute },
                            {  ParameterKey.VehicleGroupsSelected, VehicleGroups},
                            {  ParameterKey.VehicleStatusSelected, ListVehicleStatus}
                        });
            });
        }

        private void SelectVehicleGroup()
        {
            SafeExecute(async () =>
            {
                var navigationPara = new NavigationParameters
                    {
                        { ParameterKey.VehicleGroupsSelected, VehicleGroups }
                    };

                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleGroupLookUp", navigationPara, animated: true, useModalNavigation: true);
            });
        }

        private void PushToAlertPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/AlertOnlinePage", null, animated: true, useModalNavigation: true);
            });
        }

        private void CallHotLine()
        {
            if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
            {
                PhoneDialer.Open(MobileSettingHelper.HotlineGps);
            }
        }

        public ICommand PushToAleartPageCommand { get; }

        public ICommand PushToNoticePageCommand
        {
            get
            {
                return new Command(() =>
                {
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("NavigationPage/NotificationPage", null, useModalNavigation: true, true);
                    });
                });
            }
        }

        public ICommand CallHotLineCommand { get; }

        private LoginResponse userInfo;

        public LoginResponse UserInfo
        {
            get
            {
                if (StaticSettings.User != null)
                {
                    userInfo = StaticSettings.User;
                }
                return userInfo;
            }
            set => SetProperty(ref userInfo, value);
        }

        public int CurrentComanyID
        {
            get
            {
                var currentCompany = Settings.CurrentCompany;

                if (currentCompany != null && StaticSettings.ListCompany != null && StaticSettings.ListCompany.Exists(c => c.FK_CompanyID == currentCompany.FK_CompanyID))
                    return currentCompany.FK_CompanyID;
                else
                    return UserInfo.CompanyId;
            }
        }

        public virtual bool CheckPermision(int PermissionKey)
        {
            return UserInfo.Permissions.IndexOf(PermissionKey) != -1;
        }

        public bool CheckPermision(List<PermissionKeyNames> PermissionKey)
        {
            List<int> userPermissionList = PermissionKey.Select(x => (int)x).ToList();

            var rerult = ((userPermissionList != null) && (userPermissionList != null)
                && (!userPermissionList.Any(x => !UserInfo.Permissions.Contains(x)) || userPermissionList.Contains(0)));

            return rerult;
        }

        protected TControl GetControl<TControl>(string control)
        {
            return PageUtilities.GetCurrentPage(Application.Current.MainPage).FindByName<TControl>(control);
        }

        public void SetFocus(string control)
        {
            TryExecute(() => PageUtilities.GetCurrentPage(Application.Current.MainPage).FindByName<VisualElement>(control)?.Focus());
        }

        public virtual void UpdateCombobox(ComboboxResponse param)
        {
        }

        public void GoBack(bool isModal)
        {
            TryExecute(async () => await NavigationService.GoBackAsync(null, useModalNavigation: isModal, isModal));
        }

        public async void Logout()
        {
            StaticSettings.ClearStaticSettings();
            GlobalResources.Current.TotalAlert = 0;
            var navigationPara = new NavigationParameters();
            navigationPara.Add(ParameterKey.Logout, true);
            await NavigationService.NavigateAsync("/LoginPage", navigationPara);
        }
    }
}