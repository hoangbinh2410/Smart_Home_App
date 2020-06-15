using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BA_MobileGPS.Core
{
    public static class ObjectExtension
    {
        public static T DeepCopy<T>(this T other) where T : class
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(other));
        }

        public static List<T> DeepClone<T>(this List<T> source) where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (source == null)
            {
                return default;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();

            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (List<T>)formatter.Deserialize(stream);
            }
        }

        public static T DeepClone<T>(this T source) where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (source == null)
            {
                return default;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();

            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}