using System;
using System.IO;
using System.Threading.Tasks;

namespace BA_MobileGPS.Utilities.Extensions
{
    public static class StreamExtension
    {
        public static string ToString(this Stream s)
        {
            s.Position = 0;
            StreamReader reader = new StreamReader(s);
            string text = reader.ReadToEnd();

            return text;
        }

        public static string ToBase64String(this Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public async static Task<string> ToBase64(this Stream stream)
        {
            try
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}