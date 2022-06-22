using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{
  public interface IControlSmartHomeService
    {
        Task<bool> ControlWindow(bool onDevice, string NameDevice);
        Task<bool> ControlCurtains(bool onDevice, string NameDevice);
        Task<bool> ControlMaindoor(bool onDevice, string NameDevice);
        Task<bool> ControlAir(bool onDevice, string NameDevice);
        Task<bool> ControlHeater(bool onDevice, string NameDevice);
        Task<bool> ControlLamp(bool onDevice, string NameDevice);
    }
}
