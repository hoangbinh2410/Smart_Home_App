using BA_MobileGPS.Core.Droid.DependencyServices;

using System.IO;

using Xamarin.Forms;

[assembly: Dependency(typeof(SaveService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class SaveService : ISaveService
    {
        public string Save(Stream stream)
        {
            File.WriteAllBytes(GetLocation(), ReadFully(stream));
            return GetLocation();
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private string GetLocation()
        {
            var appdata = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path.ToString();
            var directoryname = Path.Combine(appdata, "Saved Images");
            if (!Directory.Exists(directoryname))
                Directory.CreateDirectory(directoryname);
            return Path.Combine(directoryname, "edits.jpg");
        }
    }
}