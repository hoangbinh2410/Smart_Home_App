using Xamarin.Forms;

namespace BA_MobileGPS.Core.Models
{
    public class DirectionMarker
    {
        /** Vị trí của marker */
        public Position Position;

        /** Hướng của marker */
        public float Direction;


        public string Label;

        public DirectionMarker InitDirectionPoint(double fromLat, double fromLng, double toLat, double toLng, string label)
        {
            // Tính hướng
            Direction = (float)GeoHelper.ComputeHeading(fromLat, fromLng, toLat, toLng);
            // Vị trí marker hướng
            Position = new Position((fromLat + toLat) / 2, (fromLng + toLng) / 2);

            Label = label;
            return this;
        }

        /* Vẽ điểm marker hướng */

        public Pin DrawPointDiretion()
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = Label,
                Anchor = new Point(.5, .5),
                Position = Position,
                Rotation = Direction,
                Icon = BitmapDescriptorFactory.FromResource("ic_arrow_tracking.png"),
                Tag = "direction_route",
                ZIndex = 1,
                IsDraggable = false
            };
            return pin;
        }
    }
}