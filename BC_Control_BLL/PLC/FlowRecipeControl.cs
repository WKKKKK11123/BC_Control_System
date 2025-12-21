using System.Text.RegularExpressions;
using BC_Control_BLL.recipedownload;
using BC_Control_Helper.RecipeDownLoad;
using BC_Control_Models;
using BC_Control_Models.RecipeModel;

namespace BC_Control_Helper
{
    public class FlowRecipeControl : IRecipeDownLoad
    {
        public string path { get; set; }
        public FlowRecipeClass recipe { get; set; }
        public readonly IPLCHelper _plcHelper;
        public FlowRecipeControl(IPLCHelper plcHelper)
        {
            _plcHelper=plcHelper;
        }
        private void GetRecipe()
        {
            recipe = JSONHelper.JSONToEntity<FlowRecipeClass>(path);
        }
        public bool DownLoad(string startAddress, PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                short[] shorts1 = new short[10000];
                var match = Regex.Match(startAddress, @"([a-zA-Z]+)(\d+)");
                int intstart = 0;
                string AddressType;
                short[] shorts=new short[10];
                if (match.Success)
                {
                    AddressType = match.Groups[1].Value;  // 前面的字符部分
                    intstart = Convert.ToInt32(match.Groups[2].Value);  // 后面的数字部分
                }
                else
                {
                    return false;
                }
                
                GetRecipe();
                //bool result = true;
                bool result = PLCSelect.Instance.SelectPLC(plcEnum).Write($"{AddressType}{intstart}", shorts1).IsSuccess;
                result &= PLCSelect.Instance.SelectPLC(plcEnum).Write(startAddress, recipe.Name, 20).IsSuccess;
                foreach (var item in recipe.FlowStepList)
                {
                    result&=DownLoadModel(item,$"{AddressType}{1000+(item.FlowStep*1000)}");
                    shorts[item.FlowStep-1]= Convert.ToInt16(Enum.Parse(typeof(BathNameEnum),item.BathName.ToString()));
                }            
                //shorts[recipe.FlowStepList.Count] = (short)BathNameEnum.END; // 结束工位
                result &= PLCSelect.Instance.SelectPLC(plcEnum).Write($"{AddressType}{intstart+9000}", shorts).IsSuccess;
                result &= PLCSelect.Instance.SelectPLC(plcEnum).Write($"{AddressType}{intstart + 9999}",(short)1).IsSuccess;
                if (result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception EE)
            {

                return false;
            }

        }
        private bool DownLoadModel(FlowStepClass flowStepClass,string startAddress)
        {
            IRecipeDownLoad recipeDownLoad;
            string path = Path.Combine(@"D:\Recipe",Enum.GetName(typeof(BathNameEnum),flowStepClass.BathName),flowStepClass.UnitRecipeName);
            switch (flowStepClass.BathName)
            {
                case BathNameEnum.Ag_1:
                    recipeDownLoad = new ETCHRecipeControl(_plcHelper);
                    recipeDownLoad.path = File.ReadAllText(path);
                    return recipeDownLoad.DownLoad(startAddress, PlcEnum.PLC1);
                    break;
                case BathNameEnum.QDR_1:
                    recipeDownLoad = new QDRRecipeControl(_plcHelper);
                    recipeDownLoad.path = File.ReadAllText(path);
                    return recipeDownLoad.DownLoad(startAddress, PlcEnum.PLC1);
                    break;                
                case BathNameEnum.LPD_1:
                    recipeDownLoad = new LPDRecipeControl(_plcHelper);
                    recipeDownLoad.path = File.ReadAllText(path);
                    return recipeDownLoad.DownLoad(startAddress, PlcEnum.PLC1);
                    break;
                    return false;
                
                default:
                    return false;
            }
        }

    }

}
