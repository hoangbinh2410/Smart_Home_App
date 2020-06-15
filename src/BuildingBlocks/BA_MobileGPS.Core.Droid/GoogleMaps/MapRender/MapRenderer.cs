using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Core.Droid.Extensions;
using BA_MobileGPS.Core.Droid.Logics;
using BA_MobileGPS.Core.Internals;
using BA_MobileGPS.Core.Logics;

using System;
using System.ComponentModel;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Color = Xamarin.Forms.Color;
using Math = System.Math;

[assembly: ExportRenderer(typeof(Map), typeof(MapRenderer))]

namespace BA_MobileGPS.Core.Droid
{
    public class MapRenderer : ViewRenderer<Map, global::Android.Views.View>,
        Android.Gms.Maps.GoogleMap.IOnMapClickListener,
        Android.Gms.Maps.GoogleMap.IOnMapLongClickListener,
        Android.Gms.Maps.GoogleMap.IOnMyLocationButtonClickListener
    {
        private readonly CameraLogic _cameraLogic;
        private readonly ClusterLogic _clusterLogic;
        private readonly UiSettingsLogic _uiSettingsLogic = new UiSettingsLogic();
        private readonly BaseLogic<Android.Gms.Maps.GoogleMap>[] _logics;

        public MapRenderer(Context context) : base(context)
        {
            _cameraLogic = new CameraLogic(UpdateVisibleRegion);
            _clusterLogic = new ClusterLogic(context, Config.BitmapDescriptorFactory);

            AutoPackage = false;
            _logics = new BaseLogic<Android.Gms.Maps.GoogleMap>[]
            {
                new PolylineLogic(),
                new PolygonLogic(),
                new CircleLogic(),
                new PinLogic(context, Config.BitmapDescriptorFactory),
                new GroundOverlayLogic(context, Config.BitmapDescriptorFactory)
                //new ClusterLogic(context, Config.BitmapDescriptorFactory)
            };
        }

        private static Bundle s_bundle;

        public static Bundle Bundle { set { s_bundle = value; } }

        public static PlatformConfig Config { private get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        protected Android.Gms.Maps.GoogleMap NativeMap { get; private set; }

        // ReSharper disable once MemberCanBePrivate.Global
        protected Map Map => Element;

        private bool _ready = false;
        private bool _onLayout = false;

        private float _scaledDensity;

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(Context.ToPixels(40), Context.ToPixels(40)));
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == e.NewElement)
            {
                return;
            }

            //// For XAML Previewer or FormsGoogleMaps.Init not called.
            if (!FormsGoogleMaps.IsInitialized)
            {
                var tv = new TextView(Context)
                {
                    Gravity = GravityFlags.Center,
                    Text = "BA_MobileGPS.Core"
                };
                tv.SetBackgroundColor(Color.Teal.ToAndroid());
                tv.SetTextColor(Color.Black.ToAndroid());
                SetNativeControl(tv);
                return;
            }

            // Uninitialize old view
            Android.Gms.Maps.GoogleMap oldNativeMap = null;
            Map oldMap = null;
            if (e.OldElement != null)
            {
                try
                {
                    var oldNativeView = Control as MapView;
                    // ReSharper disable once PossibleNullReferenceException
                    oldNativeMap = await oldNativeView?.GetGoogleMapAsync();
                    oldMap = e.OldElement;
                    Uninitialize(oldNativeMap, oldMap);
                    oldNativeView?.Dispose();
                }
                catch (System.Exception ex)
                {
                    var message = ex.Message;
                    System.Diagnostics.Debug.WriteLine($"Uninitialize old view failed. - {message}");
                }
            }

            if (e.NewElement == null)
            {
                return;
            }

            var mapView = new MapView(Context);
            mapView.OnCreate(s_bundle);
            mapView.OnResume();
            SetNativeControl(mapView);

            var activity = Context as Activity;
            if (activity != null)
            {
                _scaledDensity = activity.GetScaledDensity();
                _cameraLogic.ScaledDensity = _scaledDensity;
                foreach (var logic in _logics)
                {
                    logic.ScaledDensity = _scaledDensity;
                }
            }

            var newMap = e.NewElement;
            NativeMap = await mapView.GetGoogleMapAsync();

            foreach (var logic in _logics)
            {
                logic.Register(oldNativeMap, oldMap, NativeMap, newMap);
                logic.ScaledDensity = _scaledDensity;
            }
            if (Map.IsUseCluster)
            {
                _clusterLogic.Register(oldNativeMap, oldMap, NativeMap, newMap);
                _clusterLogic.ScaledDensity = _scaledDensity;
            }

            OnMapReady(NativeMap, newMap);
        }

        private void OnSnapshot(TakeSnapshotMessage snapshotMessage)
        {
            NativeMap.Snapshot(new DelegateSnapshotReadyCallback(snapshot =>
            {
                var stream = new MemoryStream();
                snapshot.Compress(Bitmap.CompressFormat.Png, 0, stream);
                stream.Position = 0;
                snapshotMessage?.OnSnapshot?.Invoke(stream);
            }));
        }

        protected virtual void OnMapReady(Android.Gms.Maps.GoogleMap nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                _cameraLogic.Register(map, nativeMap);

                _uiSettingsLogic.Register(map, nativeMap);
                Map.OnSnapshot += OnSnapshot;

                nativeMap.SetOnMapClickListener(this);
                nativeMap.SetOnMapLongClickListener(this);
                nativeMap.SetOnMyLocationButtonClickListener(this);

                UpdateIsShowingUser(_uiSettingsLogic.MyLocationButtonEnabled);
                UpdateHasScrollEnabled(_uiSettingsLogic.ScrollGesturesEnabled);
                UpdateHasZoomEnabled(_uiSettingsLogic.ZoomControlsEnabled, _uiSettingsLogic.ZoomGesturesEnabled);
                UpdateHasRotationEnabled(_uiSettingsLogic.RotateGesturesEnabled);
                UpdateIsTrafficEnabled();
                UpdateIndoorEnabled();
                UpdateMapStyle();
                UpdateMyLocationEnabled();
                _uiSettingsLogic.Initialize();

                SetMapType();
                SetPadding();

                //UpdateMaxZoom();

                //UpdateMinZoom();
            }

            _ready = true;
            if (_ready && _onLayout)
            {
                InitializeLogic();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            // For XAML Previewer or FormsGoogleMaps.Init not called.
            if (!FormsGoogleMaps.IsInitialized)
            {
                return;
            }

            _onLayout = true;

            if (_ready && _onLayout)
            {
                InitializeLogic();
            }
            else if (changed && NativeMap != null)
            {
                UpdateVisibleRegion(NativeMap.CameraPosition.Target);
            }
        }

        private void InitializeLogic()
        {
            _cameraLogic.MoveCamera(Map.InitialCameraUpdate);

            foreach (var logic in _logics)
            {
                if (logic.Map != null)
                {
                    logic.RestoreItems();
                    logic.OnMapPropertyChanged(new PropertyChangedEventArgs(Map.SelectedPinProperty.PropertyName));
                }
            }
            if (Map.IsUseCluster)
            {
                _clusterLogic.RestoreItems();
                _clusterLogic.OnMapPropertyChanged(new PropertyChangedEventArgs(Map.SelectedPinProperty.PropertyName));
            }

            _ready = false;
            _onLayout = false;
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
                SetMapType();
                return;
            }

            if (e.PropertyName == Map.PaddingProperty.PropertyName)
            {
                SetPadding();
                return;
            }

            if (NativeMap == null)
            {
                return;
            }

            if (e.PropertyName == Map.IsShowingUserProperty.PropertyName)
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
            else if (e.PropertyName == Map.HasZoomEnabledProperty.PropertyName)
            {
                UpdateHasZoomEnabled();
            }
            else if (e.PropertyName == Map.HasRotationEnabledProperty.PropertyName)
            {
                UpdateHasRotationEnabled();
            }
            else if (e.PropertyName == Map.IsTrafficEnabledProperty.PropertyName)
            {
                UpdateIsTrafficEnabled();
            }
            else if (e.PropertyName == Map.IndoorEnabledProperty.PropertyName)
            {
                UpdateIndoorEnabled();
            }
            else if (e.PropertyName == Map.MapStyleProperty.PropertyName)
            {
                UpdateMapStyle();
            }

            foreach (var logic in _logics)
            {
                logic.OnMapPropertyChanged(e);
            }
        }

        private void UpdateIsShowingUser(bool? initialMyLocationButtonEnabled = null)
        {
#pragma warning disable 618
            NativeMap.MyLocationEnabled = Map.IsShowingUser;
            NativeMap.UiSettings.MyLocationButtonEnabled = initialMyLocationButtonEnabled ?? Map.IsShowingUser;
#pragma warning restore 618
        }

        private void UpdateMyLocationEnabled()
        {
            NativeMap.MyLocationEnabled = Map.MyLocationEnabled;
        }

        private void UpdateHasScrollEnabled(bool? initialScrollGesturesEnabled = null)
        {
#pragma warning disable 618
            NativeMap.UiSettings.ScrollGesturesEnabled = initialScrollGesturesEnabled ?? Map.HasScrollEnabled;
#pragma warning restore 618
        }

        private void UpdateHasZoomEnabled(
            bool? initialZoomControlsEnabled = null,
            bool? initialZoomGesturesEnabled = null)
        {
#pragma warning disable 618
            NativeMap.UiSettings.ZoomControlsEnabled = initialZoomControlsEnabled ?? Map.HasZoomEnabled;
            NativeMap.UiSettings.ZoomGesturesEnabled = initialZoomGesturesEnabled ?? Map.HasZoomEnabled;
#pragma warning restore 618
        }

        private void UpdateHasRotationEnabled(bool? initialRotateGesturesEnabled = null)
        {
#pragma warning disable 618
            NativeMap.UiSettings.RotateGesturesEnabled = initialRotateGesturesEnabled ?? Map.HasRotationEnabled;
#pragma warning restore 618
        }

        private void UpdateIsTrafficEnabled()
        {
            NativeMap.TrafficEnabled = Map.IsTrafficEnabled;
        }

        private void UpdateIndoorEnabled()
        {
            NativeMap.SetIndoorEnabled(Map.IsIndoorEnabled);
        }

        private void UpdateMapStyle()
        {
            NativeMap.SetMapStyle(Map.MapStyle != null ?
                new MapStyleOptions(Map.MapStyle.JsonStyle) :
                null);
        }

        private void UpdateMaxZoom()
        {
            NativeMap.SetMaxZoomPreference(Map.MaxZoomLevel);
        }

        private void UpdateMinZoom()
        {
            NativeMap.SetMinZoomPreference(Map.MinZoomLevel);
        }

        private void SetMapType()
        {
            var map = NativeMap;
            if (map == null)
                return;

            switch (Map.MapType)
            {
                case MapType.Street:
                    map.MapType = Android.Gms.Maps.GoogleMap.MapTypeNormal;
                    break;

                case MapType.Satellite:
                    map.MapType = Android.Gms.Maps.GoogleMap.MapTypeSatellite;
                    break;

                case MapType.Hybrid:
                    map.MapType = Android.Gms.Maps.GoogleMap.MapTypeHybrid;
                    break;

                case MapType.Terrain:
                    map.MapType = Android.Gms.Maps.GoogleMap.MapTypeTerrain;
                    break;

                case MapType.None:
                    map.MapType = Android.Gms.Maps.GoogleMap.MapTypeNone;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetPadding()
        {
            NativeMap?.SetPadding(
                (int)(Map.Padding.Left * _scaledDensity),
                (int)(Map.Padding.Top * _scaledDensity),
                (int)(Map.Padding.Right * _scaledDensity),
                (int)(Map.Padding.Bottom * _scaledDensity));
        }

        public void OnMapClick(LatLng point)
        {
            Map.SendMapClicked(point.ToPosition());
        }

        public void OnMapLongClick(LatLng point)
        {
            Map.SendMapLongClicked(point.ToPosition());
        }

        public bool OnMyLocationButtonClick()
        {
            return Map.SendMyLocationClicked();
        }

        private void UpdateVisibleRegion(LatLng pos)
        {
            var map = NativeMap;
            if (map == null)
                return;
            var projection = map.Projection;
            var width = Control.Width;
            var height = Control.Height;
            var ul = projection.FromScreenLocation(new global::Android.Graphics.Point(0, 0));
            var ur = projection.FromScreenLocation(new global::Android.Graphics.Point(width, 0));
            var ll = projection.FromScreenLocation(new global::Android.Graphics.Point(0, height));
            var lr = projection.FromScreenLocation(new global::Android.Graphics.Point(width, height));
            var dlat = Math.Max(Math.Abs(ul.Latitude - lr.Latitude), Math.Abs(ur.Latitude - ll.Latitude));
            var dlong = Math.Max(Math.Abs(ul.Longitude - lr.Longitude), Math.Abs(ur.Longitude - ll.Longitude));
#pragma warning disable 618
            Element.VisibleRegion = new MapSpan(
                    new Position(
                        pos.Latitude,
                        pos.Longitude
                    ),
                dlat,
                dlong
            );
#pragma warning restore 618
            Element.Region = projection.VisibleRegion.ToRegion();
        }

        private void Uninitialize(Android.Gms.Maps.GoogleMap nativeMap, Map map)
        {
            try
            {
                if (nativeMap == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Uninitialize failed - {nameof(nativeMap)} is null");
                    return;
                }

                if (map == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Uninitialize failed - {nameof(map)} is null");
                    return;
                }

                _uiSettingsLogic.Unregister();

                map.OnSnapshot -= OnSnapshot;
                _cameraLogic.Unregister();

                foreach (var logic in _logics)
                {
                    logic.Unregister(nativeMap, map);
                }
                if (Map.IsUseCluster)
                {
                    _clusterLogic.Unregister(nativeMap, map);
                }

                nativeMap.SetOnMapClickListener(null);
                nativeMap.SetOnMapLongClickListener(null);
                nativeMap.SetOnMyLocationButtonClickListener(null);

                nativeMap.MyLocationEnabled = false;
                nativeMap.Dispose();
            }
            catch (System.Exception ex)
            {
                var message = ex.Message;
                System.Diagnostics.Debug.WriteLine($"Uninitialize failed. - {message}");
            }
        }

        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                Uninitialize(NativeMap, Map);

                if (NativeMap != null)
                {
                    NativeMap.Dispose();
                    NativeMap = null;
                }

                (Control as MapView)?.OnDestroy();
            }

            base.Dispose(disposing);
        }
    }
}