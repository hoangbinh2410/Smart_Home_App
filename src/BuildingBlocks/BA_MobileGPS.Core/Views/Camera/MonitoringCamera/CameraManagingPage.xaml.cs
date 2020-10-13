using BA_MobileGPS.Core.Resources;
using Prism.Mvvm;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

using Prism.Ioc;
using Prism.Events;
using Prism.Navigation;
using System;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Collections.Generic;
using Syncfusion.ListView.XForms;
using BA_MobileGPS.Core.Models;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.Data.Extensions;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraManagingPage : ContentPage
    {
        private int currentRows { get; set; }
        private int currentColumns { get; set; }
        private double portraitHeight { get; set; }
        private IEventAggregator eventAggregator { get; } = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        public CameraManagingPage()
        {
            try
            {
                InitializeComponent();
                eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Subscribe(SwitchToNormal);
                eventAggregator.GetEvent<SwitchToFullScreenEvent>().Subscribe(SwitchToFullScreen);
            }
            catch (Exception ex)
            {


            }

        }

        private void SwitchToFullScreen()
        {
            foreach (var obj in list.ItemsSource)
            {
                var item = (CameraManagement)obj;
                if (item.IsSelected)
                {
                    item.Height = list.Height;
                }
                else
                {
                    item.Height = 0;
                }
            }
            layout.Span = 1;
        }

        private void SwitchToNormal()
        {
            foreach (var obj in list.ItemsSource)
            {
                var item = (CameraManagement)obj;
                item.Height = portraitHeight;
            }
            layout.Span = currentColumns;
        }


        private void list_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "ItemsSource")
                {
                    var listViewTempalte = (CollectionView)sender;

                    var itemSource = listViewTempalte.ItemsSource.Cast<CameraManagement>().ToList();
                    if (itemSource != null && itemSource.Count > 0)
                    {
                        var countSqrt = Math.Sqrt(itemSource.Count); // so dong
                        var rowNum = Convert.ToInt32(countSqrt);
                        if (rowNum < countSqrt)
                        {
                            rowNum +=1; 
                        }
                        var columnNum = rowNum; // so cot

                        while (columnNum > 1)
                        {
                            var maxCamInlayout = columnNum * countSqrt;
                            if (itemSource.Count >= maxCamInlayout)
                            {
                                break;
                            }
                            columnNum--;
                        }

                        layout.Span = columnNum;

                        currentColumns = columnNum;
                        currentRows = rowNum;
                        portraitHeight = list.Height / rowNum;

                        foreach (var item in itemSource)
                        {
                            item.Height = portraitHeight;
                            if (item.Data == null)
                            {
                                item.Data = new Entities.StreamStart() { Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4" };
                            }
                            item.SetMedia(item.Data.Link);
                        }
                    }                   
                 
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var previous = e.PreviousSelection?.Cast<CameraManagement>().FirstOrDefault();
                var current = e.CurrentSelection?.Cast<CameraManagement>().FirstOrDefault();
                Device.BeginInvokeOnMainThread(() => {
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

                
            }                     
        }
    }

    public class SwitchToFullScreenEvent : PubSubEvent
    {

    }

    public class SwitchToNormalScreenEvent : PubSubEvent
    {

    }
}
