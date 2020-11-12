using Android.App;
using Android.Gms.Maps.Model;
using Android.Widget;

using BA_MobileGPS.Core.Droid.Extensions;
using BA_MobileGPS.Core.Droid.Factories;
using BA_MobileGPS.Core.Logics;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Xamarin.Forms.Platform.Android;

namespace BA_MobileGPS.Core.Droid.Logics
{
    internal class PinLogic : DefaultPinLogic<Marker, Android.Gms.Maps.GoogleMap>
    {
        protected override IList<Pin> GetItems(Map map) => map.Pins;

        private volatile bool _onMarkerEvent = false;
        private Pin _draggingPin;
        private volatile bool _withoutUpdateNative = false;

        private readonly Activity context;
        private readonly IBitmapDescriptorFactory bitmapDescriptorFactory;

        public PinLogic(Activity context,
            IBitmapDescriptorFactory bitmapDescriptorFactory)
        {
            this.bitmapDescriptorFactory = bitmapDescriptorFactory;
            this.context = context;
        }

        public override void Register(Android.Gms.Maps.GoogleMap oldNativeMap, Map oldMap, Android.Gms.Maps.GoogleMap newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            if (newNativeMap != null)
            {
                newNativeMap.InfoWindowClick += OnInfoWindowClick;
                newNativeMap.InfoWindowLongClick += OnInfoWindowLongClick;
                newNativeMap.MarkerClick += OnMakerClick;
                newNativeMap.InfoWindowClose += OnInfoWindowClose;
                newNativeMap.MarkerDragStart += OnMarkerDragStart;
                newNativeMap.MarkerDragEnd += OnMarkerDragEnd;
                newNativeMap.MarkerDrag += OnMarkerDrag;
            }
        }

        public override void Unregister(Android.Gms.Maps.GoogleMap nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.MarkerDrag -= OnMarkerDrag;
                nativeMap.MarkerDragEnd -= OnMarkerDragEnd;
                nativeMap.MarkerDragStart -= OnMarkerDragStart;
                nativeMap.MarkerClick -= OnMakerClick;
                nativeMap.InfoWindowClose -= OnInfoWindowClose;
                nativeMap.InfoWindowClick -= OnInfoWindowClick;
                nativeMap.InfoWindowLongClick -= OnInfoWindowLongClick;
            }

            base.Unregister(nativeMap, map);
        }

        protected override Marker CreateNativeItem(Pin outerItem)
        {
            var opts = new MarkerOptions()
                .SetPosition(new LatLng(outerItem.Position.Latitude, outerItem.Position.Longitude))
                .SetTitle(outerItem.Label)
                .SetSnippet(outerItem.Address)
                .Draggable(outerItem.IsDraggable)
                .SetRotation(outerItem.Rotation)
                .Anchor((float)outerItem.Anchor.X, (float)outerItem.Anchor.Y)
                .InvokeZIndex(outerItem.ZIndex)
                .Flat(outerItem.Flat)
                .SetAlpha(1f - outerItem.Transparency);

            if (outerItem.Icon != null && outerItem?.Icon?.Type != BitmapDescriptorType.View)
            {
                var factory = bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
                var nativeDescriptor = factory.ToNative(outerItem.Icon);
                opts.SetIcon(nativeDescriptor);
            }

            var marker = NativeMap.AddMarker(opts);
            marker.Tag = outerItem.Tag.ToString();
            // If the pin has an IconView set this method will convert it into an icon for the marker
            if (outerItem?.Icon?.Type == BitmapDescriptorType.View)
            {
                marker.Visible = false; // Will become visible once the iconview is ready.
                TransformXamarinViewToAndroidBitmap(outerItem, marker);
            }
            else
            {
                marker.Visible = outerItem.IsVisible;
            }

            // associate pin with marker for later lookup in event handlers
            outerItem.NativeObject = marker;
            return marker;
        }

        protected override Marker DeleteNativeItem(Pin outerItem)
        {
            if (!(outerItem.NativeObject is Marker marker))
                return null;

            marker.Remove();
            outerItem.NativeObject = null;

            if (ReferenceEquals(Map.SelectedPin, outerItem))
                Map.SelectedPin = null;

            return marker;
        }

        private Pin LookupPin(Marker marker)
        {
            if (marker != null)
                return GetItems(Map).FirstOrDefault(outerItem => ((Marker)outerItem.NativeObject).Id == marker.Id);
            return null;
        }

        private void OnInfoWindowClick(object sender, Android.Gms.Maps.GoogleMap.InfoWindowClickEventArgs e)
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

        private void OnInfoWindowLongClick(object sender, Android.Gms.Maps.GoogleMap.InfoWindowLongClickEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.Marker);

            // only consider event handled if a handler is present.
            // Else allow default behavior of displaying an info window.
            if (targetPin != null)
            {
                Map.SendInfoWindowLongClicked(targetPin);
            }
        }

        private void OnMakerClick(object sender, Android.Gms.Maps.GoogleMap.MarkerClickEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.Marker);

            // If set to PinClickedEventArgs.Handled = true in app codes,
            // then all pin selection controlling by app.
            if (Map.SendPinClicked(targetPin))
            {
                e.Handled = true;
                return;
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

            e.Handled = false;
        }

        private void OnInfoWindowClose(object sender, Android.Gms.Maps.GoogleMap.InfoWindowCloseEventArgs e)
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

        private void OnMarkerDragStart(object sender, Android.Gms.Maps.GoogleMap.MarkerDragStartEventArgs e)
        {
            // lookup pin
            _draggingPin = LookupPin(e.Marker);

            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.Marker.Position.ToPosition());
                Map.SendPinDragStart(_draggingPin);
            }
        }

        private void OnMarkerDrag(object sender, Android.Gms.Maps.GoogleMap.MarkerDragEventArgs e)
        {
            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.Marker.Position.ToPosition());
                Map.SendPinDragging(_draggingPin);
            }
        }

        private void OnMarkerDragEnd(object sender, Android.Gms.Maps.GoogleMap.MarkerDragEndEventArgs e)
        {
            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.Marker.Position.ToPosition());
                Map.SendPinDragEnd(_draggingPin);
                _draggingPin = null;
            }
            _withoutUpdateNative = false;
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
            if (pin == null)
            {
                foreach (var outerItem in GetItems(Map))
                {
                    (outerItem.NativeObject as Marker)?.HideInfoWindow();
                }
            }
            else
            {
                // lookup pin
                var targetPin = LookupPin(pin.NativeObject as Marker);
                (targetPin?.NativeObject as Marker)?.ShowInfoWindow();
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
            if (!_withoutUpdateNative)
            {
                nativeItem.Position = outerItem.Position.ToLatLng();
            }
        }

        protected override void OnUpdateType(Pin outerItem, Marker nativeItem)
        {
        }

        protected override void OnUpdateIcon(Pin outerItem, Marker nativeItem)
        {
            if (outerItem.Icon != null && outerItem.Icon.Type == BitmapDescriptorType.View)
            {
                // If the pin has an IconView set this method will convert it into an icon for the marker
                TransformXamarinViewToAndroidBitmap(outerItem, nativeItem);
            }
            else
            {
                var factory = bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
                var nativeDescriptor = factory.ToNative(outerItem.Icon);
                nativeItem.SetIcon(nativeDescriptor);
            }
        }

        public static Dictionary<string, global::Android.Gms.Maps.Model.BitmapDescriptor> cache = new Dictionary<string, global::Android.Gms.Maps.Model.BitmapDescriptor>();

        private async void TransformXamarinViewToAndroidBitmap(Pin outerItem, Marker nativeItem)
        {
            try
            {
                if (outerItem?.Icon?.Type == BitmapDescriptorType.View && outerItem.Icon?.View != null)
                {
                    var iconView = outerItem.Icon.View;
                    var exists = cache.ContainsKey(outerItem.Tag.ToString());
                    if (exists)
                    {
                        nativeItem.SetIcon(cache[outerItem.Tag.ToString()]);
                    }
                    else
                    {
                        var nativeView = await Utils.ConvertFormsToNative(iconView,
                            new Xamarin.Forms.Rectangle(0, 0, Utils.DpToPx((float)iconView.WidthRequest),
                            Utils.DpToPx((float)iconView.HeightRequest)),
                            Platform.CreateRendererWithContext(iconView, context));

                        var otherView = new FrameLayout(context);
                        nativeView.LayoutParameters = new FrameLayout.LayoutParams(Utils.DpToPx((float)iconView.WidthRequest), Utils.DpToPx((float)iconView.HeightRequest));
                        otherView.AddView(nativeView);
                        var icon = await Utils.ConvertViewToBitmapDescriptor(otherView);
                        if (outerItem.NativeObject != null)
                        {
                            nativeItem.SetIcon(icon);
                        }

                        cache.Add(outerItem.Tag.ToString(), icon);
                    }
                    nativeItem.SetAnchor((float)iconView.AnchorX, (float)iconView.AnchorY);
                    nativeItem.Visible = true;
                }
            }
            catch
            {
            }
        }

        protected override void OnUpdateIsDraggable(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Draggable = outerItem?.IsDraggable ?? false;
        }

        protected override void OnUpdateRotation(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Rotation = outerItem?.Rotation ?? 0f;
        }

        protected override void OnUpdateIsVisible(Pin outerItem, Marker nativeItem)
        {
            var isVisible = outerItem?.IsVisible ?? false;
            nativeItem.Visible = isVisible;

            if (!isVisible && ReferenceEquals(Map.SelectedPin, outerItem))
            {
                Map.SelectedPin = null;
            }
        }

        protected override void OnUpdateAnchor(Pin outerItem, Marker nativeItem)
        {
            nativeItem.SetAnchor((float)outerItem.Anchor.X, (float)outerItem.Anchor.Y);
        }

        protected override void OnUpdateFlat(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Flat = outerItem.Flat;
        }

        protected override void OnUpdateInfoWindowAnchor(Pin outerItem, Marker nativeItem)
        {
            nativeItem.SetInfoWindowAnchor((float)outerItem.InfoWindowAnchor.X, (float)outerItem.InfoWindowAnchor.Y);
        }

        protected override void OnUpdateZIndex(Pin outerItem, Marker nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }

        protected override void OnUpdateTransparency(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Alpha = 1f - outerItem.Transparency;
        }
    }
}