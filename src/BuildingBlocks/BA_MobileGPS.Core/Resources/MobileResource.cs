using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Ioc;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        private static MobileResource instance = null;

        protected static MobileResource Instance
        {
            get
            {
                // Lazy load => design Pattern
                if (instance == null)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    instance = new MobileResource();
                    sw.Stop();
                    Debug.WriteLine(string.Format("InstanceMobileResource: {0}", sw.ElapsedMilliseconds));
                }
                return instance;
            }
        }

        public static string Get(string key)
        {
            try
            {
                string value = Instance.GetType().GetProperty(key).GetValue(Instance)?.ToString() ?? key;
                return value;
            }
            catch
            {
                return key;
            }
        }

        public static string Get(MobileResourceNames key, string defaultValue,
            string defaultValueEng,
            string defaultValueLao = "")
        {
            var val = string.Empty;
            if (App.CurrentLanguage == CultureCountry.English)
            {
                val = defaultValueEng;
            }
            else if (App.CurrentLanguage == CultureCountry.Laos)
            {
                val = defaultValueEng;
            }
            else
            {
                val = defaultValue;
            }
            try
            {
                var configDict = DicMobileResource;

                // Neu dictionary chua key nay thi moi lay ra, neu ko tra ve gia tri mac dinh
                if (configDict != null && configDict.Count > 0 && configDict.ContainsKey(key.ToString()))
                {
                    var setting = configDict[key.ToString()].ToString();

                    var converter = TypeDescriptor.GetConverter(typeof(string));
                    if (converter != null)
                    {
                        // this will throw an exception when conversion is not possible
                        val = (string)converter.ConvertFromString(setting);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodBase.GetCurrentMethod().Name, key.ToString(), ex));
            }
            return val;
        }

        public static string GetResourceNotDB(MobileResourceNames key, string defaultValue, string defaultValueEng)
        {
            var val = App.CurrentLanguage == CultureCountry.Vietnamese ? defaultValue : defaultValueEng;
            return val;
        }

        public static IDictionary<string, string> _DicMobileResource = null;

        public static IDictionary<string, string> DicMobileResource
        {
            get
            {
                if (_DicMobileResource == null)
                {
                    try
                    {
                        var service = Prism.PrismApplicationBase.Current.Container.Resolve<IResourceService>();

                        _DicMobileResource = service.Find(x => x.CodeName == App.CurrentLanguage).ToDictionary(k => k.Name, v => v.Value);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
                return _DicMobileResource;
            }
        }
    }
}