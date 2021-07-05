using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Syncfusion.XForms.Buttons;
using System.Linq;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class UploadVideoPage : ContentPage
    {
        public UploadVideoPage()
        {
            InitializeComponent();
            Title = MobileResource.Camera_Title_UploadVideoToServer;
            txtNote.Text = MobileResource.Camera_Lable_NoteUploadVideoToServer;
            checkAll.Text = MobileResource.Common_Label_All;
            checkAll.StateChanged += CheckAll_StateChanged;
        }

        private void CheckAll_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            foreach (var item in lstVideoUpload.Children)
            {
                if (item.FindByName("cboUploadVideo") is SfCheckBox cboUploadVideo && item.BindingContext is VideoUploadTimeInfo video)
                {
                    cboUploadVideo.StateChanged -= cboUploadVideo_StateChanged;
                    if (video.Status == VideoUploadStatus.NotUpload || video.Status == VideoUploadStatus.UploadError)
                    {
                        video.IsSelected = checkAll.IsChecked.Value;
                    }
                    else
                    {
                        video.IsSelected = true;
                    }
                    cboUploadVideo.StateChanged += cboUploadVideo_StateChanged;
                }
            }
        }

        private void cboUploadVideo_StateChanged(object sender, StateChangedEventArgs e)
        {
            checkAll.StateChanged -= CheckAll_StateChanged;

            if (BindingContext is UploadVideoPageViewModel viewModel && viewModel.ListVideo.Count > 0)
            {
                var check = viewModel.ListVideo.All(l => l.IsSelected);
                checkAll.IsChecked = check;
            }
            else
            {
                checkAll.IsChecked = false;
            }

            checkAll.StateChanged += CheckAll_StateChanged;
        }
    }
}