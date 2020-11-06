using System;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public sealed class Pin : BindableObject
    {
        public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(PinType), typeof(Pin), default(PinType));

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(Position), typeof(Pin), default(Position));

        public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(Pin), default(string));

        public static readonly BindableProperty AddressProperty = BindableProperty.Create(nameof(Address), typeof(string), typeof(Pin), default(string));

        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(BitmapDescriptor), typeof(Pin), default(BitmapDescriptor));

        public static readonly BindableProperty IsDraggableProperty = BindableProperty.Create(nameof(IsDraggable), typeof(bool), typeof(Pin), false);

        public static readonly BindableProperty RotationProperty = BindableProperty.Create(nameof(Rotation), typeof(float), typeof(Pin), 0f);

        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(Pin), true);

        public static readonly BindableProperty AnchorProperty = BindableProperty.Create(nameof(Anchor), typeof(Point), typeof(Pin), new Point(0.5d, 1.0d));

        public static readonly BindableProperty FlatProperty = BindableProperty.Create(nameof(Flat), typeof(bool), typeof(Pin), false);

        public static readonly BindableProperty InfoWindowAnchorProperty = BindableProperty.Create(nameof(InfoWindowAnchor), typeof(Point), typeof(Pin), new Point(0.5d, 1.0d));

        public static readonly BindableProperty ZIndexProperty = BindableProperty.Create(nameof(ZIndex), typeof(int), typeof(Pin), 0);

        public static readonly BindableProperty TransparencyProperty = BindableProperty.Create(nameof(Transparency), typeof(float), typeof(Pin), 0f);

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public PinType Type
        {
            get { return (PinType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public BitmapDescriptor Icon
        {
            get { return (BitmapDescriptor)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, value); }
        }

        public float Rotation
        {
            get { return (float)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public Point Anchor
        {
            get { return (Point)GetValue(AnchorProperty); }
            set { SetValue(AnchorProperty, value); }
        }

        public bool Flat
        {
            get { return (bool)GetValue(FlatProperty); }
            set { SetValue(FlatProperty, value); }
        }

        public Point InfoWindowAnchor
        {
            get { return (Point)GetValue(InfoWindowAnchorProperty); }
            set { SetValue(InfoWindowAnchorProperty, value); }
        }

        public int ZIndex
        {
            get { return (int)GetValue(ZIndexProperty); }
            set { SetValue(ZIndexProperty, value); }
        }

        public float Transparency
        {
            get { return (float)GetValue(TransparencyProperty); }
            set { SetValue(TransparencyProperty, value); }
        }

        public object Tag { get; set; }

        public object NativeObject { get; set; }

        public bool IsRunning { get; set; }

        [Obsolete("Please use Map.PinClicked instead of this")]
        public event EventHandler Clicked;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Pin)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Label?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Type;
                hashCode = (hashCode * 397) ^ (Address?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public static bool operator ==(Pin left, Pin right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pin left, Pin right)
        {
            return !Equals(left, right);
        }

        public bool SendTap()
        {
            EventHandler handler = Clicked;
            if (handler == null)
                return false;

            handler(this, EventArgs.Empty);
            return true;
        }

        private bool Equals(Pin other)
        {
            return string.Equals(Label, other.Label) && Equals(Position, other.Position) && Type == other.Type && string.Equals(Address, other.Address);
        }

        public void Rotate(double latitude,
    double longitude,
    Action callback, int duration = 50)
        {
            //gán lại vòng quay
            var mRotateIndex = 0;
            double MAX_ROTATE_STEP = duration / 50;
            // * tính góc quay giữa 2 điểm location
            var angle = GeoHelper.ComputeHeading(this.Position.Latitude, this.Position.Longitude, latitude, longitude);
            if (angle == 0)
            {
                callback();
                return;
            }

            //tính lại độ lệch góc
            var deltaAngle = GeoHelper.GetRotaion(this.Rotation, angle);

            var startRotaion = this.Rotation;

            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                //góc quay tiếp theo
                var fractionAngle = GeoHelper.ComputeRotation(
                                      mRotateIndex / MAX_ROTATE_STEP,
                                      startRotaion,
                                      deltaAngle);
                mRotateIndex = mRotateIndex + 1;

                this.Rotation = (float)fractionAngle;

                if (mRotateIndex > MAX_ROTATE_STEP)
                {
                    callback();
                    return false;
                }

                return true;
            });
        }

        public void MarkerAnimation(double latitude, double longitude, Action callback, int duration = 4000)
        {
            if (this.IsRunning)
            {
                callback();
            }
            else
            {
                IsRunning = true;
                //gán lại vòng quay
                double mMoveIndex = 0;
                double MAX_MOVE_STEP = duration / 100;
                var startPosition = this.Position;

                var finalPosition = new Position(latitude, longitude);
                double elapsed = 0;
                double t;
                double v;

                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    // Calculate progress using interpolator
                    elapsed = elapsed + 100;
                    t = elapsed / duration;
                    v = GeoHelper.GetInterpolation(t);

                    var postionnew = GeoHelper.LinearInterpolator(v,
                        new Position(startPosition.Latitude, startPosition.Longitude),
                        new Position(latitude, longitude));

                    mMoveIndex = mMoveIndex + 1;
                    this.Position = new Position(postionnew.Latitude, postionnew.Longitude);
                    if (mMoveIndex > MAX_MOVE_STEP)
                    {
                        IsRunning = false;
                        callback();
                        return false;
                    }

                    return true;
                });
            }
        }
    }
}