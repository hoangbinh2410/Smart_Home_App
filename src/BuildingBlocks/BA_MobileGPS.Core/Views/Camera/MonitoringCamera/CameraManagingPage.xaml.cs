﻿using BA_MobileGPS.Core.Resources;
using Prism.Mvvm;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Prism.Ioc;
using Prism.Events;
using System;
using System.Collections.Generic;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Interfaces;
using System.Collections.ObjectModel;
using Syncfusion.DataSource.Extensions;
using Xamarin.Forms.Extensions;
using BA_MobileGPS.Core.ViewModels;
using PanCardView.Extensions;
using BA_MobileGPS.Core.Helpers;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraManagingPage : ContentPage, IDisposable
    {
        private double normaltHeight { get; set; }
        private CameraManagement selectedItem { get; set; }
        public CameraManagingPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }

        }

        protected override void OnAppearing()
        {          
            base.OnAppearing();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var previous = e.PreviousSelection?.Cast<CameraManagement>().FirstOrDefault();
                var current = e.CurrentSelection?.Cast<CameraManagement>().FirstOrDefault();
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (previous != null)
                    {
                        previous.IsSelected = false;
                    }
                    if (current != null)
                    {
                        current.IsSelected = true;
                    }
                });

            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void Dispose()
        {

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
                                childStack.Width = parent.Width / source.Count();
                                foreach (var item in childStack.ChildSource)
                                {
                                    item.Height = normaltHeight;
                                }
                            }
                        }
                    }
                    Grid.SetRow(playbackControl, 3);
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
                    child.Width = 0;
                }

                foreach (var child in source)
                {

                    foreach (var item in child.ChildSource)
                    {
                        if (!item.IsSelected)
                        {
                            item.Height = 0;
                        }
                        else
                        {
                            item.Height = root.Height;
                            if (child.Width != root.Width)
                            {
                                child.Width = root.Width;
                            }

                        }
                    }
                }
                Grid.SetRow(playbackControl, 2);
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
                        var firstItem = source.FirstOrDefault()?.ChildSource?.FirstOrDefault();
                        if (firstItem != null)
                        {
                            SetSelectedItem(firstItem, true); ;
                        }
                        noDataImage.IsVisible = false;
                        var maxCount = 1;
                        foreach (var item in source)
                        {
                            if (item.ChildSource.Count > maxCount)
                            {
                                maxCount = item.ChildSource.Count;
                            }
                        }

                        foreach (var item in source)
                        {
                            foreach (var cam in item.ChildSource)
                            {
                                normaltHeight = parent.Height / maxCount;
                                cam.Height = normaltHeight;
                            }
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
        private double width;
        public double Width
        {
            get { return width; }
            set
            {
                SetProperty(ref width, value);
                RaisePropertyChanged();
            }
        }
    }
}
