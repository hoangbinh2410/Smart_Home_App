using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        [Fact]
        public void GenerateMappingObject()
        {

            // GenerateModelToData(typeof(User), typeof(UserRealm));
            // GenerateDataToModel(typeof(UserRealm), typeof(User));

            // GenerateModelToData(typeof(AdditionalFee), typeof(AdditionalFeeRealm));
            // GenerateDataToModel(typeof(AdditionalFeeRealm), typeof(AdditionalFee));

            // GenerateModelToData(typeof(AdditionalFeeTypeSync), typeof(AdditionalFeeTypeRealm));
            // GenerateDataToModel(typeof(AdditionalFeeTypeRealm), typeof(AdditionalFeeType));

            GenerateModelToData(typeof(VehicleOnline), typeof(VehicleOnlineViewModel));
            GenerateDataToModel(typeof(VehicleOnlineViewModel), typeof(VehicleOnline));

            // GenerateModelToData(typeof(LandmarkSync), typeof(LandmarkRealm));
            // GenerateDataToModel(typeof(LandmarkRealm), typeof(Landmark));

            // GenerateModelToData(typeof(RouteVehicleSync), typeof(RouteVehicleRealm));
            // GenerateDataToModel(typeof(RouteVehicleRealm), typeof(RouteVehicleType));

            // GenerateModelToData(typeof(SettingSync), typeof(SettingRealm));
            // GenerateDataToModel(typeof(SettingRealm), typeof(Setting));

            // GenerateModelToData(typeof(RouteSync), typeof(RouteRealm));
            // GenerateDataToModel(typeof(RouteRealm), typeof(Route));

            // GenerateModelToData(typeof(User), typeof(UserRealm));
            // GenerateDataToModel(typeof(UserRealm), typeof(User));

            // GenerateModelToData(typeof(BookedHistory), typeof(BookedHistoryRealm));
            // GenerateDataToModel(typeof(BookedHistoryRealm), typeof(BookedHistory));

            // GenerateModelToData(typeof(VersionData), typeof(VersionDataRealm));
            // GenerateDataToModel(typeof(VersionDataRealm), typeof(VersionData));

            // GenerateModelToData(typeof(HighlightAddressSync), typeof(HighlightAddressRealm));
            // GenerateDataToModel(typeof(HighlightAddressRealm), typeof(HighLightAddress));
        }

        public string GenerateModelToData(Type s, Type d)
        {
            var fields = d.GetProperties();

            Debug.WriteLine($"private static {d.Name} ModelToData({s.Name} model)");
            Debug.WriteLine("{");
            Debug.WriteLine($"return new {d.Name}");
            Debug.WriteLine("{");
            foreach (var propertyInfo in fields)
            {
                try
                {
                    var thisField = s.GetProperty(propertyInfo.Name);
                    // Console.WriteLine($"{thisField?.DeclaringType?.Name} = {propertyInfo.Name}");
                    if (thisField != null)
                        Debug.WriteLine($"{propertyInfo.Name} = model.{thisField.Name},");
                }
                catch (Exception e)
                {
                    //de.WriteFile("", "", e);
                }
            }

            Debug.WriteLine("};");
            Debug.WriteLine("}");
            return "";
        }

        public string GenerateDataToModel(Type s, Type d)
        {
            var fields = d.GetProperties();

            Debug.WriteLine($"private static {d.Name} DataToModel({s.Name} model)");
            Debug.WriteLine("{");
            Debug.WriteLine($"return new {d.Name}");
            Debug.WriteLine("{");
            foreach (var propertyInfo in fields)
            {
                try
                {
                    var thisField = s.GetProperty(propertyInfo.Name);
                    // Console.WriteLine($"{thisField?.DeclaringType?.Name} = {propertyInfo.Name}");
                    if (thisField != null)
                        Console.WriteLine($"{propertyInfo.Name} = model.{thisField.Name},");
                }
                catch (Exception e)
                {
                    //Logger.WriteFile("", "", e);
                }
            }

            Debug.WriteLine("};");
            Debug.WriteLine("}");
            return "";
        }
    }
}
