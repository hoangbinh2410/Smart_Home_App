using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Input;

using System.Collections.ObjectModel;
using BA_MobileGPS.Utilities;
using System.Reflection;
using BA_MobileGPS.Entities;
using PanCardView.Extensions;
using System.Reactive.Linq;
using Syncfusion.Data.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImageManagingPageViewModel : ViewModelBase
    {

        private readonly IStreamCameraService _streamCameraService;

        public ICommand TapItemsCommand { get; set; }

        public ICommand TabCommandDetail { get; set; }
        

        public ImageManagingPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            SpanCount = 1;
            ListHeight = 1;
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

        private int listHeight;
        public int ListHeight { get => listHeight; set => SetProperty(ref listHeight, value); }

        private ObservableCollection<ImageSourceModel> listLastView;

        public ObservableCollection<ImageSourceModel> ListLastView { get => listLastView; set => SetProperty(ref listLastView, value); }

        private ObservableCollection<CaptureImageGroup> listGroup;

        public ObservableCollection<CaptureImageGroup> ListGroup { get => listGroup; set => SetProperty(ref listGroup, value); }

        private ObservableCollection<CaptureImageData> listImage;

        public ObservableCollection<CaptureImageData> ListImage { get => listImage; set => SetProperty(ref listImage, value); }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            ShowLastView();

            ShowImage();
        }

        private void ShowImage()
        {
            using (new HUDService())
            {
                TryExecute(async () =>
                {
                    var request = new StreamImageRequest()
                    {
                        xnCode = 1010,
                        VehiclePlates = "BACAM1409,BACAM1450"
                    };

                    var response = await _streamCameraService.GetListCaptureImage(request);

                    if (response != null && response.Count > 0)
                    {
                        ListGroup = response.ToObservableCollection();
                        
                        
                    }
                    else
                    {
                        ListGroup = new ObservableCollection<CaptureImageGroup>();
                    }
                });
            }
        }



        private void ShowLastView()
        {
            // Số lượng xem gần nhất trả về ví dụ 10
            var listcount = 10;

            var view = GetImageLastView(listcount);

            SpanCount = view[0]; // số lượng xếp

            ListHeight = view[2]; // gán chiều cao cho listview

            ListLastView = new ObservableCollection<ImageSourceModel>();
            for (int i = 0; i < view[1]; i++)
            {
                ListLastView.Add(new ImageSourceModel
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
