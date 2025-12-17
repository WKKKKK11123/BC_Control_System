using NPOI.POIFS.Storage;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    [AddINotifyPropertyChangedInterface]
    public class ModuleRecipeClassBase<T> : ILIftSpeedInterface, IRevisioninterface where T : class , IRecipeStep, new()
    {
        public virtual string Name { get; set; } = "New Recipe";
        public virtual string Comment { get; set; } = "V1.0";
        public virtual string RevisionNo { get; set; } = "V1.0";
        public virtual string RevComment { get; set; } = "";
        public virtual TempratureEnum Temprature { get; set; } = TempratureEnum.pattern1;
        public virtual short LiftSpeedPre { get; set; } = 210;
        public virtual short LiftSpeedDown1 { get; set; } = 110;
        public virtual short LiftSpeedDown2 { get; set; } = 25;
        public virtual short LiftSpeedUp1 { get; set; } = 25;
        public virtual short LiftSpeedUp2 { get; set; } = 25;
        public virtual List<T> RecipeStepCollection { get; set; }=new List<T>();
        
    }
}
