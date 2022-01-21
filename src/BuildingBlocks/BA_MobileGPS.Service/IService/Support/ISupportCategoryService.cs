using BA_MobileGPS.Entities.RequestEntity.Support;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService.Support
{
    public interface ISupportCategoryService
    {
        Task<List<SupportCategoryRespone>> GetListSupportCategory(int languageId);
        Task<List<MessageSupportRespone>> GetMessagesSupport(Guid id, int languageId);
        Task<SupportBapRespone> Getfeedback(SupportBapRequest request );
    }
}
