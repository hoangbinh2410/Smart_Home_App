using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Ioc;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Unity;

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
                return Instance.GetType().GetProperty(key).GetValue(Instance)?.ToString() ?? key;
            }
            catch
            {
                return key;
            }
        }

        public static T Get<T>(MobileResourceNames key, T defaultValue, T defaultValueEng) where T : IConvertible
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var cultureInfo = Settings.CurrentLanguage;
            var val = cultureInfo == CultureCountry.Vietnamese ? defaultValue : defaultValueEng;
            try
            {
                var configDict = DicMobileResource;

                // Neu dictionary chua key nay thi moi lay ra, neu ko tra ve gia tri mac dinh
                if (configDict != null && configDict.Count > 0 && configDict.ContainsKey(key.ToString()))
                {
                    var setting = configDict[key.ToString()].ToString();

                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        // this will throw an exception when conversion is not possible
                        val = (T)converter.ConvertFromString(setting);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodBase.GetCurrentMethod().Name, key.ToString(), ex));
            }
            sw.Stop();
            Debug.WriteLine(string.Format("MobileResourceGet {0} : {1}", key.ToString(), sw.ElapsedMilliseconds));
            

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

                        _DicMobileResource = service.Find(x => x.CodeName == Settings.CurrentLanguage).ToDictionary(k => k.Name, v => v.Value);
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