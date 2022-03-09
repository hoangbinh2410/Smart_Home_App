using BA_MobileGPS.Entities;
using BA_MobileGPS.Models;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MessageService : IMessageService
    {
        private readonly IRequestProvider requestProvider;
        private readonly IRealmBaseService<MessageSOSRealm, MessageSOS> messageSOSRepository;

        public MessageService(IRequestProvider requestProvider, IRealmBaseService<MessageSOSRealm, MessageSOS> messageSOSRepository)
        {
            this.requestProvider = requestProvider;
            this.messageSOSRepository = messageSOSRepository;
        }

        public List<MessageSOS> GetListMessage()
        {
            return messageSOSRepository.All().ToList();
        }

        public List<MessageSOS> FindListMessage(string content, DateTime date)
        {
            return messageSOSRepository.Find(x => x.Content.Equals(content)).ToList();
        }

        public List<MessageSOS> GetListMessage(string receiver)
        {
            return messageSOSRepository.Find(m => m.Receiver.Equals(receiver)).ToList();
        }

        public List<MessageSOS> GetListMessage(Expression<Func<MessageSOSRealm, bool>> predicate)
        {
            return messageSOSRepository.Find(predicate).ToList();
        }

        public MessageSOS GetMessage(long id)
        {
            try
            {
                return messageSOSRepository.Get(id);
            }
            catch
            {
                return default;
            }
        }

        public bool SaveMessage(MessageSOS message)
        {
            try
            {
                messageSOSRepository.Add(message);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool SaveMessage(MessageSOS message, out MessageSOS inserted)
        {
            try
            {
                inserted = messageSOSRepository.Add(message);
            }
            catch
            {
                inserted = default;
                return false;
            }

            return true;
        }

        public bool UpdateMessage(MessageSOS message)
        {
            try
            {
                messageSOSRepository.Update(message);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SendMessage(MessageSOS message)
        {
            bool result = false;
            try
            {
                var content = new JObject
                {
                    new JProperty("VehiclePlate", message.Receiver),
                    new JProperty("Data", message.Content)
                };
                var respone = await requestProvider.PostAsync<JObject, ResponseBase<bool>>(ApiUri.POST_SEND_MESSAGE, content);
                if(respone != null&& respone.Data)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public bool DeleteMessage(long id)
        {
            try
            {
                messageSOSRepository.Delete(id);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteMessages(string receiver)
        {
            try
            {
                foreach (var item in messageSOSRepository.Find(m => m.Receiver.Equals(receiver)).Select(m => m.Id))
                {
                    messageSOSRepository.Delete(item);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<ResponseBase<SmsPackageInfor>> GetPackageDataByte(SmsPackageRequest request)
        {
            ResponseBase<SmsPackageInfor> result = new ResponseBase<SmsPackageInfor>();
            try
            {
                var data = await requestProvider.PostAsync<SmsPackageRequest, ResponseBase<SmsPackageInfor>>(ApiUri.POST_SMSPACKAGE, request);

                if (data != null&& data.Data!=null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}