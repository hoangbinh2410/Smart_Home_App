using Android.Runtime;

using System.Collections.Generic;

namespace BA_MobileGPS.Core.Droid.Extensions
{
    public static class EnumerableExtensions
    {
        public static JavaList<T> ToJavaList<T>(this IEnumerable<T> self)
        {
            var array = new JavaList<T>();
            foreach (var item in self ?? new T[0])
            {
                array.Add(item);
            }

            return array;
        }
    }
}