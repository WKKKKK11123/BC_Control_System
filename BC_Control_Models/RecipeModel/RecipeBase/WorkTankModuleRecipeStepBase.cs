using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    [AddINotifyPropertyChangedInterface]
    public class WorkTankModuleRecipeStepBase : IRecipeStep
    {
        public virtual ProcessStepEnum StepType { get; set; }
        public virtual int StepNo { get; set; }
        [JsonIgnore]
        public virtual string Step { get =>$"{StepType} {StepNo}"; }
        public virtual int Time { get; set; }
        public virtual float DSM { get; set; }
        public virtual bool PumpStop { get; set; }
        public virtual bool Agination { get; set; }
    }
}
