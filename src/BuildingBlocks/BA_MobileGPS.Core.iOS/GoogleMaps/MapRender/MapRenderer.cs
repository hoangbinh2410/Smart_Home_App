using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Internals;
using BA_MobileGPS.Core.iOS;
using BA_MobileGPS.Core.iOS.Extensions;
using BA_MobileGPS.Core.iOS.Logics;
using BA_MobileGPS.Core.Logics;

using Google.Maps;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using GCameraPosition = Google.Maps.CameraPosition;

[assembly: ExportRenderer(typeof(Map), typeof(MapRenderer))]

namespace BA_MobileGPS.Core.iOS
{
    public class MapRenderer : ViewRenderer
    {
        private bool _shouldUpdateRegion = true;

        // ReSharper disable once MemberCanBePrivate.Global
        protected MapView NativeMap => (MapView)Control;

        // ReSharper disable once MemberCanBePrivate.Global
        protected Map Map => (Map)Element;

        internal static PlatformConfig Config { private get; set; }

        private readonly UiSettingsLogic _uiSettingsLogic = new UiSettingsLogic();
        private readonly CameraLogic _cameraLogic;
        private readonly ClusterLogic _clusterLogic;

        private readonly BaseLogic<MapView>[] _logics;

        private bool _ready = false;

        public MapRenderer()
        {
            _logics = new BaseLogic<MapView>[]
            {
                new PolylineLogic(),
                new PolygonLogic(),
                new CircleLogic(),
                new PinLogic(Config.ImageFactory),
                new GroundOverlayLogic(Config.ImageFactory)
            };
            _clusterLogic = new ClusterLogic(Config.ImageFactory);
            _cameraLogic = new CameraLogic(() =>
            {
                OnCameraPositionChanged(NativeMap.Camera);
            });
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return Control.GetSizeRequest(widthConstraint, heightConstraint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Map != null)
                {
                    Map.OnSnapshot -= OnSnapshot;
                    foreach (var logic in _logics)
                    {
                        logic.Unregister(NativeMap, Map);
                    }
                    if (Map.IsUseCluster)
                    {
                        _clusterLogic.Unregister(NativeMap, Map);
                    }
                }
                _cameraLogic.Unregister();
                _uiSettingsLogic.Unregister();

                var mkMapView = (MapView)Control;
                if (mkMapView != null)
                {
                    mkMapView.CoordinateLongPressed -= CoordinateLongPressed;
                    mkMapView.CoordinateTapped -= CoordinateTapped;
                    mkMapView.CameraPositionChanged -= CameraPositionChanged;
                    mkMapView.DidTapMyLocationButton = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            // For XAML Previewer or FormsGoogleMaps.Init not called.
            if (!FormsGoogleMaps.IsInitialized)
            {
                var label = new UILabel()
                {
                    Text = "Xamarin.Forms.GoogleMaps",
                    BackgroundColor = Xamarin.Forms.Color.Teal.ToUIColor(),
                    TextColor = Xamarin.Forms.Color.Black.ToUIColor(),
                    TextAlignment = UITextAlignment.Center
                };
                SetNativeControl(label);
                return;
            }

            var oldMapView = (MapView)Control;
            if (e.OldElement != null)
            {
                var oldMapModel = (Map)e.OldElement;
                oldMapModel.OnSnapshot -= OnSnapshot;
                _cameraLogic.Unregister();

                if (oldMapView != null)
                {
                    oldMapView.CoordinateLongPressed -= CoordinateLongPressed;
                    oldMapView.CoordinateTapped -= CoordinateTapped;
                    oldMapView.CameraPositionChanged -= CameraPositionChanged;
                    oldMapView.DidTapMyLocationButton = null;
                }
            }

            if (e.NewElement != null)
            {
                var mapModel = (Map)e.NewElement;

                if (Control == null)
                {
                    SetNativeControl(new MapView(RectangleF.Empty));
                    var mkMapView = (MapView)Control;
                    mkMapView.CameraPositionChanged += CameraPositionChanged;
                    mkMapView.CoordinateTapped += CoordinateTapped;
                    mkMapView.CoordinateLongPressed += CoordinateLongPressed;
                    mkMapView.DidTapMyLocationButton = DidTapMyLocation;
                }

                _cameraLogic.Register(Map, NativeMap);
                Map.OnSnapshot += OnSnapshot;

                //_cameraLogic.MoveCamera(mapModel.InitialCameraUpdate);
                //_ready = true;

                _uiSettingsLogic.Register(Map, NativeMap);
                UpdateMapType();
                UpdateIsShowingUser(_uiSettingsLogic.MyLocationButtonEnabled);
                UpdateHasScrollEnabled(_uiSettingsLogic.ScrollGesturesEnabled);
                UpdateHasZoomEnabled(_uiSettingsLogic.ZoomGesturesEnabled);
                UpdateHasRotationEnabled(_uiSettingsLogic.RotateGesturesEnabled);
                UpdateIsTrafficEnabled();
                UpdatePadding();
                UpdateMapStyle();
                //SetMaxZoom();
                UpdateMyLocationEnabled();
                _uiSettingsLogic.Initialize();

                foreach (var logic in _logics)
                {
                    logic.Register(oldMapView, (Map)e.OldElement, NativeMap, Map);
                    logic.RestoreItems();
                    logic.OnMapPropertyChanged(new PropertyChangedEventArgs(Map.SelectedPinProperty.PropertyName));
                }
                if (Map.IsUseCluster)
                {
                    _clusterLogic.Register(oldMapView, (Map)e.OldElement, NativeMap, Map);
                    _clusterLogic.RestoreItems();
                    _clusterLogic.OnMapPropertyChanged(new PropertyChangedEventArgs(Map.SelectedPinProperty.PropertyName));
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            // For XAML Previewer or FormsGoogleMaps.Init not called.
            if (!FormsGoogleMaps.IsInitialized)
            {
                return;
            }

            if (e.PropertyName == Map.MapTypeProperty.PropertyName)
            {
                UpdateMapType();
            }
            else if (e.PropertyName == Map.IsShowingUserProperty.PropertyName)
            {
                UpdateIsShowingUser();
            }
            else if (e.PropertyName == Map.MyLocationEnabledProperty.PropertyName)
            {
                UpdateMyLocationEnabled();
            }
            else if (e.PropertyName == Map.HasScrollEnabledProperty.PropertyName)
            {
                UpdateHasScrollEnabled();
            }
            else if (e.PropertyName == Map.HasRotationEnabledProperty.PropertyName)
            {
                UpdateHasRotationEnabled();
            }
            else if (e.PropertyName == Map.HasZoomEnabledProperty.PropertyName)
            {
                UpdateHasZoomEnabled();
            }
            else if (e.PropertyName == Map.IsTrafficEnabledProperty.PropertyName)
            {
                UpdateIsTrafficEnabled();
            }
            else if (e.PropertyName == VisualElement.HeightProperty.PropertyName &&
                     ((Map)Element).InitialCameraUpdate != null)
            {
                _shouldUpdateRegion = true;
            }
            else if (e.PropertyName == Map.IndoorEnabledProperty.PropertyName)
            {
                UpdateHasIndoorEnabled();
            }
            else if (e.PropertyName == Map.PaddingProperty.PropertyName)
            {
                UpdatePadding();
            }
            //else if (e.PropertyName == Map.MaxZoomLevelProperty.PropertyName)
            //{
            //    SetMaxZoom();
            //}
            //else if (e.PropertyName == Map.MinZoomLevelProperty.PropertyName)
            //{
            //    SetMaxZoom();
            //}
            else if (e.PropertyName == Map.MapStyleProperty.PropertyName)
            {
                UpdateMapStyle();
            }

            foreach (var logic in _logics)
            {
                logic.OnMapPropertyChanged(e);
            }
            if (Map.IsUseCluster)
            {
                _clusterLogic.OnMapPropertyChanged(e);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            // For XAML Previewer or FormsGoogleMaps.Init not called.
            if (!FormsGoogleMaps.IsInitialized)
            {
                return;
            }

            if (_shouldUpdateRegion && !_ready)
            {
                _cameraLogic.MoveCamera(((Map)Element).InitialCameraUpdate);
                _ready = true;
                _shouldUpdateRegion = false;
            }
        }

        private void OnSnapshot(TakeSnapshotMessage snapshotMessage)
        {
            UIGraphics.BeginImageContextWithOptions(NativeMap.Frame.Size, false, 0f);
            NativeMap.Layer.RenderInContext(UIGraphics.GetCurrentContext());
            var snapshot = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            // Why using task? Because Android side is asynchronous.
            Task.Run(() =>
            {
                snapshotMessage.OnSnapshot.Invoke(snapshot.AsPNG().AsStream());
            });
        }

        private void CameraPositionChanged(object sender, GMSCameraEventArgs args)
        {
            OnCameraPositionChanged(args.Position);
        }

        private void OnCameraPositionChanged(GCameraPosition pos)
        {
            if (Element == null)
                return;

            var mapModel = (Map)Element;
            var mkMapView = (MapView)Control;

            var region = mkMapView.Projection.VisibleRegion;
            var minLat = Math.Min(Math.Min(Math.Min(region.NearLeft.Latitude, region.NearRight.Latitude), region.FarLeft.Latitude), region.FarRight.Latitude);
            var minLon = Math.Min(Math.Min(Math.Min(region.NearLeft.Longitude, region.NearRight.Longitude), region.FarLeft.Longitude), region.FarRight.Longitude);
            var maxLat = Math.Max(Math.Max(Math.Max(region.NearLeft.Latitude, region.NearRight.Latitude), region.FarLeft.Latitude), region.FarRight.Latitude);
            var maxLon = Math.Max(Math.Max(Math.Max(region.NearLeft.Longitude, region.NearRight.Longitude), region.FarLeft.Longitude), region.FarRight.Longitude);

#pragma warning disable 618
            mapModel.VisibleRegion = new MapSpan(pos.Target.ToPosition(), maxLat - minLat, maxLon - minLon);
#pragma warning restore 618

            Map.Region = mkMapView.Projection.VisibleRegion.ToRegion();

            var camera = pos.ToXamarinForms();
            Map.CameraPosition = camera;
            Map.SendCameraChanged(camera);
        }

        private void CoordinateTapped(object sender, GMSCoordEventArgs e)
        {
            Map.SendMapClicked(e.Coordinate.ToPosition());
        }

        private void CoordinateLongPressed(object sender, GMSCoordEventArgs e)
        {
            Map.SendMapLongClicked(e.Coordinate.ToPosition());
        }

        private bool DidTapMyLocation(MapView mapView)
        {
            return Map.SendMyLocationClicked();
        }

        private void UpdateHasScrollEnabled(bool? initialScrollGesturesEnabled = null)
        {
#pragma warning disable 618
            NativeMap.Settings.ScrollGestures = initialScrollGesturesEnabled ?? ((Map)Element).HasScrollEnabled;
#pragma warning restore 618
        }

        private void UpdateHasZoomEnabled(bool? initialZoomGesturesEnabled = null)
        {
#pragma warning disable 618
            NativeMap.Settings.ZoomGestures = initialZoomGesturesEnabled ?? ((Map)Element).HasZoomEnabled;
#pragma warning restore 618
        }

        private void UpdateHasRotationEnabled(bool? initialRotateGesturesEnabled = null)
        {
#pragma warning disable 618
            NativeMap.Settings.RotateGestures = initialRotateGesturesEnabled ?? ((Map)Element).HasRotationEnabled;
#pragma warning restore 618
        }

        private void UpdateIsShowingUser(bool? initialMyLocationButtonEnabled = null)
        {
#pragma warning disable 618
            ((MapView)Control).MyLocationEnabled = ((Map)Element).IsShowingUser;
            ((MapView)Control).Settings.MyLocationButton = initialMyLocationButtonEnabled ?? ((Map)Element).IsShowingUser;
#pragma warning restore 618
        }

        private void UpdateMyLocationEnabled()
        {
            ((MapView)Control).MyLocationEnabled = ((Map)Element).MyLocationEnabled;
        }

        private void UpdateIsTrafficEnabled()
        {
            ((MapView)Control).TrafficEnabled = ((Map)Element).IsTrafficEnabled;
        }

        private void UpdateHasIndoorEnabled()
        {
            ((MapView)Control).IndoorEnabled = ((Map)Element).IsIndoorEnabled;
        }

        private void UpdateMapType()
        {
            switch (((Map)Element).MapType)
            {
                case MapType.Street:
                    ((MapView)Control).MapType = MapViewType.Normal;
                    break;

                case MapType.Satellite:
                    ((MapView)Control).MapType = MapViewType.Satellite;
                    break;

                case MapType.Hybrid:
                    ((MapView)Control).MapType = MapViewType.Hybrid;
                    break;

                case MapType.Terrain:
                    ((MapView)Control).MapType = MapViewType.Terrain;
                    break;

                case MapType.None:
                    ((MapView)Control).MapType = MapViewType.None;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdatePadding(Action onFinished = null)
        {
            UIView.Animate(0.4, () =>
            {
                ((MapView)Control).Padding = ((Map)Element).Padding.ToUIEdgeInsets();
            }, completion: onFinished);
        }

        private void UpdateMapStyle()
        {
            if (Map.MapStyle == null)
            {
                ((MapView)Control).MapStyle = null;
            }
            else
            {
                var mapStyle = Google.Maps.MapStyle.FromJson(Map.MapStyle.JsonStyle, null);
                ((MapView)Control).MapStyle = mapStyle;
            }
        }

        private void SetMaxZoom()
        {
            ((MapView)Control).SetMinMaxZoom(((Map)Element).MaxZoomLevel, ((Map)Element).MinZoomLevel);
        }
    }
}