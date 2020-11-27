using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BA_MobileGPS.Utilities.Extensions
{
    public static class MapperUtility
    {
        /// <summary>
        /// 实体属性反射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T AutoMapping<T>(this object obj) where T : new()
        {
            T t = new T();
            var properties = obj.GetType().GetProperties();// BindingFlags.Public);//| BindingFlags.Instance);
            var fields = obj.GetType().GetFields();
            Type target = (t).GetType();

            foreach (var pp in properties)
            {
                PropertyInfo targetPP = target.GetProperty(pp.Name);
                object value = pp.GetValue(obj, null);

                if (targetPP != null && value != null)
                {
                    targetPP.SetValue(t, value, null);
                }
            }
            foreach (var field in fields)
            {
                var targetField = target.GetField(field.Name);
                object value = field.GetValue(obj);

                if (targetField != null && value != null)
                {
                    targetField.SetValue(t, value);
                }
            }
            return t;
        }

        public static TDestination MatchAndMap<TSource, TDestination>(TSource source)
           where TSource : class, new()
           where TDestination : class, new()
        {
            var destination = new TDestination();
            if (source != null && destination != null)
            {
                List<PropertyInfo> sourceProperties = source.GetType().GetProperties().ToList<PropertyInfo>();
                List<PropertyInfo> destinationProperties = destination.GetType().GetProperties().ToList<PropertyInfo>();

                foreach (PropertyInfo sourceProperty in sourceProperties)
                {
                    PropertyInfo destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);

                    if (destinationProperty != null)
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                    }
                }
            }
            return destination;
        }

        public static List<TDestination> MatchAndMapList<TSource, TDestination>(List<TSource> source)
          where TSource : class, new()
          where TDestination : class, new()
        {
            var listdestination = new List<TDestination>();
            foreach (var item in source)
            {
                listdestination.Add(MatchAndMap<TSource, TDestination>(item));
            }
            return listdestination;
        }
    }
}