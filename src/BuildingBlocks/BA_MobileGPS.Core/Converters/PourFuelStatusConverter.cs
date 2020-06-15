using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class PourFuelStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var str = string.Empty;
            if (!int.TryParse(value.ToString(), out var Value))
            {
                Value = -1;
            }
            switch (Value)
            {
                case (int)FuelStatusEnum.Absorb:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_Normal;
                    break;

                case (int)FuelStatusEnum.Pour:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_Lost;
                    break;

                case (int)FuelStatusEnum.SuspiciousAbsorb:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_NotInit;
                    break;

                case (int)FuelStatusEnum.SuspiciousPour:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_NotInit;
                    break;

                default:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_Normal;
                    break;
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}