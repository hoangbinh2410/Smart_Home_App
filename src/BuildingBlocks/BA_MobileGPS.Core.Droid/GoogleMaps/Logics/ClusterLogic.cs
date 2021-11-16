using Android.App;
using Android.Gms.Maps.Model;
using Android.Widget;

using BA_MobileGPS.Core.Droid.Factories;
using BA_MobileGPS.Core.Logics;

using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.Algo;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Droid
{
    public class ClusterLogic : DefaultPinLogic<ClusteredMarker, Android.Gms.Maps.GoogleMap>
    {
        protected override IList<Pin> GetItems(Map map) => (map as Map)?.ClusteredPins;

        private ClusterManager clusterManager;
        private ClusterLogicHandler clusterHandler;

        private readonly Activity context;
        private readonly IBitmapDescriptorFactory bitmapDescriptorFactory;

        private Android.Gms.Maps.Model.CameraPosition previousCameraPostion;
        private ClusterRenderer clusterRenderer;

        private readonly Dictionary<string, Pin> itemsDictionary = new Dictionary<string, Pin>();

        public Map ClusteredMap => Map;

        public ClusterLogic(Activity context, IBitmapDescriptorFactory bitmapDescriptorFactory)
        {
            this.bitmapDescriptorFactory = bitmapDescriptorFactory;
            this.context = context;
        }

        public override void Register(Android.Gms.Maps.GoogleMap oldNativeMap, Map oldMap, Android.Gms.Maps.GoogleMap newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            clusterManager = new ClusterManager(context, NativeMap) { Algorithm = GetClusterAlgorithm() };
            clusterHandler = new ClusterLogicHandler(Map, clusterManager, this);

            if (newNativeMap == null)
                return;

            ClusteredMap.OnCluster = HandleClusterRequest;

            NativeMap.CameraIdle += NativeMapOnCameraIdle;
            NativeMap.SetOnMarkerClickListener(clusterManager);
            NativeMap.SetOnInfoWindowClickListener(clusterManager);

            clusterRenderer = new ClusterRenderer(context, ClusteredMap, NativeMap, clusterManager);
            clusterManager.Renderer = clusterRenderer;

            clusterManager.SetOnClusterClickListener(clusterHandler);
            clusterManager.SetOnClusterInfoWindowClickListener(clusterHandler);
            clusterManager.SetOnClusterItemClickListener(clusterHandler);
            clusterManager.SetOnClusterItemInfoWindowClickListener(clusterHandler);
        }

        private IAlgorithm GetClusterAlgorithm()
        {
            IAlgorithm algorithm;

            switch (ClusteredMap.ClusterOptions.Algorithm)
            {
                case ClusterAlgorithm.GridBased:
                    algorithm = new PreCachingAlgorithmDecorator(new GridBasedAlgorithm());
                    break;

                case ClusterAlgorithm.VisibleNonHierarchicalDistanceBased:
                    algorithm = new NonHierarchicalViewBasedAlgorithm(context.Resources.DisplayMetrics.WidthPixels, context.Resources.DisplayMetrics.HeightPixels);
                    break;

                case ClusterAlgorithm.NonHierarchicalDistanceBased:
                    algorithm = new PreCachingAlgorithmDecorator(new NonHierarchicalDistanceBasedAlgorithm());
                    break;

                default:
                    algorithm = new PreCachingAlgorithmDecorator(new NonHierarchicalDistanceBasedAlgorithm());
                    break;
            }

            return algorithm;
        }

        private void NativeMapOnCameraIdle(object sender, EventArgs e)
        {
            if (previousCameraPostion == null || Math.Abs(previousCameraPostion.Zoom - NativeMap.CameraPosition.Zoom) > 0.001)
            {
                previousCameraPostion = NativeMap.CameraPosition;
            }
        }

        public override void Unregister(Android.Gms.Maps.GoogleMap nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                NativeMap.CameraIdle -= NativeMapOnCameraIdle;
                NativeMap.SetOnMarkerClickListener(null);
                NativeMap.SetOnInfoWindowClickListener(null);

                clusterHandler.Dispose();
                clusterManager.Dispose();
            }

            base.Unregister(nativeMap, map);
        }

        protected override ClusteredMarker CreateNativeItem(Pin outerItem)
        {
            var marker = new ClusteredMarker(outerItem);

            if (outerItem.Icon != null && outerItem?.Icon?.Type != BitmapDescriptorType.View)
            {
                try
                {
                    var factory = bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
                    var nativeDescriptor = factory.ToNative(outerItem.Icon);
                    marker.Icon = nativeDescriptor;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (outerItem?.Icon?.Type == BitmapDescriptorType.View)
            {
                marker.Visible = false;
                TransformXamarinViewToAndroidBitmap(outerItem, marker);
                return marker;
            }
            else
            {
                marker.Visible = outerItem.IsVisible;
            }

            AddMarker(outerItem, marker);

            return marker;
        }

        private void AddMarker(Pin outerItem, ClusteredMarker marker)
        {
            outerItem.NativeObject = marker;
            clusterManager.AddItem(marker);
            itemsDictionary.Add(marker.Id, outerItem);
        }

        protected override ClusteredMarker DeleteNativeItem(Pin outerItem)
        {
            if (!(outerItem.NativeObject is ClusteredMarker marker))
                return null;

            clusterManager.RemoveItem(marker);
            outerItem.NativeObject = null;

            if (ReferenceEquals(Map.SelectedPin, outerItem))
                Map.SelectedPin = null;

            itemsDictionary.Remove(marker.Id);

            return marker;
        }

        protected override void AddItems(IList newItems)
        {
            base.AddItems(newItems);
        }

        protected override void RemoveItems(IList oldItems)
        {
            base.RemoveItems(oldItems);
        }

        protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnItemPropertyChanged(sender, e);

            if (e.PropertyName != Pin.PositionProperty.PropertyName)
                clusterRenderer.SetUpdateMarker((sender as Pin).NativeObject as ClusteredMarker);
        }

        public Pin LookupPin(ClusteredMarker marker)
        {
            var markerId = marker.Id;
            return markerId != null ?
                 (itemsDictionary.ContainsKey(markerId) ? itemsDictionary[markerId] : null)
                 : null;
        }

        public void HandleClusterRequest()
        {
            try
            {
                clusterManager.Cluster();
            }
            catch (Exception)
            {
            }
        }

        public override void OnMapPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Map.SelectedPinProperty.PropertyName)
            {
            }
        }

        private void UpdateSelectedPin(Pin pin)
        {
            if (pin == null)
            {
                foreach (var outerItem in GetItems(Map))
                {
                    (outerItem.NativeObject as Marker)?.HideInfoWindow();
                }
            }
            else
            {
                var targetPin = LookupPin(pin.NativeObject as ClusteredMarker);
                clusterRenderer.SetUpdateMarker(pin.NativeObject as ClusteredMarker);
                (targetPin?.NativeObject as Marker)?.ShowInfoWindow();
            }
        }

        private void UpdatePositionWithoutMove(Pin pin, Position position)
        {
            pin.Position = position;
        }

        protected override void OnUpdateAddress(Pin outerItem, ClusteredMarker nativeItem) => nativeItem.Snippet = outerItem.Address;

        protected override void OnUpdateLabel(Pin outerItem, ClusteredMarker nativeItem) => nativeItem.Title = outerItem.Label;

        protected override void OnUpdatePosition(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Position = outerItem.Position.ToLatLng();

            clusterRenderer.SetUpdatePositionMarker(outerItem.NativeObject as ClusteredMarker);
        }

        protected override void OnUpdateType(Pin outerItem, ClusteredMarker nativeItem)
        {
        }

        protected override void OnUpdateIcon(Pin outerItem, ClusteredMarker nativeItem)
        {
            if (outerItem.Icon != null && outerItem.Icon.Type == BitmapDescriptorType.View)
            {
                TransformXamarinViewToAndroidBitmap(outerItem, nativeItem);
            }
            else
            {
                var factory = bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
                var nativeDescriptor = factory.ToNative(outerItem.Icon);
                nativeItem.Icon = nativeDescriptor;
                nativeItem.AnchorX = 0.5f;
                nativeItem.AnchorY = 0.5f;
                nativeItem.InfoWindowAnchorX = 0.5f;
                nativeItem.InfoWindowAnchorY = 0.5f;
            }
        }

        public static Dictionary<string, Android.Gms.Maps.Model.BitmapDescriptor> cache = new Dictionary<string, global::Android.Gms.Maps.Model.BitmapDescriptor>();

        private async void TransformXamarinViewToAndroidBitmap(Pin outerItem, ClusteredMarker nativeItem)
        {
            try
            {
                if (outerItem?.Icon?.Type == BitmapDescriptorType.View && outerItem.Icon?.View != null && !string.IsNullOrEmpty(outerItem.Tag.ToString()))
                {
                    var iconView = outerItem.Icon.View;
                    nativeItem.Position = outerItem.Position.ToLatLng();
                    var exists = cache.ContainsKey(outerItem.Tag.ToString());
                    if (exists)
                    {
                        nativeItem.Icon = cache[outerItem.Tag.ToString()];
                    }
                    else
                    {
                        var nativeView = await Utils.ConvertFormsToNative(iconView,
                            new Rectangle(0, 0, Utils.DpToPx((float)iconView.WidthRequest),
                            Utils.DpToPx((float)iconView.HeightRequest)));

                        var otherView = new FrameLayout(nativeView.Context);
                        nativeView.LayoutParameters = new FrameLayout.LayoutParams(Utils.DpToPx((float)iconView.WidthRequest),
                            Utils.DpToPx((float)iconView.HeightRequest));
                        otherView.AddView(nativeView);
                        nativeItem.Icon = await Utils.ConvertViewToBitmapDescriptor(otherView);
                        cache.Add(outerItem.Tag.ToString(), nativeItem.Icon);
                    }
                    nativeItem.AnchorX = (float)iconView.AnchorX;
                    nativeItem.AnchorY = (float)iconView.AnchorY;
                    nativeItem.Visible = true;
                    if (outerItem.NativeObject == null)
                        AddMarker(outerItem, nativeItem);
                    else
                    {
                        clusterRenderer.SetUpdateMarker(nativeItem);
                    }
                }
            }
            catch
            {
            }
        }

        protected override void OnUpdateIsDraggable(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Draggable = outerItem?.IsDraggable ?? false;
        }

        protected override void OnUpdateRotation(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Rotation = outerItem?.Rotation ?? 0f;

            clusterRenderer.SetUpdateRotationMarker((ClusteredMarker)outerItem?.NativeObject);
        }

        protected override void OnUpdateIsVisible(Pin outerItem, ClusteredMarker nativeItem)
        {
            var isVisible = outerItem?.IsVisible ?? false;
            nativeItem.Visible = isVisible;

            if (!isVisible && ReferenceEquals(Map.SelectedPin, outerItem))
            {
                Map.SelectedPin = null;
            }
        }

        protected override void OnUpdateAnchor(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.AnchorX = (float)outerItem.Anchor.X;
            nativeItem.AnchorY = (float)outerItem.Anchor.Y;
        }

        protected override void OnUpdateFlat(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Flat = outerItem.Flat;
        }

        protected override void OnUpdateInfoWindowAnchor(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.InfoWindowAnchorX = (float)outerItem.InfoWindowAnchor.X;
            nativeItem.InfoWindowAnchorY = (float)outerItem.InfoWindowAnchor.Y;
        }

        protected override void OnUpdateZIndex(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }

        protected override void OnUpdateTransparency(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Alpha = 1f - outerItem.Transparency;
        }
    }
}