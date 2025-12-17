using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel
{
    public class FlowRecipeClass : IRevisioninterface
    {
         public string Name { get; set; } = "Flow Recipe Base";
         public string Comment { get; set; } = "";
        public string RevisionNo { get; set; } = "V1.0";
        public string RevComment { get; set; } = "";
        public List<FlowStepClass> FlowStepList { get; set; } = new List<FlowStepClass>();
    }
}
