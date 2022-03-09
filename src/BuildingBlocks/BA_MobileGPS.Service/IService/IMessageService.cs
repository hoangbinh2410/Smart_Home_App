using BA_MobileGPS.Entities;
using BA_MobileGPS.Models;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IMessageService
    {
        List<MessageSOS> FindListMessage(string content, DateTime date);

        List<MessageSOS> GetListMessage();

        List<MessageSOS> GetListMessage(string receiver);

        List<MessageSOS> GetListMessage(Expression<Func<MessageSOSRealm, bool>> predicate);

        MessageSOS GetMessage(long id);

        bool SaveMessage(MessageSOS message);

        bool SaveMessage(MessageSOS message, out MessageSOS inserted);

        bool UpdateMessage(MessageSOS message);

        Task<bool> SendMessage(MessageSOS message);

        bool DeleteMessage(long id);

        bool DeleteMessages(string receiver);

        Task<ResponseBase<SmsPackageInfor>> GetPackageDataByte(SmsPackageRequest request);
    }
}