using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public enum DataItem
    {
        [Description("Địa điểm khác")]
        Place = 1,

        //[Description("bg_Account.png")]
        //Image = 2
        [Description("https://media.istockphoto.com/vectors/photo-coming-soon-image-icon-vector-illustration-isolated-on-white-vector-id1193046540?k=20&m=1193046540&s=612x612&w=0&h=HQfBJLo1S0CJEsD4uk7m3EkR99gkICDdf0I52uAlk-8=")]
        Image = 2,

        [Description("Tất cả chi phí")]
        AllExpense = 3,
    }
}
