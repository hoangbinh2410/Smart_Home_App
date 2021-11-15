using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;

using Xamarin.Forms;

namespace BA_MobileGPS
{
    public class DoubleMarker : BaseModel
    {
        /* Marker xe có biển số */
        public Pin mPlate;

        public Pin Plate
        { get => mPlate; set => SetProperty(ref mPlate, value, nameof(Plate)); }

        /* Marker xe không có biển số */

        private Pin mCar;

        public Pin Car
        { get => mCar; set => SetProperty(ref mCar, value, nameof(Car)); }

        /* Vẽ marker xe có biển số */

        public DoubleMarker DrawMarker(VehicleOnline vehicle)
        {
            Car = new Pin()
            {
                Position = new Position(vehicle.Lat, vehicle.Lng),
                Label = vehicle.VehiclePlate,
                Anchor = new Point(0.5, 0.5),
                Icon = BitmapDescriptorFactory.FromResource(vehicle.IconImage),
                Tag = vehicle.VehiclePlate,
                Rotation = StateVehicleExtension.IsLostGPSIcon(vehicle.GPSTime, vehicle.VehicleTime) ? 0 : vehicle.Direction * 45
            };
            Plate = new Pin()
            {
                Position = new Position(vehicle.Lat, vehicle.Lng),
                Label = vehicle.VehiclePlate,
                Icon = BitmapDescriptorFactory.FromView(new PinInfowindowView(vehicle.PrivateCode)),
                Tag = vehicle.VehiclePlate + "Plate",
                IsVisible = true
            };
            return this;
        }
    }
}