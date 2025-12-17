using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel
{
    [AddINotifyPropertyChangedInterface]
    public class FlowStepClass
    {
         public int FlowStep { get; set; }
        public BathNameEnum BathName { get; set; }
        public string UnitRecipeName { get; set; }
        public string Comment { get; set; }
    }
}
