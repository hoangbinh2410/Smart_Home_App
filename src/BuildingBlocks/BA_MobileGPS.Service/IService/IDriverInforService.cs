using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IDriverInforService
    {
        Task<List<DriverInfor>> GetListDriverByCompanyId(int companyId, int pageSize = 0, 
            int pageIndex = 0, DriverOrderByEnum orderBy = DriverOrderByEnum.ASC, DriverSortOderEnum sortOrder = DriverSortOderEnum.DisplayName);
        Task<int> AddDriverInfor(DriverInfor driver); // return Id
        Task<int> UpdateDriverInfor(DriverInfor driver); // return Id
        Task<int> DeleteDriverInfor(DriverDeleteRequest driver); // return Id
        
    }


    public enum DriverSortOderEnum
    {
        CreatedDate,
        DisplayName,
        Mobile
    }
    public enum DriverOrderByEnum
    {
        DESC = 0,
        ASC = 1
    }
}
