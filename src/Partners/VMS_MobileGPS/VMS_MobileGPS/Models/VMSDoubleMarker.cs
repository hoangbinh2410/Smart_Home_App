using BA_MobileGPS.Core;
using BA_MobileGPS.Entities;

using VMS_MobileGPS.Views;

using Xamarin.Forms;

namespace VMS_MobileGPS
{
    public class VMSDoubleMarker : BaseModel
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

        public VMSDoubleMarker DrawMarker(VehicleOnline vehicle)
        {
            Car = new Pin()
            {
                Position = new Position(vehicle.Lat, vehicle.Lng),
                Label = vehicle.VehiclePlate,
                Anchor = new Point(0.5, 0.5),
                Icon = BitmapDescriptorFactory.FromResource(vehicle.IconImage),
                Tag = vehicle.VehiclePlate,
                Rotation = vehicle.Direction * 45
            };
            Plate = new Pin()
            {
                Position = new Position(vehicle.Lat, vehicle.Lng),
                Label = vehicle.VehiclePlate,
                Icon = BitmapDescriptorFactory.FromView(new VMSPinInfowindowView(vehicle.PrivateCode)),
                Tag = vehicle.VehiclePlate + "Plate"
            };
            return this;
        }
    }
}