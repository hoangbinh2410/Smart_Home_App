using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class DoubleMarkerRoute : BaseModel
    {
        /* Marker xe có biển số */
        public Pin mPlate;

        public Pin Plate
        { get => mPlate; set => SetProperty(ref mPlate, value, nameof(Plate)); }

        /* Marker xe không có biển số */

        private Pin mCar;

        public Pin Car
        { get => mCar; set => SetProperty(ref mCar, value, nameof(Car)); }

        /** Hướng của marker */
        public float Direction;

        /** Vị trí của marker */
        public Position Position;

        public string Label;

        /* Vẽ marker xe có biển số */

        public DoubleMarkerRoute InitDoubleMarkerRoute(double fromLat, double fromLng, double toLat, double toLng, string label)
        {
            // Tính hướng
            Direction = (float)GeoHelper.ComputeHeading(fromLat, fromLng, toLat, toLng);
            // Vị trí marker hướng
            Position = new Position(fromLat, fromLng);

            Label = label;
            return this;
        }

        public DoubleMarkerRoute DrawMarker(VehicleRoute route)
        {
            Car = new Pin()
            {
                Position = Position,
                Label = Label,
                Anchor = new Point(0.5, 0.5),
                ZIndex = 2,
                Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                Tag = Label + "Route",
                Rotation = Direction
            };
            if (StateVehicleExtension.IsEngineOff(route.State) || route.StateType?.State == StateType.Stop)
            {
                Car.Icon = BitmapDescriptorFactory.FromResource("car_grey.png");
            }
            else if (StateVehicleExtension.IsOverVelocityRoute(route.Velocity))
            {
                Car.Icon = BitmapDescriptorFactory.FromResource("car_red.png");
            }
            else
            {
                Car.Icon = BitmapDescriptorFactory.FromResource("car_blue.png");
            }
            Plate = new Pin()
            {
                Position = Position,
                Label = Label,
                Icon = BitmapDescriptorFactory.FromView(new PinInfowindowActiveView(Label)),
                ZIndex = 3,
                Tag = Label + "PlateRoute"
            };
            return this;
        }
    }
}