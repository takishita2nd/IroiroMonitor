using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IroiroMonitor
{
    [JsonObject("sensorModel")]
    public class Sensor
    {
        [JsonProperty("datetime")]
        public string datetime { get; set; }
        [JsonProperty("temperature")]
        public string temperature { get; set; }
        [JsonProperty("humidity")]
        public string humidity { get; set; }
    }
}
