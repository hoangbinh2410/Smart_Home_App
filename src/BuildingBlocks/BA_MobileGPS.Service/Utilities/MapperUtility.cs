using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BA_MobileGPS.Service.Utilities
{
    public class MapperUtility : IMapper
    {
        private void MatchAndMap<TSource, TDestination>(TSource source, TDestination destination)
            where TSource : class, new()
            where TDestination : class, new()
        {
            if (source != null && destination != null)
            {
                List<PropertyInfo> sourceProperties = source.GetType().GetProperties().ToList<PropertyInfo>();
                List<PropertyInfo> destinationProperties = destination.GetType().GetProperties().ToList<PropertyInfo>();

                foreach (PropertyInfo sourceProperty in sourceProperties)
                {
                    PropertyInfo destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);

                    if (destinationProperty != null)
                    {
                        try
                        {
                            destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        public TDestination MapProperties<TDestination>(object source)
           where TDestination : class, new()
        {
            var destination = Activator.CreateInstance<TDestination>();
            MatchAndMap(source, destination);
            return destination;
        }

        public List<TDestination> MapListProperties<TDestination>(object sources)
          where TDestination : class, new()
        {
            var destinations = new List<TDestination>();
            if (sources != null && sources is IList && sources.GetType().IsGenericType)
            {
                foreach (var item in (IList)sources)
                {
                    var destination = Activator.CreateInstance<TDestination>();
                    MatchAndMap(item, destination);
                    destinations.Add(destination);
                }
            }
            return destinations;
        }
    }
}