using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Interfaces
{
    public interface IAppVersionService
    {
        string GetAppVersion();

        string GetAppBuild();
    }
}
