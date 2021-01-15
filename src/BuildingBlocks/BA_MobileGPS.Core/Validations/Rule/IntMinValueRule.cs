using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core
{
    public class IntMinValueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "This field is invalid";
        public int MinValue { get; set; }

        public bool Check(T value)
        {
            if (value is int current)
            {
                if (current < MinValue)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
    public class IntMaxValueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "This field is invalid";
        public int MaxValue { get; set; }

        public bool Check(T value)
        {
            if (value is int current)
            {
                if (current > MaxValue)
                {
                    return false;
                }
                return true;
            }


            return false;
        }
    }
}
