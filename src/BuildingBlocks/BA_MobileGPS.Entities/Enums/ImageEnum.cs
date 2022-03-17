using BA_MobileGPS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BA_MobileGPS.Entities
{  
    public enum FileType
    {
        Image,
        File,
        Video
    }

    public enum SystemType
    {
        [Description("BinhAnh")]
        BA,

        [Description("CNN")]
        CNN,

        [Description("GIS")]
        GIS
    }

    public enum ModuleType
    {
        [Description("Avatar")]
        Avatar,

        [Description("Expenses")]
        Expenses,

        [Description("Paper")]
        Paper,

        [Description("Drivers")]
        Driver
    }
    public enum FileSizeUnit
    {
        [Description("B")]
        Byte,

        [Description("KB")]
        K,

        [Description("MB")]
        M,

        [Description("GB")]
        G
    }
    //public static class FileSizeUnitExtensions
    //{
    //    public static string Description(this FileSizeUnit? unit)
    //    {
    //        return unit == null ? string.Empty : unit.Value.ToDescription();
    //    }

    //    public static int? Value(this FileSizeUnit? unit)
    //    {
    //        return unit?.Value();
    //    }
    //}
}
