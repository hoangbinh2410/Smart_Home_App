using Android.Gms.Maps.Model;

using BA_MobileGPS.Core.Logics;

using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms.Platform.Android;

using NativePolyline = Android.Gms.Maps.Model.Polyline;

namespace BA_MobileGPS.Core.Droid.Logics
{
    public class PolylineLogic : DefaultPolylineLogic<NativePolyline, Android.Gms.Maps.GoogleMap>
    {
        protected override IList<Polyline> GetItems(Map map) => map.Polylines;

        public override void Register(Android.Gms.Maps.GoogleMap oldNativeMap, Map oldMap, Android.Gms.Maps.GoogleMap newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            if (newNativeMap != null)
            {
                newNativeMap.PolylineClick += OnPolylineClick;
            }
        }

        public override void Unregister(Android.Gms.Maps.GoogleMap nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.PolylineClick -= OnPolylineClick;
            }

            base.Unregister(nativeMap, map);
        }

        protected override NativePolyline CreateNativeItem(Polyline outerItem)
        {
            var opts = new PolylineOptions();

            foreach (var p in outerItem.Positions)
                opts.Add(new LatLng(p.Latitude, p.Longitude));

            opts.InvokeWidth(outerItem.StrokeWidth * this.ScaledDensity); // TODO: convert from px to pt. Is this collect? (looks like same iOS Maps)
            opts.InvokeColor(outerItem.StrokeColor.ToAndroid());
            opts.Clickable(outerItem.IsClickable);
            opts.InvokeZIndex(outerItem.ZIndex);

            var nativePolyline = NativeMap.AddPolyline(opts);

            // associate pin with marker for later lookup in event handlers
            outerItem.NativeObject = nativePolyline;
            outerItem.SetOnPositionsChanged((polyline, e) =>
            {
                var native = polyline.NativeObject as NativePolyline;
                native.Points = polyline.Positions.ToLatLngs();
            });

            return nativePolyline;
        }

        protected override NativePolyline DeleteNativeItem(Polyline outerItem)
        {
            outerItem.SetOnPositionsChanged(null);

            var nativeShape = outerItem.NativeObject as NativePolyline;
            if (nativeShape == null)
                return null;

            nativeShape.Remove();
            outerItem.NativeObject = null;
            return nativeShape;
        }

        private void OnPolylineClick(object sender, Android.Gms.Maps.GoogleMap.PolylineClickEventArgs e)
        {
            // clicked polyline
            var nativeItem = e.Polyline;

            // lookup pin
            var targetOuterItem = GetItems(Map).FirstOrDefault(
                outerItem => ((NativePolyline)outerItem.NativeObject).Id == nativeItem.Id);

            // only consider event handled if a handler is present.
            // Else allow default behavior of displaying an info window.
            targetOuterItem?.SendTap();
        }

        public override void OnUpdateIsClickable(Polyline outerItem, NativePolyline nativeItem)
        {
            nativeItem.Clickable = outerItem.IsClickable;
        }

        public override void OnUpdateStrokeColor(Polyline outerItem, NativePolyline nativeItem)
        {
            nativeItem.Color = outerItem.StrokeColor.ToAndroid();
        }

        public override void OnUpdateStrokeWidth(Polyline outerItem, NativePolyline nativeItem)
        {
            nativeItem.Width = outerItem.StrokeWidth;
        }

        public override void OnUpdateZIndex(Polyline outerItem, NativePolyline nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }
    }
}