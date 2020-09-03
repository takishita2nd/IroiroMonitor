using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IroiroMonitor
{
    [JsonObject("CommandModel")]
    class Command
    {
        [JsonProperty("number")]
        public int number { get; set; }
    }
}
