using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    public class QDRTankModuleRecipeStep: QDRTankModuleRecipeStepBase
    {
        [JsonIgnore]
        public override bool Bubble { get => base.Bubble; set => base.Bubble = value; }
        [JsonIgnore]
        public override bool N2Bubble { get => base.Bubble; set => base.Bubble = value; }
    }
}
