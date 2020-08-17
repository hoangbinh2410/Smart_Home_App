using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NotificationPageViewModel : ViewModelBase
    {
        private readonly INotificationService notificationService;

        public ICommand LoadMoreItemsCommand { get; set; }
        public ICommand TapMenuCommand { get; set; }

        public ICommand DeleteAllNotificationCommand { get; set; }

        public NotificationPageViewModel(INavigationService navigationService, INotificationService notificationService) : base(navigationService)
        {
            Title = MobileResource.Notification_Label_TilePage;
            PageCount = 10;
            PageIndex = 1;
            this.notificationService = notificationService;
            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);
            TapMenuCommand = new Command<ItemTappedEventArgs>(OnTappedMenu);
            DeleteAllNotificationCommand = new Command(OnDeleteAllNotification);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListNotice();
        }

        #region Property

        private ObservableCollection<NotificationRespone> _ListNotice = new ObservableCollection<NotificationRespone>();

        public ObservableCollection<NotificationRespone> ListNotice
        {
            get
            {
                return _ListNotice;
            }
            set => SetProperty(ref _ListNotice, value);
        }

        private int _pageIndex;

        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set => SetProperty(ref _pageIndex, value);
        }

        private int _pageCount;

        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set => SetProperty(ref _pageCount, value);
        }

        private bool _isMaxLoadMore;

        public bool IsMaxLoadMore
        {
            get
            {
                return _isMaxLoadMore;
            }
            set => SetProperty(ref _isMaxLoadMore, value);
        }

        #endregion Property

        #region Private Method

        private bool CanLoadMoreItems(object obj)
        {
            if (ListNotice.Count < PageIndex * PageCount || IsMaxLoadMore)
                return false;
            return true;
        }

        private void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                PageIndex++;

                GetListNotice();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                listview.IsBusy = false;
                IsBusy = false;
            }
        }

        private void GetListNotice()
        {
            RunOnBackground(async () =>
            {
                return await notificationService.GetListNotification(UserInfo.UserId, PageCount, PageIndex, Settings.CurrentLanguage);
            },
                  (items) =>
                  {
                      if (items != null && items.Data != null && items.Data.Count > 0)
                      {
                          var lst = ListNotice.ToList();

                          lst.AddRange(items.Data);

                          ListNotice = lst.ToObservableCollection();
                      }
                      else
                      {
                          IsMaxLoadMore = true;
                      }
                  });
        }

        public void OnTappedMenu(ItemTappedEventArgs args)
        {
            if (!(args.ItemData is NotificationRespone item))
            {
                return;
            }
            SafeExecute(async () =>
            {
                if (item != null && !item.IsRead)
                {
                    UpdateIsReadNotification(item);
                }
                else
                {
                    await NavigationService.NavigateAsync("NotificationDetailPage", parameters: new NavigationParameters
                             {
                                 { ParameterKey.NotificationKey, item }
                            });
                }
            });
        }

        public void DeleteNotification(NotificationRespone notification)
        {
            if (notification != null)
            {
                RunOnBackground(async () =>
                {
                    return await notificationService.DeleteNotificationByUser(new NoticeDeletedByUserRequest()
                    {
                        FK_NoticeContentID = notification.PK_NoticeContentID,
                        FK_UserID = UserInfo.UserId
                    });
                },
                 (items) =>
                 {
                     if (items != null && items.Data)
                     {
                         ListNotice.Remove(notification);

                         if (ListNotice == null || ListNotice.Count == 0)
                         {
                             ListNotice = new ObservableCollection<NotificationRespone>();
                         }
                     }
                     else
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             DisplayMessage.ShowMessageInfo(MobileResource.Notification_Label_DeleteNoticeNotSuccess);
                         });
                     }
                 });
            }
        }

        public void UpdateIsReadNotification(NotificationRespone notification, bool isNavigatePage = true)
        {
            RunOnBackground(async () =>
            {
                return await notificationService.UpdateIsReadNotification(new UpdateIsReadRequest()
                {
                    fk_NoticeContentID = notification.PK_NoticeContentID,
                    userId = UserInfo.UserId
                });
            },
                 async (items) =>
                  {
                      if (items != null && items.Data)
                      {
                          foreach (var item in ListNotice)
                          {
                              if (item.PK_NoticeContentID == notification.PK_NoticeContentID)
                              {
                                  item.IsRead = true;
                              }
                          }
                          if (isNavigatePage)
                          {
                              await NavigationService.NavigateAsync("NotificationDetailPage", parameters: new NavigationParameters
                             {
                                 { ParameterKey.NotificationKey, notification }
                            });
                          }
                      }
                  });
        }

        private void OnDeleteAllNotification(object obj)
        {
            SafeExecute(async () =>
            {
                if (ListNotice.Count > 0)
                {
                    var action = await PageDialog.DisplayAlertAsync(MobileResource.Notification_Label_TilePage, MobileResource.Notification_Label_DeleteAllNotice, MobileResource.Notification_Label_DeleteAllNoticeAction, MobileResource.Common_Message_Skip);
                    if (action)
                    {
                        var list = new NoticeDeletedRangeByUserRequest();
                        list.FK_UserID = UserInfo.UserId;
                        list.FK_NoticeContentID = new List<int>();
                        foreach (var item in ListNotice)
                        {
                            list.FK_NoticeContentID.Add(item.PK_NoticeContentID);
                        }
                        DependencyService.Get<IHUDProvider>().DisplayProgress("");
                        var result = await notificationService.DeleteRangeNotificationByUser(list);

                        DependencyService.Get<IHUDProvider>().Dismiss();
                        if (result != null && result.Success && result.Data)
                        {
                            ListNotice = new ObservableCollection<NotificationRespone>();
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayMessage.ShowMessageInfo(MobileResource.Notification_Label_DeleteNoticeNotSuccess);
                            });
                        }
                    }
                }
            });
        }

        #endregion Private Method
    }
}