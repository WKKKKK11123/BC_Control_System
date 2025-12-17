using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    [AddINotifyPropertyChangedInterface]
    public class LPDTankModuleRecipeBase : IRecipeStep
    {
        public virtual ProcessStepEnum StepType { get; set; }
        public virtual int StepNo { get; set; }
        [JsonIgnore]
        public virtual string Step { get => $"{StepType} {StepNo}"; }
        public int Time { get; set; }
        public DIWEnum DIW { get; set; }
        public BlowPatternEnum BlowPattern { get; set; }
        public short N2Flow { get; set; }
        public short IPAN2Flow { get; set; }
        public bool QDR { get; set; }
        public bool Vacuum { get; set; }
        public bool ResistivityCheck { get; set; }
        public short LFRSpeed { get; set; }
        public LFREnum LFR { get; set; }
    }
}
