using BA_MobileGPS.Utilities;

using System;

namespace BA_MobileGPS.Core
{
    public class PhoneNumberRule<T> : IValidationRule<T>
    {
        public string ValidationMessage
        {
            get; set;
        }

        public string CountryCode { get; set; }

        public bool Check(T value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }
                var str = value as string;
                if (!string.IsNullOrEmpty(CountryCode))
                {
                    if (CountryCodeConstant.VietNam.Equals(CountryCode))
                    {
                        if (!str.StartsWith("0"))
                        {
                            str = string.Format("0{0}", str);
                        }
                        var newPhone = string.Empty;
                        return StringHelper.ValidPhoneNumer(str, MobileSettingHelper.LengthAndPrefixNumberPhone, out newPhone);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class PhoneNumberDoubleRule<T> : IValidationRule<T>
    {
        public string ValidationMessage
        {
            get; set;
        }

        public string PhoneStr { get; set; }

        public bool Check(T value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }
                var str = value as string;
                if (!string.IsNullOrEmpty(PhoneStr))
                {
                    int count = 0;
                    int i = 0;
                    while ((i = PhoneStr.IndexOf(str, i)) != -1)
                    {
                        i += str.Length;
                        count++;
                    }
                    if (count >= 2)
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}