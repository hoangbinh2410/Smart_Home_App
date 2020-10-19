using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using PanCardView.Extensions;
using Prism.Mvvm;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraManagingPage : ContentPage
    {
        private double normaltWidth { get; set; }
        private double normalHeight { get; set; }
        private CameraManagement selectedItem { get; set; }

        public CameraManagingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var temp = ((TappedEventArgs)e).Parameter;
                var newItemSlect = (CameraManagement)temp;
                if (newItemSlect != null &&
                    newItemSlect.Data.Channel != selectedItem?.Data?.Channel)
                {
                    SetSelectedItem(newItemSlect);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SetSelectedItem(CameraManagement newItemSlect, bool excute = false)
        {
            if (newItemSlect.CanExcute() || excute)
            {
                if (selectedItem != null && selectedItem.IsSelected)
                {
                    selectedItem.IsSelected = false;
                }

                selectedItem = newItemSlect;
                selectedItem.IsSelected = true;
                ((CameraManagingPageViewModel)this.BindingContext).SelectedItem = selectedItem;
            }
        }

        private double _width;
        private double _height;

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;
            const double sizenotallocated = -1;

            base.OnSizeAllocated(width, height);
            if (Equals(_width, width) && Equals(_height, height)) return;

            _width = width;
            _height = height;

            // ignore if the previous height was size unallocated
            if (Equals(oldWidth, sizenotallocated)) return;

            // Has the device been rotated ?
            if (!Equals(width, oldWidth))
            {
                if (width < height)
                {
                    OrientChangedToVetical();
                }
                else
                {
                    OrientChangedToLanscape();
                }
            }
        }

        private void OrientChangedToVetical()
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var safe = On<iOS>().SafeAreaInsets();
                    Padding = new Thickness(0, 0, 0, safe.Bottom);
                }
                if (parent != null)
                {
                    var source = BindableLayout.GetItemsSource(parent);
                    if (source != null)
                    {
                        foreach (var child in source)
                        {
                            if (child is ChildStackSource childStack)
                            {
                                childStack.Height = parent.Height / source.Count();
                                foreach (var item in childStack.ChildSource)
                                {
                                    item.Width = normaltWidth;
                                    item.Height = normaltWidth;
                                }
                            }
                        }
                    }
                    Grid.SetRow(playbackControl, 3);
                    ((CameraManagingPageViewModel)this.BindingContext).IsFullScreenOff = true;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void OrientChangedToLanscape()
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    Padding = new Thickness(0, 0, 0, 0);
                }
                var source = BindableLayout.GetItemsSource(parent).Cast<ChildStackSource>();

                foreach (var child in source)
                {
                    child.Height = 0;
                }

                foreach (var child in source)
                {
                    foreach (var item in child.ChildSource)
                    {
                        if (!item.IsSelected)
                        {
                            item.Width = 0;
                            item.Height = 0;
                        }
                        else
                        {
                            item.Width = root.Width;
                            if (child.Height != root.Height)
                            {
                                child.Height = root.Height;
                            }
                        }
                    }
                }
                Grid.SetRow(playbackControl, 2);
                ((CameraManagingPageViewModel)this.BindingContext).IsFullScreenOff = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                noDataImage.IsVisible = true;
                try
                {
                    var source = BindableLayout.GetItemsSource(parent)?.Cast<ChildStackSource>();
                    if (source != null && source.Count() > 0)
                    {
                        noDataImage.IsVisible = false;
                        var firstItem = source.FirstOrDefault()?.ChildSource?.FirstOrDefault();
                        if (firstItem != null)
                        {
                            SetSelectedItem(firstItem, true); ;
                        }
                        var columnNum = 0;
                        foreach (var item in source)
                        {
                            if (item.ChildSource.Count > columnNum)
                            {
                                columnNum = item.ChildSource.Count;
                            }
                        }
                        var rowNum = source.Count();
                        normalHeight = parent.Height / rowNum;
                        normaltWidth = parent.Width / columnNum;

                        foreach (var item in source)
                        {
                            foreach (var cam in item.ChildSource)
                            {
                                cam.Height = normalHeight;
                                cam.Width = normaltWidth;
                            }
                            item.Height = normalHeight;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                }
            }
        }
    }

    public class ChildStackSource : BindableBase
    {
        public List<CameraManagement> ChildSource { get; set; }
        private double height;

        public double Height
        {
            get { return height; }
            set
            {
                SetProperty(ref height, value);
                RaisePropertyChanged();
            }
        }
    }
}