﻿using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using Rg.Plugins.Popup.Pages;
using Syncfusion.XForms.Buttons;
using System.Linq;

namespace BA_MobileGPS.Core.Views
{
    public partial class UploadVideoPage : PopupPage
    {
        public UploadVideoPage()
        {
            InitializeComponent();
            checkAll.StateChanged += CheckAll_StateChanged;
        }

        private void CheckAll_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            foreach (var item in lstVideoUpload.Children)
            {
                if (item.FindByName("cboUploadVideo") is SfCheckBox cboUploadVideo && item.BindingContext is VideoUploadTimeInfo video)
                {
                    cboUploadVideo.StateChanged -= cboUploadVideo_StateChanged;
                    video.IsSelected = checkAll.IsChecked.Value;
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