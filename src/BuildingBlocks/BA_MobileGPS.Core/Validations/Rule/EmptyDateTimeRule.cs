using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core
{
   public class EmptyDateTimeRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "This field is required";

        public bool Check(T value)
        {
            if (value is DateTime date)
            {
                if (date == new DateTime() || date == DateTime.MinValue)
                {
                    return false;
                }
                return true;
            }
            else return false;            
        }
    }
}
