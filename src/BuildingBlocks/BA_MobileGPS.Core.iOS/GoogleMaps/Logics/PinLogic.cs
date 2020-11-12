using BA_MobileGPS.Core.iOS.Extensions;
using BA_MobileGPS.Core.iOS.Factories;
using BA_MobileGPS.Core.Logics;

using CoreGraphics;

using Google.Maps;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using UIKit;

namespace BA_MobileGPS.Core.iOS.Logics
{
    internal class PinLogic : DefaultPinLogic<Marker, MapView>
    {
        protected override IList<Pin> GetItems(Map map) => map.Pins;

        private bool _onMarkerEvent;
        private Pin _draggingPin;
        private volatile bool _withoutUpdateNative = false;

        private readonly IImageFactory _imageFactory;

        public static Dictionary<string, UIImage> cache = new Dictionary<string, UIImage>();

        public PinLogic(
            IImageFactory imageFactory)
        {
            _imageFactory = imageFactory;
        }

        public override void Register(MapView oldNativeMap, Map oldMap, MapView newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            if (newNativeMap != null)
            {
                newNativeMap.InfoTapped += OnInfoTapped;
                newNativeMap.InfoLongPressed += OnInfoLongPressed;
                newNativeMap.TappedMarker = HandleGMSTappedMarker;
                newNativeMap.InfoClosed += InfoWindowClosed;
                newNativeMap.DraggingMarkerStarted += DraggingMarkerStarted;
                newNativeMap.DraggingMarkerEnded += DraggingMarkerEnded;
                newNativeMap.DraggingMarker += DraggingMarker;
            }
        }

        public override void Unregister(MapView nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.DraggingMarker -= DraggingMarker;
                nativeMap.DraggingMarkerEnded -= DraggingMarkerEnded;
                nativeMap.DraggingMarkerStarted -= DraggingMarkerStarted;
                nativeMap.InfoClosed -= InfoWindowClosed;
                nativeMap.TappedMarker = null;
                nativeMap.InfoTapped -= OnInfoTapped;
                nativeMap.InfoLongPressed -= OnInfoLongPressed;
            }

            base.Unregister(nativeMap, map);
        }

        protected override Marker CreateNativeItem(Pin outerItem)
        {
            var nativeMarker = Marker.FromPosition(outerItem.Position.ToCoord());
            nativeMarker.Title = outerItem.Label;
            nativeMarker.Snippet = outerItem.Address ?? string.Empty;
            nativeMarker.Draggable = outerItem.IsDraggable;
            nativeMarker.Rotation = outerItem.Rotation;
            nativeMarker.GroundAnchor = new CGPoint(outerItem.Anchor.X, outerItem.Anchor.Y);
            nativeMarker.Flat = outerItem.Flat;
            nativeMarker.ZIndex = outerItem.ZIndex;
            nativeMarker.Opacity = 1f - outerItem.Transparency;
            nativeMarker.AppearAnimation = MarkerAnimation.None;
            if (outerItem.Icon != null && outerItem.Icon.Type != BitmapDescriptorType.View)
            {
                var factory = _imageFactory ?? DefaultImageFactory.Instance;
                nativeMarker.Icon = factory.ToUIImage(outerItem.Icon);
            }
            outerItem.NativeObject = nativeMarker;
            nativeMarker.Map = outerItem.IsVisible ? NativeMap : null;

            OnUpdateIconView(outerItem, nativeMarker);

            return nativeMarker;
        }

        protected override Marker DeleteNativeItem(Pin outerItem)
        {
            var nativeMarker = outerItem.NativeObject as Marker;
            nativeMarker.Map = null;

            if (ReferenceEquals(Map.SelectedPin, outerItem))
                Map.SelectedPin = null;
            return nativeMarker;
        }

        public override void OnMapPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Map.SelectedPinProperty.PropertyName)
            {
                if (!_onMarkerEvent)
                    UpdateSelectedPin(Map.SelectedPin);
                Map.SendSelectedPinChanged(Map.SelectedPin);
            }
        }

        private void UpdateSelectedPin(Pin pin)
        {
            if (pin != null)
                NativeMap.SelectedMarker = (Marker)pin.NativeObject;
            else
                NativeMap.SelectedMarker = null;
        }

        private Pin LookupPin(Marker marker)
        {
            return GetItems(Map).FirstOrDefault(outerItem => ReferenceEquals(outerItem.NativeObject, marker));
        }

        private void OnInfoTapped(object sender, GMSMarkerEventEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.Marker);

            // only consider event handled if a handler is present.
            // Else allow default behavior of displaying an info window.
            targetPin?.SendTap();

            if (targetPin != null)
            {
                Map.SendInfoWindowClicked(targetPin);
            }
        }

        private void OnInfoLongPressed(object sender, GMSMarkerEventEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.Marker);

            if (targetPin != null)
            {
                Map.SendInfoWindowLongClicked(targetPin);
            }
        }

        private bool HandleGMSTappedMarker(MapView mapView, Marker marker)
        {
            // lookup pin
            var targetPin = LookupPin(marker);

            // If set to PinClickedEventArgs.Handled = true in app codes,
            // then all pin selection controlling by app.
            if (Map.SendPinClicked(targetPin))
            {
                return true;
            }

            try
            {
                _onMarkerEvent = true;
                if (targetPin != null && !ReferenceEquals(targetPin, Map.SelectedPin))
                    Map.SelectedPin = targetPin;
            }
            finally
            {
                _onMarkerEvent = false;
            }

            return false;
        }

        private void InfoWindowClosed(object sender, GMSMarkerEventEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.Marker);

            try
            {
                _onMarkerEvent = true;
                if (targetPin != null && ReferenceEquals(targetPin, Map.SelectedPin))
                    Map.SelectedPin = null;
            }
            finally
            {
                _onMarkerEvent = false;
            }
        }

        private void DraggingMarkerStarted(object sender, GMSMarkerEventEventArgs e)
        {
            // lookup pin
            _draggingPin = LookupPin(e.Marker);

            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.Marker.Position.ToPosition());
                Map.SendPinDragStart(_draggingPin);
            }
        }

        private void DraggingMarkerEnded(object sender, GMSMarkerEventEventArgs e)
        {
            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.Marker.Position.ToPosition());
                Map.SendPinDragEnd(_draggingPin);
                _draggingPin = null;
            }
        }

        private void DraggingMarker(object sender, GMSMarkerEventEventArgs e)
        {
            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.Marker.Position.ToPosition());
                Map.SendPinDragging(_draggingPin);
            }
        }

        private void UpdatePositionWithoutMove(Pin pin, Position position)
        {
            try
            {
                _withoutUpdateNative = true;
                pin.Position = position;
            }
            finally
            {
                _withoutUpdateNative = false;
            }
        }

        protected override void OnUpdateAddress(Pin outerItem, Marker nativeItem)
            => nativeItem.Snippet = outerItem.Address;

        protected override void OnUpdateLabel(Pin outerItem, Marker nativeItem)
            => nativeItem.Title = outerItem.Label;

        protected override void OnUpdatePosition(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Position = outerItem.Position.ToCoord();
        }

        protected override void OnUpdateType(Pin outerItem, Marker nativeItem)
        {
        }

        protected override void OnUpdateIcon(Pin outerItem, Marker nativeItem)
        {
            if (outerItem.Icon.Type == BitmapDescriptorType.View)
            {
                OnUpdateIconView(outerItem, nativeItem);
            }
            else
            {
                var factory = _imageFactory ?? DefaultImageFactory.Instance;
                nativeItem.Icon = factory.ToUIImage(outerItem.Icon);
            }
        }

        protected override void OnUpdateIsDraggable(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Draggable = outerItem?.IsDraggable ?? false;
        }

        protected void OnUpdateIconView(Pin outerItem, Marker nativeItem)
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

        protected override void OnUpdateRotation(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Rotation = outerItem?.Rotation ?? 0f;
        }

        protected override void OnUpdateIsVisible(Pin outerItem, Marker nativeItem)
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

        protected override void OnUpdateAnchor(Pin outerItem, Marker nativeItem)
        {
            nativeItem.GroundAnchor = new CGPoint(outerItem.Anchor.X, outerItem.Anchor.Y);
        }

        protected override void OnUpdateFlat(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Flat = outerItem.Flat;
        }

        protected override void OnUpdateInfoWindowAnchor(Pin outerItem, Marker nativeItem)
        {
            nativeItem.InfoWindowAnchor = new CGPoint(outerItem.InfoWindowAnchor.X, outerItem.InfoWindowAnchor.Y);
        }

        protected override void OnUpdateZIndex(Pin outerItem, Marker nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }

        protected override void OnUpdateTransparency(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Opacity = 1f - outerItem.Transparency;
        }
    }
}