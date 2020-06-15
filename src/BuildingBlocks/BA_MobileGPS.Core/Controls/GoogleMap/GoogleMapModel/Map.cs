using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Internals;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class Map : View, IEnumerable<Pin>
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(IEnumerable), typeof(IEnumerable), typeof(Map), default(IEnumerable),
            propertyChanged: (b, o, n) => ((Map)b).OnItemsSourcePropertyChanged((IEnumerable)o, (IEnumerable)n));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(Map), default(DataTemplate),
            propertyChanged: (b, o, n) => ((Map)b).OnItemTemplatePropertyChanged((DataTemplate)o, (DataTemplate)n));

        public static readonly BindableProperty MapTypeProperty = BindableProperty.Create(nameof(MapType), typeof(MapType), typeof(Map), default(MapType));

#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly BindableProperty IsShowingUserProperty = BindableProperty.Create(nameof(IsShowingUser), typeof(bool), typeof(Map), default(bool));

        public static readonly BindableProperty MyLocationEnabledProperty = BindableProperty.Create(nameof(MyLocationEnabled), typeof(bool), typeof(Map), default(bool));

        public static readonly BindableProperty HasScrollEnabledProperty = BindableProperty.Create(nameof(HasScrollEnabled), typeof(bool), typeof(Map), true);

        public static readonly BindableProperty HasZoomEnabledProperty = BindableProperty.Create(nameof(HasZoomEnabled), typeof(bool), typeof(Map), true);

        public static readonly BindableProperty HasRotationEnabledProperty = BindableProperty.Create(nameof(HasRotationEnabled), typeof(bool), typeof(Map), true);
#pragma warning restore CS0618 // Type or member is obsolete

        public static readonly BindableProperty SelectedPinProperty = BindableProperty.Create(nameof(SelectedPin), typeof(Pin), typeof(Map), default(Pin), defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty IsTrafficEnabledProperty = BindableProperty.Create(nameof(IsTrafficEnabled), typeof(bool), typeof(Map), false);

        public static readonly BindableProperty IndoorEnabledProperty = BindableProperty.Create(nameof(IsIndoorEnabled), typeof(bool), typeof(Map), true);

        public static readonly BindableProperty InitialCameraUpdateProperty = BindableProperty.Create(
            nameof(InitialCameraUpdate), typeof(CameraUpdate), typeof(Map),
            CameraUpdateFactory.NewPositionZoom(new Position(41.89, 12.49), 10),  // center on Rome by default
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((Map)bindable)._useMoveToRegisonAsInitialBounds = false;
            });

        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(PaddingProperty), typeof(Thickness), typeof(Map), default(Thickness));

        public static readonly BindableProperty MaxZoomLevelProperty = BindableProperty.Create(nameof(MaxZoomLevelProperty), typeof(float), typeof(Map), default(float));

        public static readonly BindableProperty MinZoomLevelProperty = BindableProperty.Create(nameof(MaxZoomLevelProperty), typeof(float), typeof(Map), default(float));

        private bool _useMoveToRegisonAsInitialBounds = true;

        public static readonly BindableProperty CameraPositionProperty = BindableProperty.Create(
            nameof(CameraPosition), typeof(CameraPosition), typeof(Map),
            defaultValueCreator: (bindable) => new CameraPosition(((Map)bindable).InitialCameraUpdate.Position, 10),
            defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty MapStyleProperty = BindableProperty.Create(nameof(MapStyle), typeof(MapStyle), typeof(Map), null);

        private readonly ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        private readonly ObservableCollection<Polyline> _polylines = new ObservableCollection<Polyline>();
        private readonly ObservableCollection<Polygon> _polygons = new ObservableCollection<Polygon>();
        private readonly ObservableCollection<GroundOverlay> _groundOverlays = new ObservableCollection<GroundOverlay>();
        private readonly ObservableCollection<Circle> _circles = new ObservableCollection<Circle>();

        public event EventHandler<PinClickedEventArgs> PinClicked;

        public event EventHandler<SelectedPinChangedEventArgs> SelectedPinChanged;

        public event EventHandler<InfoWindowClickedEventArgs> InfoWindowClicked;

        public event EventHandler<InfoWindowLongClickedEventArgs> InfoWindowLongClicked;

        public event EventHandler<PinDragEventArgs> PinDragStart;

        public event EventHandler<PinDragEventArgs> PinDragEnd;

        public event EventHandler<PinDragEventArgs> PinDragging;

        public event EventHandler<MapClickedEventArgs> MapClicked;

        public event EventHandler<MapLongClickedEventArgs> MapLongClicked;

        public event EventHandler<MyLocationButtonClickedEventArgs> MyLocationButtonClicked;

        [Obsolete("Please use Map.CameraIdled instead of this")]
        public event EventHandler<CameraChangedEventArgs> CameraChanged;

        public event EventHandler<CameraMoveStartedEventArgs> CameraMoveStarted;

        public event EventHandler<CameraMovingEventArgs> CameraMoving;

        public event EventHandler<CameraIdledEventArgs> CameraIdled;

        public Action<MoveToRegionMessage> OnMoveToRegion { get; set; }

        public Action<CameraUpdateMessage> OnMoveCamera { get; set; }

        public Action<CameraUpdateMessage> OnAnimateCamera { get; set; }

        public Action<TakeSnapshotMessage> OnSnapshot { get; set; }

        private MapSpan _visibleRegion;
        private MapRegion _region;

        //// Simone Marra
        //public static Position _TopLeft = new Position();
        //public static Position _TopRight = new Position();
        //public static Position _BottomLeft = new Position();
        //public static Position _BottomRight = new Position();
        //// End Simone Marra

        public Map()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;

            ClusterOptions = new ClusterOptions();

            _clusteredPins.CollectionChanged += (sender, args) =>
            {
                if (args.OldItems != null)
                {
                    foreach (INotifyPropertyChanged item in args.OldItems)
                        item.PropertyChanged -= MarkerPropertyChanged;
                }

                if (args.NewItems != null)
                {
                    foreach (INotifyPropertyChanged item in args.NewItems)
                        item.PropertyChanged += MarkerPropertyChanged;
                }
            };

            _pins.CollectionChanged += PinsOnCollectionChanged;
            _polylines.CollectionChanged += PolylinesOnCollectionChanged;
            _polygons.CollectionChanged += PolygonsOnCollectionChanged;
            _groundOverlays.CollectionChanged += GroundOverlays_CollectionChanged;
            _circles.CollectionChanged += CirclesOnCollectionChanged;
        }

        [Obsolete("Please use Map.UiSettings.ScrollGesturesEnabled instead of this")]
        public bool HasScrollEnabled
        {
            get { return (bool)GetValue(HasScrollEnabledProperty); }
            set { SetValue(HasScrollEnabledProperty, value); }
        }

        [Obsolete("Please use Map.UiSettings.ZoomGesturesEnabled and ZoomControlsEnabled instead of this")]
        public bool HasZoomEnabled
        {
            get { return (bool)GetValue(HasZoomEnabledProperty); }
            set { SetValue(HasZoomEnabledProperty, value); }
        }

        [Obsolete("Please use Map.UiSettings.RotateGesturesEnabled instead of this")]
        public bool HasRotationEnabled
        {
            get { return (bool)GetValue(HasRotationEnabledProperty); }
            set { SetValue(HasRotationEnabledProperty, value); }
        }

        public bool IsTrafficEnabled
        {
            get { return (bool)GetValue(IsTrafficEnabledProperty); }
            set { SetValue(IsTrafficEnabledProperty, value); }
        }

        public bool IsIndoorEnabled
        {
            get { return (bool)GetValue(IndoorEnabledProperty); }
            set { SetValue(IndoorEnabledProperty, value); }
        }

        [Obsolete("Please use Map.MyLocationEnabled and Map.UiSettings.MyLocationButtonEnabled instead of this")]
        public bool IsShowingUser
        {
            get { return (bool)GetValue(IsShowingUserProperty); }
            set { SetValue(IsShowingUserProperty, value); }
        }

        public bool MyLocationEnabled
        {
            get { return (bool)GetValue(MyLocationEnabledProperty); }
            set { SetValue(MyLocationEnabledProperty, value); }
        }

        public MapType MapType
        {
            get { return (MapType)GetValue(MapTypeProperty); }
            set { SetValue(MapTypeProperty, value); }
        }

        public Pin SelectedPin
        {
            get { return (Pin)GetValue(SelectedPinProperty); }
            set { SetValue(SelectedPinProperty, value); }
        }

        [Xamarin.Forms.TypeConverter(typeof(CameraUpdateConverter))]
        public CameraUpdate InitialCameraUpdate
        {
            get { return (CameraUpdate)GetValue(InitialCameraUpdateProperty); }
            set { SetValue(InitialCameraUpdateProperty, value); }
        }

        public CameraPosition CameraPosition
        {
            get { return (CameraPosition)GetValue(CameraPositionProperty); }
            set { SetValue(CameraPositionProperty, value); }
        }

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public float MaxZoomLevel
        {
            get { return (float)GetValue(MaxZoomLevelProperty); }
            set { SetValue(MaxZoomLevelProperty, value); }
        }

        public float MinZoomLevel
        {
            get { return (float)GetValue(MinZoomLevelProperty); }
            set { SetValue(MinZoomLevelProperty, value); }
        }

        public MapStyle MapStyle
        {
            get { return (MapStyle)GetValue(MapStyleProperty); }
            set { SetValue(MapStyleProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public IList<Pin> Pins
        {
            get { return _pins; }
        }

        public IList<Polyline> Polylines
        {
            get { return _polylines; }
        }

        public IList<Polygon> Polygons
        {
            get { return _polygons; }
        }

        public IList<GroundOverlay> GroundOverlays
        {
            get { return _groundOverlays; }
        }

        public IList<Circle> Circles
        {
            get { return _circles; }
        }

        [Obsolete("Please use Map.Region instead of this")]
        public MapSpan VisibleRegion
        {
            get { return _visibleRegion; }
            set
            {
                if (_visibleRegion == value)
                    return;
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                OnPropertyChanging();
                _visibleRegion = value;
                OnPropertyChanged();
            }
        }

        public MapRegion Region
        {
            get { return _region; }
            set
            {
                if (_region == value)
                    return;
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                OnPropertyChanging();
                _region = value;
                OnPropertyChanged();
            }
        }

        public UiSettings UiSettings { get; } = new UiSettings();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Pin> GetEnumerator()
        {
            return _pins.GetEnumerator();
        }

        public void MoveToRegion(MapSpan mapSpan, bool animate = true)
        {
            if (mapSpan == null)
                throw new ArgumentNullException(nameof(mapSpan));

            if (_useMoveToRegisonAsInitialBounds)
            {
                InitialCameraUpdate = CameraUpdateFactory.NewBounds(mapSpan.ToBounds(), 0);
                _useMoveToRegisonAsInitialBounds = false;
            }

            SendMoveToRegion(new MoveToRegionMessage(mapSpan, animate));
        }

        public Task<AnimationStatus> MoveCamera(CameraUpdate cameraUpdate)
        {
            var comp = new TaskCompletionSource<AnimationStatus>();

            SendMoveCamera(new CameraUpdateMessage(cameraUpdate, null, new DelegateAnimationCallback(
                () => comp.SetResult(AnimationStatus.Finished),
                () => comp.SetResult(AnimationStatus.Canceled))));

            return comp.Task;
        }

        public Task<AnimationStatus> AnimateCamera(CameraUpdate cameraUpdate, TimeSpan? duration = null)
        {
            var comp = new TaskCompletionSource<AnimationStatus>();

            SendAnimateCamera(new CameraUpdateMessage(cameraUpdate, duration, new DelegateAnimationCallback(
                () => comp.SetResult(AnimationStatus.Finished),
                () => comp.SetResult(AnimationStatus.Canceled))));

            return comp.Task;
        }

        public Task<Stream> TakeSnapshot()
        {
            var comp = new TaskCompletionSource<Stream>();

            SendTakeSnapshot(new TakeSnapshotMessage(image => comp.SetResult(image)));

            return comp.Task;
        }

        private void PinsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Cast<Pin>().Any(pin => pin.Label == null))
                throw new ArgumentException("Pin must have a Label to be added to a map");
        }

        private void PolylinesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Cast<Polyline>().Any(polyline => polyline.Positions.Count < 2))
                throw new ArgumentException("Polyline must have a 2 positions to be added to a map");
        }

        private void PolygonsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Cast<Polygon>().Any(polygon => polygon.Positions.Count < 3))
                throw new ArgumentException("Polygon must have a 3 positions to be added to a map");
        }

        private void GroundOverlays_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void CirclesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Cast<Circle>().Any(circle => (
                circle?.Center == null || circle?.Radius == null || circle.Radius.Meters <= 0f)))
                throw new ArgumentException("Circle must have a center and radius");
        }

        public void SendSelectedPinChanged(Pin selectedPin)
        {
            SelectedPinChanged?.Invoke(this, new SelectedPinChangedEventArgs(selectedPin));
        }

        public bool SendPinClicked(Pin pin)
        {
            var args = new PinClickedEventArgs(pin);
            PinClicked?.Invoke(this, args);
            return args.Handled;
        }

        public void SendInfoWindowClicked(Pin pin)
        {
            var args = new InfoWindowClickedEventArgs(pin);
            InfoWindowClicked?.Invoke(this, args);
        }

        public void SendInfoWindowLongClicked(Pin pin)
        {
            var args = new InfoWindowLongClickedEventArgs(pin);
            InfoWindowLongClicked?.Invoke(this, args);
        }

        public void SendPinDragStart(Pin pin)
        {
            PinDragStart?.Invoke(this, new PinDragEventArgs(pin));
        }

        public void SendPinDragEnd(Pin pin)
        {
            PinDragEnd?.Invoke(this, new PinDragEventArgs(pin));
        }

        public void SendPinDragging(Pin pin)
        {
            PinDragging?.Invoke(this, new PinDragEventArgs(pin));
        }

        public void SendMapClicked(Position point)
        {
            MapClicked?.Invoke(this, new MapClickedEventArgs(point));
        }

        public void SendMapLongClicked(Position point)
        {
            MapLongClicked?.Invoke(this, new MapLongClickedEventArgs(point));
        }

        public bool SendMyLocationClicked()
        {
            var args = new MyLocationButtonClickedEventArgs();
            MyLocationButtonClicked?.Invoke(this, args);
            return args.Handled;
        }

        public void SendCameraChanged(CameraPosition position)
        {
            CameraChanged?.Invoke(this, new CameraChangedEventArgs(position));
        }

        public void SendCameraMoveStarted(bool isGesture)
        {
            CameraMoveStarted?.Invoke(this, new CameraMoveStartedEventArgs(isGesture));
        }

        public void SendCameraMoving(CameraPosition position)
        {
            CameraMoving?.Invoke(this, new CameraMovingEventArgs(position));
        }

        public void SendCameraIdled(CameraPosition position)
        {
            CameraIdled?.Invoke(this, new CameraIdledEventArgs(position));
        }

        private void SendMoveToRegion(MoveToRegionMessage message)
        {
            OnMoveToRegion?.Invoke(message);
        }

        private void SendMoveCamera(CameraUpdateMessage message)
        {
            OnMoveCamera?.Invoke(message);
        }

        private void SendAnimateCamera(CameraUpdateMessage message)
        {
            OnAnimateCamera?.Invoke(message);
        }

        private void SendTakeSnapshot(TakeSnapshotMessage message)
        {
            OnSnapshot?.Invoke(message);
        }

        private void OnItemsSourcePropertyChanged(IEnumerable oldItemsSource, IEnumerable newItemsSource)
        {
            if (oldItemsSource is INotifyCollectionChanged ncc)
            {
                ncc.CollectionChanged -= OnItemsSourceCollectionChanged;
            }

            if (newItemsSource is INotifyCollectionChanged ncc1)
            {
                ncc1.CollectionChanged += OnItemsSourceCollectionChanged;
            }

            _pins.Clear();
            CreatePinItems();
        }

        private void OnItemTemplatePropertyChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
        {
            if (newItemTemplate is DataTemplateSelector)
            {
                throw new NotSupportedException($"You are using an instance of {nameof(DataTemplateSelector)} to set the {nameof(Map)}.{ItemTemplateProperty.PropertyName} property. Use an instance of a {nameof(DataTemplate)} property instead to set an item template.");
            }

            _pins.Clear();
            CreatePinItems();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == -1)
                        goto case NotifyCollectionChangedAction.Reset;
                    foreach (object item in e.NewItems)
                        CreatePin(item);
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex == -1 || e.NewStartingIndex == -1)
                        goto case NotifyCollectionChangedAction.Reset;
                    // Not tracking order
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == -1)
                        goto case NotifyCollectionChangedAction.Reset;
                    foreach (object item in e.OldItems)
                        RemovePin(item);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex == -1)
                        goto case NotifyCollectionChangedAction.Reset;
                    foreach (object item in e.OldItems)
                        RemovePin(item);
                    foreach (object item in e.NewItems)
                        CreatePin(item);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _pins.Clear();
                    break;
            }
        }

        private void CreatePinItems()
        {
            if (ItemsSource == null || ItemTemplate == null)
            {
                return;
            }

            foreach (object item in ItemsSource)
            {
                CreatePin(item);
            }
        }

        private void CreatePin(object newItem)
        {
            if (ItemTemplate == null)
            {
                return;
            }

            var pin = (Pin)ItemTemplate.CreateContent();
            pin.BindingContext = newItem;
            _pins.Add(pin);
        }

        private void RemovePin(object itemToRemove)
        {
            Pin pinToRemove = _pins.FirstOrDefault(pin => pin.BindingContext?.Equals(itemToRemove) == true);
            if (pinToRemove != null)
            {
                _pins.Remove(pinToRemove);
            }
        }

        #region Cluster

        public static readonly BindableProperty ClusterOptionsProperty = BindableProperty.Create(nameof(ClusterOptionsProperty),
           typeof(ClusterOptions),
           typeof(Map),
           default(ClusterOptions));

        public event EventHandler<ClusterClickedEventArgs> ClusterClicked;

        private readonly ObservableCollection<Pin> _clusteredPins = new ObservableCollection<Pin>();

        public static readonly BindableProperty IsUseClusterProperty = BindableProperty.Create(nameof(IsUseClusterProperty),
         typeof(bool),
         typeof(Map),
         default(bool));

        private void MarkerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != Pin.PositionProperty.PropertyName)
                OnMarkerUpdate?.Invoke((Pin)sender);
        }

        public bool IsUseCluster
        {
            get { return (bool)GetValue(IsUseClusterProperty); }
            set { SetValue(IsUseClusterProperty, value); }
        }

        public Action OnCluster { get; set; }

        public Action<Pin> OnMarkerUpdate { get; set; }

        public bool PendingClusterRequest { get; set; }

        public ClusterOptions ClusterOptions
        {
            get => (ClusterOptions)GetValue(ClusterOptionsProperty);
            set => SetValue(ClusterOptionsProperty, value);
        }

        public void Cluster()
        {
            SendCluster();
        }

        public IList<Pin> ClusteredPins => _clusteredPins;

        private void SendCluster()
        {
            if (OnCluster != null)
            {
                OnCluster.Invoke();
            }
            else
            {
                PendingClusterRequest = true;
            }
        }

        public bool SendClusterClicked(int itemsCount, IEnumerable<Pin> pins, Position position)
        {
            var args = new ClusterClickedEventArgs(itemsCount, pins, position);
            ClusterClicked?.Invoke(this, args);
            return args.Handled;
        }

        #endregion Cluster
    }
}