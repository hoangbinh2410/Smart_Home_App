using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Service.Utilities
{
    public interface IMapper
    {
        TDestination MapProperties<TDestination>(object mapSource)
            where TDestination : class, new();

        List<TDestination> MapListProperties<TDestination>(object sources)
          where TDestination : class, new();
    }
}
