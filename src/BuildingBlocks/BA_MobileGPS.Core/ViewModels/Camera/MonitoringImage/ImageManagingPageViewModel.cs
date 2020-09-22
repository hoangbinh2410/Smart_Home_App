using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using BA_MobileGPS.Utilities;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageManagingPageViewModel : ViewModelBase
    {
        public ICommand TapItemsCommand { get; set; }

        public ICommand TabCommandDetail { get; set; }
        

        public ImageManagingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SpanCount = 1;
            ListHeght = 1;
            TapItemsCommand = new DelegateCommand<object>(TapItems);
            TabCommandDetail = new DelegateCommand(TabDetail);
        }

        private void TabDetail()
        {
            //chuyên trang detail
        }

        private void TapItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
          
            try
            {
                // truyền key xử lý ở đây
                var Name = ((ImageSourceModel)listview.ItemData).Name;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            
        }

        private int spanCount;
        public int SpanCount { get => spanCount; set => SetProperty(ref spanCount, value); }

        private int listHeght;
        public int ListHeght { get => listHeght; set => SetProperty(ref listHeght, value); }

        private ObservableCollection<ImageSourceModel> listImage;

        public ObservableCollection<ImageSourceModel> ListImage
        {
            get => listImage;
            set
            {
                SetProperty(ref listImage, value);
                RaisePropertyChanged();
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            // Số lượng xem gần nhất trả về ví dụ 10
            var listcount = 10;

            var view = GetImageLastView(listcount);

            SpanCount = view[0]; // số lượng xếp

            ListHeght = view[2]; // gán chiều cao cho listview

            ListImage = new ObservableCollection<ImageSourceModel>();
            for (int i = 0; i < view[1]; i++)
            {
                ListImage.Add(new ImageSourceModel
                {
                    Name = "30K123" + i
                });
            }
        }

        public int[] GetImageLastView(int count)
        {
            var respone = new int[3];
            var width = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
            if (width > 1080)
            {
                respone[0] = 5;
                respone[1] = 10 > count ? count : 10;
                respone[2] = count > 5 ? 80 : 40;
            }
            else if(width <= 1080 && width > 768)
            {
                respone[0] = 4;
                respone[1] = 8 > count ? count : 8;
                respone[2] = count > 4 ? 80 : 40;
            }
            else // < 768
            {
                respone[0] = 3;
                respone[1] = 6 > count ? count : 6;
                respone[2] = count > 3 ? 80 : 40;
            }
            return respone;
        }

    }

    public class ImageSourceModel
    {
        public string Name { get; set; }
    }
}
