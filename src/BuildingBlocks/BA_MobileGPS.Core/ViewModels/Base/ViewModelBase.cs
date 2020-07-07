using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using Prism;
using Prism.AppModel;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Shiny.Caching;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using System.Threading;
using Xamarin.Forms;
using BA_MobileGPS.Utilities;
using Xamarin.Essentials;
using Prism.Services;

namespace BA_MobileGPS.Core.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IInitialize, IInitializeAsync, IDestructible, IApplicationLifecycleAware, IDisposable
    {
        protected INavigationService NavigationService { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }
        protected IDisplayMessage DisplayMessage { get; private set; }

        protected IPageDialogService PageDialog { get; private set; }
        public PageMode ViewMode { get; set; } = PageMode.View;

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

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;

            EventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            PageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            DisplayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();

            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
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
                //await PopupNavigation.Instance.PushAsync(new NetworkPage());
            }
            else
            {
                //await NavigationService.GoBackAsync();
                //if (PopupNavigation.Instance.PopupStack.Count > 0)
                //{
                //    await PopupNavigation.Instance.PopAllAsync();
                //}
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
            //if (showLoading)
            //    UserDialogs.Instance.ShowLoading();

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

                //if (showLoading)
                //    UserDialogs.Instance.HideLoading();

                finalAction?.Invoke();
            }));
        }

        protected Task RunOnBackground<T>(Func<Task<T>> action, Action<T> onComplete = null, Action<Exception> onError = null, Action finalAction = null, CancellationTokenSource cts = null, bool showLoading = false)
        {
            //if (showLoading)
            //    UserDialogs.Instance.ShowLoading();

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

                //if (showLoading)
                //    UserDialogs.Instance.HideLoading();

                finalAction?.Invoke();
            }));
        }
    }
}
