using System;
using System.Drawing;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.iOS.DependencyServices;
using UIKit;
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

            var image = UIImage.FromFile(oldPath);
            image.SaveToPhotosAlbum((uiImage, nsError) =>
            {
                if (nsError != null) { }
              // do something about the error..
                 else { }
                 // image should be saved
             });
            return true;

        }
    }
}