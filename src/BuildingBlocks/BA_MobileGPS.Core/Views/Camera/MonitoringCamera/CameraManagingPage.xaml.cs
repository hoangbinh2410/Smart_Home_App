using BA_MobileGPS.Core.Resources;
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

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraManagingPage : ContentPage, IDisposable
    {
        private double normaltHeight { get; set; }
        private CameraManagement selectedItem { get; set; }
        private IEventAggregator eventAggregator { get; } = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        public CameraManagingPage()
        {
            try
            {
                InitializeComponent();
                eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Subscribe(SwitchToNormal);
                eventAggregator.GetEvent<SwitchToFullScreenEvent>().Subscribe(SwitchToFullScreen);
                eventAggregator.GetEvent<GenerateViewEvent>().Subscribe(GenerateView);
            }
            catch (Exception ex)
            {


            }

        }
        private bool isLoaded = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            isLoaded = true;
        }

        private void GenerateView(List<CameraManagement> obj)
        {
            if (obj == null)
            {
                try
                {
                    var source = BindableLayout.GetItemsSource(parent);
                    if (source != null && source.Count() > 0)
                    {
                        var castSource = source.Cast<ChildStackSource>();
                        foreach (var child in castSource)
                        {
                            foreach (var item in child.ChildSource)
                            {
                                item.Clear();
                                item.Dispose();
                            }
                        }
                        BindableLayout.SetItemsSource(parent, null);
                        selectedItem = null;
                        normaltHeight = 0;
                    }
                }
                catch (Exception ex)
                {

                  
                }
                //Clear:
             
            }
            else
            {
                var itemSource = obj;
                var parentSource = new List<ChildStackSource>();
                if (itemSource != null && itemSource.Count > 0)
                {
                    var countSqrt = Math.Sqrt(itemSource.Count);
                    var rowNum = Convert.ToInt32(countSqrt); // so dong
                    if (rowNum < countSqrt)
                    {
                        rowNum += 1;
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

                    normaltHeight = parent.Height / rowNum;

                    foreach (var item in itemSource)
                    {
                        item.Height = normaltHeight;

                        if (item != null)
                        {
                            if (item.Data == null)
                            {
                                item.Data = new Entities.StreamStart() { Link = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4" };
                            }
                            item.SetMedia(item.Data.Link);
                        }
                    }
                    // have rowNum Item per column, each colum is a childSource
                    for (int i = 0; i < columnNum; i++)
                    {
                        var temp = new ChildStackSource();
                        temp.ChildSource = itemSource.Skip(i * rowNum).Take(rowNum).ToList();
                        parentSource.Add(temp);
                    }
                    foreach (var item in parentSource)
                    {
                        item.Width = parent.Width / parentSource.Count;
                    }
                     ((CameraManagingPageViewModel)this.BindingContext).ItemsSource = parentSource;
                    BindableLayout.SetItemsSource(parent, ((CameraManagingPageViewModel)this.BindingContext).ItemsSource);
                }
            }
        }

        private void SwitchToFullScreen()
        {
            DependencyService.Get<IScreenOrientServices>().ForceLandscape();
        }

        private void SwitchToNormal()
        {
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
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


            }
        }

        public void Dispose()
        {
            eventAggregator.GetEvent<SwitchToNormalScreenEvent>().Unsubscribe(SwitchToNormal);
            eventAggregator.GetEvent<SwitchToFullScreenEvent>().Unsubscribe(SwitchToFullScreen);
            eventAggregator.GetEvent<GenerateViewEvent>().Unsubscribe(GenerateView);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (selectedItem != null && selectedItem.IsSelected)
            {
                selectedItem.IsSelected = false;
            }
            var temp = ((TappedEventArgs)e).Parameter;
            selectedItem = (CameraManagement)temp;
            selectedItem.IsSelected = true;
            ((CameraManagingPageViewModel)this.BindingContext).SelectedItem = selectedItem;
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
                if (isLoaded)
                {
                    var source = BindableLayout.GetItemsSource(parent).Cast<ChildStackSource>();
                    foreach (var child in source)
                    {
                        child.Width = parent.Width / source.Count();
                        foreach (var item in child.ChildSource)
                        {
                            item.Height = normaltHeight;
                        }
                    }
                }             
                Grid.SetRow(playbackControl, 3);
            }
            catch (Exception ex)
            {


            }

        }

        private void OrientChangedToLanscape()
        {
            try
            {
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


            }

        }
    }

    public class SwitchToFullScreenEvent : PubSubEvent
    {

    }

    public class SwitchToNormalScreenEvent : PubSubEvent
    {

    }

    public class GenerateViewEvent : PubSubEvent<List<CameraManagement>>
    {

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
