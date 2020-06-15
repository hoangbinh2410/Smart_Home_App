using System.ComponentModel;

namespace BA_MobileGPS.Core.Logics
{
    public abstract class DefaultPolylineLogic<TNative, TNativeMap> : DefaultLogic<Polyline, TNative, TNativeMap>
        where TNative : class
        where TNativeMap : class
    {
        protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnItemPropertyChanged(sender, e);
            var outerItem = sender as Polyline;
            var nativeItem = outerItem?.NativeObject as TNative;

            if (nativeItem == null)
                return;

            if (e.PropertyName == Polyline.IsClickableProperty.PropertyName) OnUpdateIsClickable(outerItem, nativeItem);
            else if (e.PropertyName == Polyline.StrokeColorProperty.PropertyName) OnUpdateStrokeColor(outerItem, nativeItem);
            else if (e.PropertyName == Polyline.StrokeWidthProperty.PropertyName) OnUpdateStrokeWidth(outerItem, nativeItem);
            else if (e.PropertyName == Polyline.ZIndexProperty.PropertyName) OnUpdateZIndex(outerItem, nativeItem);
        }

        public abstract void OnUpdateIsClickable(Polyline outerItem, TNative nativeItem);

        public abstract void OnUpdateStrokeColor(Polyline outerItem, TNative nativeItem);

        public abstract void OnUpdateStrokeWidth(Polyline outerItem, TNative nativeItem);

        public abstract void OnUpdateZIndex(Polyline outerItem, TNative nativeItem);
    }
}