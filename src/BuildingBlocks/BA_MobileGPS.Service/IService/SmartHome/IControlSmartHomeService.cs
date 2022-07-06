using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{
  public interface IControlSmartHomeService
    {
        Task<bool> ControlHome(int id);
    }
}
