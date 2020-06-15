using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    public enum IconCode
    {
        [Description("none")]
        None = 0,

        [Description("truck")]
        Trucks = 1,

        [Description("motorcycles")]
        Motorcycles = 2,

        [Description("bus")]
        Bus = 3,

        [Description("car")]
        Cars = 4,

        [Description("boat")]
        Boat = 5,

        [Description("taxi")]
        Taxi = 6,

        [Description("rescue")]
        Rescue = 7,

        [Description("car")]
        CarsV1 = 8,

        [Description("train")]
        Train = 9,

        [Description("ball")]
        Other = 10,

        [Description("tank")]
        Tank = 11,

        [Description("tipper")]
        Tipper = 12,

        [Description("arrowcars")]
        ArrowCars = 13,

        [Description("trash")]
        Trash = 14,

        [Description("trashtwo")]
        Trash2 = 15,

        [Description("motorbike")]
        Motobike2 = 16,

        [Description("excavator")]
        Excavator = 17,

        [Description("electric")]
        Electric = 18
    }
}