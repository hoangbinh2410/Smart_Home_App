﻿using BA_MobileGPS.Entities;
using System;
using System.Collections;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class HasPermisstionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if null then not visible
            if (value == null)
                return false;

            return StaticSettings.User.Permissions.IndexOf((int)value) != -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}