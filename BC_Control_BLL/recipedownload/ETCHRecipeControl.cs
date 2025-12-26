using BC_Control_Models;
using BC_Control_Models.RecipeModel.RecipeBase;
using BC_Control_Models.RecipeModel.RecipeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BC_Control_BLL.recipedownload
{
    public class ETCHRecipeControl : ModuleRecipeServiceBase<ETCHTankModuleRecipeStep, ETCHRecipeClassBase>
    {
        public ETCHRecipeControl(IPLCHelper plcHelper) : base(plcHelper)
        {

        }
        public override short[] RecipeStepToShortArray(ETCHTankModuleRecipeStep t)
        {
            try
            {
                short[] shorts = new short[20];
                short steptemp = Convert.ToInt16(Regex.Match(t.Step, @"-?\d+(\.\d+)?").Value);
                int Step = (steptemp - 1) * 20;
                var shortstemp = floatTointArray(steptemp);
                //Array.Copy(shortstemp, 0, shorts, 0, shortstemp.Length);
                shortstemp = floatTointArray(t.Time);
                Array.Copy(shortstemp, 0, shorts, 0, shortstemp.Length);
                shortstemp = floatTointArray(t.PumpStop ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 2, shortstemp.Length);
                shortstemp = floatTointArray(t.Agination ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 4, shortstemp.Length);
                //shorts[Step + 4] = (short)t.DSM;
                return shorts;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
