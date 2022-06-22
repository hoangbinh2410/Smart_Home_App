using BA_MobileGPS.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service
{
    public class ControlSmartHomeService : IControlSmartHomeService
    {
        private readonly IRequestProvider _requestProvider;
        public ControlSmartHomeService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public Task<bool> ControlAir(bool onDevice, string NameDevice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ControlCurtains(bool onDevice, string NameDevice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ControlHeater(bool onDevice, string NameDevice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ControlLamp(bool onDevice, string NameDevice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ControlMaindoor(bool onDevice, string NameDevice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ControlWindow(bool onDevice, string NameDevice)
        {
            throw new NotImplementedException();
        }
    }
}
