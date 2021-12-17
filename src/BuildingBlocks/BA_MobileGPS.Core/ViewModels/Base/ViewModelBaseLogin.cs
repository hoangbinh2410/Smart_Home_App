using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.AppModel;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public abstract class ViewModelBaseLogin : ExtendedBindableObject, INavigationAware, IInitialize, IInitializeAsync, IDestructible, IApplicationLifecycleAware, IDisposable
    {
        protected INavigationService NavigationService { get; private set; }
        protected IDisplayMessage DisplayMessage { get; private set; }
        protected IPageDialogService PageDialog { get; private set; }
        public ICommand CallHotLineCommand { get; }

        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        private bool isBusy = false;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        public ViewModelBaseLogin(INavigationService navigationService)
        {
            if (navigationService == null)
            {
                NavigationService = PrismApplicationBase.Current.Container.Resolve<INavigationService>();
            }
            else NavigationService = navigationService;
            DisplayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();
            PageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            CallHotLineCommand = new DelegateCommand(CallHotLine);
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        ~ViewModelBaseLogin()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }

        public virtual void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnected));
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
                        await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
                    });
                });
            }
        }

        private void CallHotLine()
        {
            try
            {
                var phoneNumber = MobileSettingHelper.HotlineGps;
                if (GlobalResources.Current.PartnerConfig != null && !string.IsNullOrEmpty(GlobalResources.Current.PartnerConfig.Email))
                {
                    phoneNumber = GlobalResources.Current.PartnerConfig.Hotline;
                }
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    PhoneDialer.Open(phoneNumber);
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
                {
                    PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                }
            }
        }
    }
}