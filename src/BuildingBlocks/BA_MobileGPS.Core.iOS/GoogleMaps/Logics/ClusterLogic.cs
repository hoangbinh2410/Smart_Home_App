using BA_MobileGPS.Core.iOS.Extensions;
using BA_MobileGPS.Core.iOS.Factories;
using BA_MobileGPS.Core.Logics;

using CoreGraphics;

using Foundation;

using Google.Maps;
using Google.Maps.Utility;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UIKit;

namespace BA_MobileGPS.Core.iOS.Logics
{
    public class ClusterLogic : DefaultPinLogic<ClusteredMarker, MapView>
    {
        protected override IList<Pin> GetItems(Map map) => Map.ClusteredPins;

        private Map ClusteredMap => (Map)Map;

        private ClusterManager clusterManager;

        private bool onMarkerEvent;
        private readonly IImageFactory imageFactory;
        private ClusterRendererHandler clusterRenderer;

        private readonly Dictionary<NSObject, Pin> itemsDictionary = new Dictionary<NSObject, Pin>();

        public static Dictionary<string, UIImage> cache = new Dictionary<string, UIImage>();

        public ClusterLogic(
            IImageFactory imageFactory)
        {
            this.imageFactory = imageFactory;
        }

        public override void Register(MapView oldNativeMap, Map oldMap, MapView newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            var clusteredNewMap = (Map)newMap;
            var algorithm = GetClusterAlgorithm(clusteredNewMap);

            var iconGenerator = new ClusterIconGeneratorHandler(ClusteredMap.ClusterOptions);
            clusterRenderer = new ClusterRendererHandler(newNativeMap, iconGenerator);
            clusterManager = new ClusterManager(newNativeMap, algorithm, clusterRenderer);

            ClusteredMap.OnCluster = HandleClusterRequest;

            if (newNativeMap == null) return;
            newNativeMap.InfoTapped += OnInfoTapped;
            newNativeMap.InfoLongPressed += OnInfoLongPressed;
            newNativeMap.TappedMarker = HandleGmsTappedMarker;
            newNativeMap.InfoClosed += InfoWindowClosed;
        }

        private static IClusterAlgorithm GetClusterAlgorithm(Map clusteredNewMap)
        {
            IClusterAlgorithm algorithm;
            switch (clusteredNewMap.ClusterOptions.Algorithm)
            {
                case ClusterAlgorithm.GridBased:
                    algorithm = new GridBasedClusterAlgorithm();
                    break;

                case ClusterAlgorithm.VisibleNonHierarchicalDistanceBased:
                    throw new NotSupportedException("VisibleNonHierarchicalDistanceBased is only supported on Android");
                default:
                    algorithm = new NonHierarchicalDistanceBasedAlgorithm();
                    break;
            }

            return algorithm;
        }

        public override void Unregister(MapView nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.InfoClosed -= InfoWindowClosed;
                nativeMap.TappedMarker = null;
                nativeMap.InfoTapped -= OnInfoTapped;
            }

            ClusteredMap.OnCluster = null;

            base.Unregister(nativeMap, map);
        }

        protected override ClusteredMarker CreateNativeItem(Pin outerItem)
        {
            var nativeMarker = new ClusteredMarker
            {
                Position = outerItem.Position.ToCoord(),
                Title = outerItem.Label,
                Snippet = outerItem.Address ?? string.Empty,
                Draggable = outerItem.IsDraggable,
                Rotation = outerItem.Rotation,
                GroundAnchor = new CGPoint(outerItem.Anchor.X, outerItem.Anchor.Y),
                InfoWindowAnchor = new CGPoint(outerItem.InfoWindowAnchor.X, outerItem.InfoWindowAnchor.Y),
                Flat = outerItem.Flat,
                ZIndex = outerItem.ZIndex,
                Opacity = 1f - outerItem.Transparency
            };

            if (outerItem.Icon != null && outerItem?.Icon?.Type != BitmapDescriptorType.View)
            {
                var factory = imageFactory ?? DefaultImageFactory.Instance;
                nativeMarker.Icon = factory.ToUIImage(outerItem.Icon);
            }

            outerItem.NativeObject = nativeMarker;

            clusterManager.AddItem(nativeMarker);

            itemsDictionary.Add(nativeMarker, outerItem);
            OnUpdateIconView(outerItem, nativeMarker);
            return nativeMarker;
        }

        protected override ClusteredMarker DeleteNativeItem(Pin outerItem)
        {
            if (outerItem?.NativeObject == null)
                return null;
            var nativeMarker = outerItem.NativeObject as ClusteredMarker;

            nativeMarker.Map = null;

            clusterManager.RemoveItem(nativeMarker);

            if (ReferenceEquals(Map.SelectedPin, outerItem))
                Map.SelectedPin = null;

            itemsDictionary.Remove(nativeMarker);

            return nativeMarker;
        }

        protected override void AddItems(IList newItems)
        {
            base.AddItems(newItems);
            //clusterManager.Cluster();
        }

        protected override void RemoveItems(IList oldItems)
        {
            base.RemoveItems(oldItems);
            //clusterManager.Cluster();
        }

        protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnItemPropertyChanged(sender, e);
            if (e.PropertyName != Pin.PositionProperty.PropertyName)
                clusterRenderer.SetUpdateMarker((ClusteredMarker)(sender as Pin)?.NativeObject);
        }

        public override void OnMapPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Map.SelectedPinProperty.PropertyName)
            {
            }
        }

        private void UpdateSelectedPin(Pin pin)
        {
            if (pin != null)
                NativeMap.SelectedMarker = (ClusteredMarker)pin.NativeObject;
            else
                NativeMap.SelectedMarker = null;
        }

        private Pin LookupPin(Marker marker)
        {
            var associatedClusteredMarker = marker.UserData;
            return associatedClusteredMarker != null ? itemsDictionary[associatedClusteredMarker] : null;
        }

        private void HandleClusterRequest()
        {
            try
            {
                clusterManager.Cluster();
            }
            catch (Exception)
            {
            }
        }

        private void OnInfoTapped(object sender, GMSMarkerEventEventArgs e)
        {
            var targetPin = LookupPin(e.Marker);

            targetPin?.SendTap();

            if (targetPin != null)
            {
                Map.SendInfoWindowClicked(targetPin);
            }
        }

        private void OnInfoLongPressed(object sender, GMSMarkerEventEventArgs e)
        {
            var targetPin = LookupPin(e.Marker);

            if (targetPin != null)
                Map.SendInfoWindowLongClicked(targetPin);
        }

        private bool HandleGmsTappedMarker(MapView mapView, Marker marker)
        {
            if (marker?.UserData is ICluster cluster)
            {
                var pins = GetClusterPins(cluster);
                var clusterPosition = new Position(cluster.Position.Latitude, cluster.Position.Longitude);
                return ClusteredMap.SendClusterClicked((int)cluster.Count, pins, clusterPosition);
            }
            var targetPin = LookupPin(marker);

            if (Map.SendPinClicked(targetPin))
                return true;

            try
            {
                onMarkerEvent = true;
                if (targetPin != null && !ReferenceEquals(targetPin, Map.SelectedPin))
                    Map.SelectedPin = targetPin;
            }
            finally
            {
                onMarkerEvent = false;
            }

            return false;
        }

        private void InfoWindowClosed(object sender, GMSMarkerEventEventArgs e)
        {
            var targetPin = LookupPin(e.Marker);

            try
            {
                onMarkerEvent = true;
                if (targetPin != null && ReferenceEquals(targetPin, Map.SelectedPin))
                    Map.SelectedPin = null;
            }
            finally
            {
                onMarkerEvent = false;
            }
        }

        protected override void OnUpdateAddress(Pin outerItem, ClusteredMarker nativeItem)
            => nativeItem.Snippet = outerItem.Address;

        protected override void OnUpdateLabel(Pin outerItem, ClusteredMarker nativeItem)
            => nativeItem.Title = outerItem.Label;

        protected override void OnUpdatePosition(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Position = outerItem.Position.ToCoord();

            clusterRenderer.SetUpdatePositionMarker((ClusteredMarker)outerItem?.NativeObject);
        }

        protected override void OnUpdateType(Pin outerItem, ClusteredMarker nativeItem)
        {
        }

        protected override void OnUpdateIcon(Pin outerItem, ClusteredMarker nativeItem)
        {
            if (outerItem.Icon.Type == BitmapDescriptorType.View)
                OnUpdateIconView(outerItem, nativeItem);
            else
            {
                if (nativeItem?.Icon != null)
                    nativeItem.Icon = DefaultImageFactory.Instance.ToUIImage(outerItem.Icon);
            }
        }

        protected override void OnUpdateIsDraggable(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Draggable = outerItem?.IsDraggable ?? false;
        }

        protected void OnUpdateIconView(Pin outerItem, ClusteredMarker nativeItem)
        {
            try
            {
                if (outerItem?.Icon?.Type == BitmapDescriptorType.View && outerItem?.Icon?.View != null)
                {
                    nativeItem.GroundAnchor = new CGPoint(outerItem.Icon.View.AnchorX, outerItem.Icon.View.AnchorY);
                    var exists = cache.ContainsKey(outerItem.Tag.ToString());
                    if (exists)
                    {
                        nativeItem.Icon = cache[outerItem.Tag.ToString()];
                    }
                    else
                    {
                        NativeMap.InvokeOnMainThread(() =>
                        {
                            nativeItem.Icon = Utils.ConvertViewToImage(outerItem);

                            cache.Add(outerItem.Tag.ToString(), nativeItem.Icon);
                        });
                    }
                }
            }
            catch
            {
            }
        }

        protected override void OnUpdateRotation(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Rotation = outerItem?.Rotation ?? 0f;

            clusterRenderer.SetUpdateRotationMarker((ClusteredMarker)outerItem?.NativeObject);
        }

        protected override void OnUpdateIsVisible(Pin outerItem, ClusteredMarker nativeItem)
        {
            if (outerItem?.IsVisible ?? false)
            {
                nativeItem.Map = NativeMap;
            }
            else
            {
                nativeItem.Map = null;
                if (ReferenceEquals(Map.SelectedPin, outerItem))
                    Map.SelectedPin = null;
            }
        }

        protected override void OnUpdateAnchor(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.GroundAnchor = new CGPoint(outerItem.Anchor.X, outerItem.Anchor.Y);
        }

        protected override void OnUpdateFlat(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Flat = outerItem.Flat;
        }

        protected override void OnUpdateInfoWindowAnchor(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.InfoWindowAnchor = new CGPoint(outerItem.Anchor.X, outerItem.Anchor.Y);
        }

        protected override void OnUpdateZIndex(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }

        protected override void OnUpdateTransparency(Pin outerItem, ClusteredMarker nativeItem)
        {
            nativeItem.Opacity = 1f - outerItem.Transparency;
        }

        public List<Pin> GetClusterPins(ICluster cluster)
        {
            var pins = new List<Pin>();
            foreach (var item in cluster.Items)
            {
                var clusterItem = (ClusteredMarker)item;
                pins.Add(itemsDictionary[clusterItem]);
            }

            return pins;
        }
    }
}