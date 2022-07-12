using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class StastusSmartHomeResponse
    {
        public List<Switche> switches { get; set; }      
        public List<Light> lights { get; set; }      
        public List<Sensor> sensors { get; set; }      
    }

    public class Switche
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code_name { get; set; }
        public string type_name { get; set; }
        public DateTime time { get; set; }
        public bool data { get; set; }
    }
    public class Light
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code_name { get; set; }
        public string type_name { get; set; }
        public DateTime time { get; set; }
        public Data1 data { get; set; }

    }
    public class Sensor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code_name { get; set; }
        public string type_name { get; set; }
        public DateTime time { get; set; }
        public string data { get; set; }
    }
    public class Data1
    {
        public bool power { get; set; }
        public int config { get; set; }
    }
    public class ControlResponse
    {
        public bool data { get; set; }
    }
    public class AirControll
    {
        public int data { get; set; }
    }
}
