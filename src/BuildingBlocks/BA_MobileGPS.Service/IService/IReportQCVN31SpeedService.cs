using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IReportQCVN31SpeedService
    {
        Task<IList<ReportQCVN31SpeedRespone>> GetData(ReportQCVN31SpeedRequest input);
    }
}
