using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.EAP
{
    public class ProcessJobClass
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("mf")]
        public string MF { get; set; }
        [JsonProperty("carrier")]
        public CarrierInfoClass[] Carrier { get; set; }
        [JsonProperty("recipe")]
        public RecipeInfoClass Recipe { get; set; }
        [JsonProperty("autoStart")]
        public string AutoStart { get; set; }
    }
}
