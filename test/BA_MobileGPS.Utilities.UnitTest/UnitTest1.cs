using System;
using System.Collections.Generic;
using Xunit;

namespace BA_MobileGPS.Utilities.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string msg = "Cau hinh 999: 10:54:54 - SRV: KN, GSM: ON-31, GPS: CO, SAT: OK-ON-0, RTC: OK, FLASH: OK, FRAM: ER, BLE: OK-CON, SOS: 0, Vex:20409, Vsolar:97, Vbat:4047, Vsys:20461, V33:3267, Vgsm:4039, Vsup:4874, Mode: GSM+SAT, Aux: 1 4|1 1|1 1";
            var data = msg.Remove(0, msg.IndexOf(':') + 1).Trim();
            var result = data.Remove(0, data.IndexOf('-') + 1).Trim();
            var lst = result.Split(',');
            var state = new Dictionary<string, string>();
            if (lst != null && lst.Length > 0)
            {
                for (int i = 0; i < lst.Length; i++)
                {
                    var key = lst[i].Substring(0, lst[i].IndexOf(':')).Trim().ToUpper();
                    var value = lst[i].Remove(0, lst[i].IndexOf(':') + 1).Trim().ToUpper();
                    state.Add(key, value);
                }

            }

        }
    }
}
