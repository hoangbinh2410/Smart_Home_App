using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class EngineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateVehicleExtension.EngineStateConverter((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DoorOpenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateVehicleExtension.DoorOpen((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class AirConditionerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateVehicleExtension.AirConditionerOn((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CountStopParkingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return $"{value} Lần";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class MemoryStickConverter : IValueConverter
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
                case (int)MemoryStatusEnum.Normal:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_Normal;
                    break;

                case (int)MemoryStatusEnum.Lost:
                    str = MobileResource.DetailVehicle_Label_ValueMemoryStick_Lost;
                    break;

                case (int)MemoryStatusEnum.NotInit:
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
            return null;
        }
    }

    public class BenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateVehicleExtension.BenState((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CraneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateVehicleExtension.CraneState((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IsCompanyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //1- Đối với TK companytype = 3 có nhóm đội , không có quyền xem khách lẻ : Ẩn icon chọn công ty, hiện icon nhóm đội (petajicohn/12341234)
            //2 - Đối với TK companytype = 3 có nhóm đội , có quyền xem khách lẻ(PermissionID = 18) : Hiện icon chọn công ty, hiện icon nhóm đội(admin1002/ 12341234)
            if (UserHelper.isCompanyPartner((CompanyType)value) || (UserHelper.isCompanyEndUserWithPermisstion((CompanyType)value)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ConcreteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StateVehicleExtension.ConcreteState((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}