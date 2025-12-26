using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    public class TotalStep: IRecipeStep
    {
        public ProcessStepEnum StepType { get; set; }
        public int StepNo { get; set; } = 0;
        public string Step { get; } = "Total Step";
        public int Time { get; set; } = 0;
    
    }
}
