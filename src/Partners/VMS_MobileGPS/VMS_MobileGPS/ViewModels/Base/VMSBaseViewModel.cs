using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Utilities;

using Prism;
using Prism.AppModel;
using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class VMSBaseViewModel : BindableBase, IInitialize, IInitializeAsync, INavigationAware, IDestructible, IApplicationLifecycleAware, IDisposable
    {
        public INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialog { get; private set; }

        public PageMode ViewMode { get; set; } = PageMode.View;

        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        private string title;
        public virtual string Title { get => title; set => SetProperty(ref title, value); }

        private bool isBusy = false;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        public ICommand ClosePageCommand { get; private set; }

        public VMSBaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            PageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            ClosePageCommand = new Command(ClosePage);
        }

        ~VMSBaseViewModel()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnected));
        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public void Destroy()
        {
            OnDestroy();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
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

        public virtual void Dispose()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnSleep()
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

        protected async void SafeExecute(Func<Task> func, Action onException = null)
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
                onException?.Invoke();
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

        protected Task RunOnBackground(Action action, Action onComplete = null, Action<Exception> onError = null, Action finalAction = null)
        {
            return Task.Run(action).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    onComplete?.Invoke();
                }
                else if (task.IsFaulted)
                {
                    onError?.Invoke(task.Exception);
                }

                finalAction?.Invoke();
            }));
        }

        protected Task RunOnBackground<T>(Func<Task<T>> action, Action<T> onComplete = null, Action<Exception> onError = null, Action finalAction = null)
        {
            return Task.Run(action).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    onComplete?.Invoke(task.Result);
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception);
                    onError?.Invoke(task.Exception);
                }

                finalAction?.Invoke();
            }));
        }

        public void ClosePage()
        {
            SafeExecute(async () =>
            {
               var a = await NavigationService.GoBackAsync(useModalNavigation: true);
            });
        }

        protected TControl GetControl<TControl>(string control)
        {
            return PageUtilities.GetCurrentPage(Application.Current.MainPage).FindByName<TControl>(control);
        }

        public void SetFocus(string control)
        {
            TryExecute(() => PageUtilities.GetCurrentPage(Application.Current.MainPage).FindByName<VisualElement>(control)?.Focus());
        }
    }
}