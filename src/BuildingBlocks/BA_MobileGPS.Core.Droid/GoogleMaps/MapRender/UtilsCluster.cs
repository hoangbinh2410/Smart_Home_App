// Original code from https://github.com/javiholcman/Wapps.Forms.Map/
// Cacheing implemented by Gadzair

using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Droid
{
    public class UtilsCluster
    {
        public static Task<global::Android.Gms.Maps.Model.BitmapDescriptor> ConvertViewToBitmapDescriptor(global::Android.Views.View v)
        {
            return Task.Run(() =>
            {
                var bmp = Utils.ConvertViewToBitmap(v);
                var img = global::Android.Gms.Maps.Model.BitmapDescriptorFactory.FromBitmap(bmp);
                return img;
            });
        }
    }
}