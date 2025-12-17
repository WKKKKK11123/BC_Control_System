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
    public class QDRTankModuleRecipeStepBase : IRecipeStep
    {
        public virtual ProcessStepEnum StepType { get; set; }
        public virtual int StepNo { get; set; }
        [JsonIgnore]
        public virtual string Step { get => $"{StepType} {StepNo}"; }
        public virtual int Time { get; set; }
        public virtual bool FastLeak { get; set; }
        public virtual bool SlowLeak { get; set; }
        public virtual bool QDR { get; set; }
        public virtual bool Shower { get; set; }
        public virtual bool Bubble { get; set; }
        public virtual bool Agitation { get; set; }
        public virtual bool ResistivityCheck { get; set; }
        public virtual bool N2Bubble { get; set; }
    }
}
