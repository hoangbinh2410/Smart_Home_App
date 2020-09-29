using System;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraSnapShotServices))]
namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class CameraSnapShotServices : ICameraSnapShotServices
    {
        public string GetFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public bool SaveSnapShotToGalery(string oldPath)
        {
            throw new NotImplementedException();
        }
    }
}