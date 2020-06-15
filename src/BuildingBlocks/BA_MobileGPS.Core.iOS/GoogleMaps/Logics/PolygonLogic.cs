using BA_MobileGPS.Core.iOS.Extensions;
using BA_MobileGPS.Core.Logics;

using Google.Maps;

using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms.Platform.iOS;

using NativePolygon = Google.Maps.Polygon;

namespace BA_MobileGPS.Core.iOS.Logics
{
    public class PolygonLogic : DefaultPolygonLogic<NativePolygon, MapView>
    {
        protected override IList<Polygon> GetItems(Map map) => map.Polygons;

        public override void Register(MapView oldNativeMap, Map oldMap, MapView newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            if (newNativeMap != null)
            {
                newNativeMap.OverlayTapped += OnOverlayTapped;
            }
        }

        public override void Unregister(MapView nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.OverlayTapped -= OnOverlayTapped;
            }

            base.Unregister(nativeMap, map);
        }

        protected override NativePolygon CreateNativeItem(Polygon outerItem)
        {
            var nativePolygon = NativePolygon.FromPath(outerItem.Positions.ToMutablePath());
            nativePolygon.StrokeWidth = outerItem.StrokeWidth;
            nativePolygon.StrokeColor = outerItem.StrokeColor.ToUIColor();
            nativePolygon.FillColor = outerItem.FillColor.ToUIColor().ColorWithAlpha(0.5f);
            nativePolygon.Tappable = outerItem.IsClickable;
            nativePolygon.ZIndex = outerItem.ZIndex;

            nativePolygon.Holes = outerItem.Holes
                .Select(hole => hole.ToMutablePath())
                .ToArray();

            outerItem.NativeObject = nativePolygon;
            nativePolygon.Map = NativeMap;

            outerItem.SetOnPositionsChanged((polygon, e) =>
            {
                var native = polygon.NativeObject as NativePolygon;
                native.Path = polygon.Positions.ToMutablePath();
            });

            outerItem.SetOnHolesChanged((polygon, e) =>
            {
                var native = polygon.NativeObject as NativePolygon;
                native.Holes = outerItem.Holes
                    .Select(hole => hole.ToMutablePath())
                    .ToArray();
            });

            return nativePolygon;
        }

        protected override NativePolygon DeleteNativeItem(Polygon outerItem)
        {
            outerItem.SetOnHolesChanged(null);

            var nativePolygon = outerItem.NativeObject as NativePolygon;
            nativePolygon.Map = null;
            return nativePolygon;
        }

        private void OnOverlayTapped(object sender, GMSOverlayEventEventArgs e)
        {
            var targetOuterItem = GetItems(Map).FirstOrDefault(
                outerItem => object.ReferenceEquals(outerItem.NativeObject, e.Overlay));
            targetOuterItem?.SendTap();
        }

        public override void OnUpdateIsClickable(Polygon outerItem, NativePolygon nativeItem)
        {
            nativeItem.Tappable = outerItem.IsClickable;
        }

        public override void OnUpdateStrokeColor(Polygon outerItem, NativePolygon nativeItem)
        {
            nativeItem.StrokeColor = outerItem.StrokeColor.ToUIColor();
        }

        public override void OnUpdateStrokeWidth(Polygon outerItem, NativePolygon nativeItem)
        {
            nativeItem.StrokeWidth = outerItem.StrokeWidth;
        }

        public override void OnUpdateFillColor(Polygon outerItem, NativePolygon nativeItem)
        {
            nativeItem.FillColor = outerItem.FillColor.ToUIColor();
        }

        public override void OnUpdateZIndex(Polygon outerItem, NativePolygon nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }
    }
}