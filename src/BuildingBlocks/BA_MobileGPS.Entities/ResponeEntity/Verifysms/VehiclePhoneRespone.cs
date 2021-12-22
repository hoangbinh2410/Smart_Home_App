using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
  public class VehiclePhoneRespone : BaseModel
    { 
            public bool state { get; set; }
            public byte errorcode { get; set; }
    }
}
