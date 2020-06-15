//using System;
//using System.ComponentModel;

//namespace BA_MobileGPS.Core.Logics
//{
//    public abstract class DefaultPinClusterLogic<TNative, TNativeMap> : DefaultLogic<PinCluster, TNative, TNativeMap>
//        where TNative : class
//        where TNativeMap : class
//    {
//        protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnItemPropertyChanged(sender, e);
//            var outerItem = sender as PinCluster;
//            var nativeItem = outerItem?.NativeObject as TNative;

//            if (nativeItem == null)
//                return;

//            if (e.PropertyName == PinCluster.AddressProperty.PropertyName) OnUpdateAddress(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.LabelProperty.PropertyName) OnUpdateLabel(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.PositionProperty.PropertyName) OnUpdatePosition(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.TypeProperty.PropertyName) OnUpdateType(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.IconProperty.PropertyName) OnUpdateIcon(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.IsDraggableProperty.PropertyName) OnUpdateIsDraggable(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.RotationProperty.PropertyName) OnUpdateRotation(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.IsVisibleProperty.PropertyName) OnUpdateIsVisible(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.AnchorProperty.PropertyName) OnUpdateAnchor(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.FlatProperty.PropertyName) OnUpdateFlat(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.InfoWindowAnchorProperty.PropertyName) OnUpdateInfoWindowAnchor(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.ZIndexProperty.PropertyName) OnUpdateZIndex(outerItem, nativeItem);
//            else if (e.PropertyName == PinCluster.TransparencyProperty.PropertyName) OnUpdateTransparency(outerItem, nativeItem);
//        }

//        protected abstract void OnUpdateAddress(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateLabel(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdatePosition(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateType(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateIcon(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateIsDraggable(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateRotation(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateIsVisible(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateAnchor(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateFlat(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateInfoWindowAnchor(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateZIndex(PinCluster outerItem, TNative nativeItem);

//        protected abstract void OnUpdateTransparency(PinCluster outerItem, TNative nativeItem);
//    }
//}