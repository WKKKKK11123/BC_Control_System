using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel.RecipeBase
{
    public interface IRecipeStep
    {
        public ProcessStepEnum StepType { get; set; }
        public int StepNo { get; set; }
        public string Step { get; }
        public int Time { get; set; }

    }
}
