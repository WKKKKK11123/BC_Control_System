using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel
{
    public class NMPClass :SCClass
    {
        [JsonIgnore]
        public override bool Agination { get ; set; }
    }
}
