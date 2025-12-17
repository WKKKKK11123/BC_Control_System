using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    public class ETCHTankModuleRecipeStep:WorkTankModuleRecipeStepBase
    {
        [JsonIgnore]
        public override float DSM { get => base.DSM; set => base.DSM = value; }
    }
}
