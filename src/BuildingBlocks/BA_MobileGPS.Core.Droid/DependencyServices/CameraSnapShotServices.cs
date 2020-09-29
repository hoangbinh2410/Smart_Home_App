using System;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraSnapShotServices))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class CameraSnapShotServices : ICameraSnapShotServices
    {
        [Obsolete]
        public string GetFolderPath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path.ToString();
        }

        public bool SaveSnapShotToGalery()
        {
            throw new NotImplementedException();
        }
    }
}