using BA_MobileGPS.Entities.ResponeEntity.Support;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService.Support
{
    public interface IMessageSuportService
    {
        Task<List<MessageSupportRespone>> GetMessagesSupport(Guid id);
    }
}
