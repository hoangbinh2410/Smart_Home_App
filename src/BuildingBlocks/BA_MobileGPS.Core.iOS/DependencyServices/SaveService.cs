using BA_MobileGPS.Core.iOS.DependencyServices;

using System;
using System.IO;

using Xamarin.Forms;

[assembly: Dependency(typeof(SaveService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class SaveService : ISaveService
    {
        public string Save(Stream stream)
        {
            stream.Position = 0;
            var byteArray = ReadFully(stream);
            File.WriteAllBytes(GetLocation(), byteArray);
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
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directoryname = Path.Combine(appdata, "Saved Images");
            if (!Directory.Exists(directoryname))
                Directory.CreateDirectory(directoryname);
            return Path.Combine(directoryname, "edits.jpg");
        }
    }
}