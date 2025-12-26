using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BC_Control_BLL.recipedownload;
using BC_Control_Models;
using BC_Control_Models.RecipeModel;
using BC_Control_Models.RecipeModel.RecipeBase;
using BC_Control_Models.RecipeModel.RecipeEntity;

namespace BC_Control_Helper
{
    public class QDRRecipeControl : ModuleRecipeServiceBase<QDRTankModuleRecipeStep, QDRRecipeClassBase>
    {
        public QDRRecipeControl(IPLCHelper plcHelper) : base(plcHelper)
        {

        }
        public override short[] RecipeStepToShortArray(QDRTankModuleRecipeStep t)
        {
            try
            {
                short[] shorts = new short[20];
                short steptemp = Convert.ToInt16(Regex.Match(t.Step, @"-?\d+(\.\d+)?").Value);
                int Step = (steptemp - 1) * 20;
                var shortstemp = floatTointArray(steptemp);
                shortstemp = floatTointArray(t.Time);
                Array.Copy(shortstemp, 0, shorts, 0, shortstemp.Length);
                shortstemp = floatTointArray(t.FastLeak ? 1 : 0); ;
                Array.Copy(shortstemp, 0, shorts, 2, shortstemp.Length);
                shortstemp = floatTointArray(t.SlowLeak ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 4, shortstemp.Length);
                shortstemp = floatTointArray(t.QDR ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 6, shortstemp.Length);
                shortstemp = floatTointArray(t.Shower ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 8, shortstemp.Length);
                shortstemp = floatTointArray(t.Agitation ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 8, shortstemp.Length);
                shortstemp = floatTointArray(t.ResistivityCheck ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 10, shortstemp.Length);
                //Array.Copy(shortstemp, 0, shorts, 12, shortstemp.Length);
                return shorts;
            }
            catch (Exception)
            {
                throw;
            }

        }
        //public string path { get; set; }
        //public QDRRecipeClass recipe { get; set; }
        //public void GetRecipe()
        //{
        //    //string tempRecipe = File.ReadAllText(path);
        //    recipe = JSONHelper.JSONToEntity<QDRRecipeClass>(path);

        //}
        //public bool DownLoad(string startAddress, PlcEnum plcEnum = PlcEnum.PLC1)
        //{
        //    try
        //    {
        //        GetRecipe();
        //        short[] shorts = new short[1000];
        //        if (!PLCSelect.Instance.SelectPLC(plcEnum).Write(startAddress, shorts).IsSuccess)
        //        {
        //            return false;
        //        }
        //        short[] nametemp = GlobalMethodHelper.Instance.stringTointArray(recipe.Name, 20);
        //        Array.Copy(nametemp, 0, shorts, 0, 20);
        //        short[] nameComment = GlobalMethodHelper.Instance.stringTointArray(recipe.Comment, 20);
        //        Array.Copy(nameComment, 0, shorts, 20, 20);
        //        short[] revisionNotemp = GlobalMethodHelper.Instance.stringTointArray(recipe.RevisionNo, 10);
        //        Array.Copy(revisionNotemp, 0, shorts, 40, 10);
        //        short[] revCommenttemp = GlobalMethodHelper.Instance.stringTointArray(recipe.RevComment, 20);
        //        Array.Copy(revCommenttemp, 0, shorts, 50, 20);
        //        shorts[80] = recipe.LiftSpeedPre;
        //        shorts[82] = recipe.LiftSpeedUp1;
        //        shorts[84] = recipe.LiftSpeedUp2;
        //        shorts[86] = recipe.LiftSpeedDown1;
        //        shorts[88] = recipe.LiftSpeedDown2;

        //        shorts[70] = (short)recipe.PreSCRecipeList.Sum(para => para.Time);
        //        shorts[71] = (short)recipe.SCRecipeList.Sum(para => para.Time);
        //        shorts[72] = (short)recipe.PostSCRecipeList.Sum(para => para.Time);
        //        shorts[73] = (short)(shorts[70] + shorts[71] + shorts[72] + shorts[73]);
        //        short[] PreSCRecipeList = new short[100];
        //        ParallelLoopResult comState = Parallel.ForEach(recipe.PreSCRecipeList, (item) =>
        //        {
        //            int t = 0;
        //            short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);
        //            int Step = (steptemp - 1) * 20;
        //            PreSCRecipeList[Step] = steptemp;
        //            PreSCRecipeList[Step + 1] = Convert.ToInt16(item.Time);
        //            PreSCRecipeList[Step + 2] = (short)(item.SlowLeak ? 1 : 0);
        //            PreSCRecipeList[Step + 3] = (short)(item.QDR ? 1 : 0);
        //            PreSCRecipeList[Step + 4] = (short)(item.Shower ? 1 : 0);
        //            PreSCRecipeList[Step + 5] = (short)(item.Bubble ? 1 : 0);
        //            PreSCRecipeList[Step + 6] = (short)(item.Agitation ? 1 : 0);
        //            PreSCRecipeList[Step + 7] = (short)(item.ResistivityCheck ? 1 : 0);
        //            PreSCRecipeList[Step + 8] = (short)(item.N2Bubble ? 1 : 0);
        //        });
        //        Array.Copy(PreSCRecipeList, 0, shorts, 100, 100);
        //        short[] SCRecipeList = new short[600];
        //        ParallelLoopResult comState1 = Parallel.ForEach(recipe.SCRecipeList, (item) =>
        //        {
        //            int t = 0;
        //            short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);
        //            int Step = (steptemp - 1) * 20;
        //            SCRecipeList[Step] = steptemp;
        //            SCRecipeList[Step + 1] = Convert.ToInt16(item.Time);
        //            SCRecipeList[Step + 2] = (short)(item.SlowLeak ? 1 : 0);
        //            SCRecipeList[Step + 3] = (short)(item.QDR ? 1 : 0);
        //            SCRecipeList[Step + 4] = (short)(item.Shower ? 1 : 0);
        //            SCRecipeList[Step + 5] = (short)(item.Bubble ? 1 : 0);
        //            SCRecipeList[Step + 6] = (short)(item.Agitation ? 1 : 0);
        //            SCRecipeList[Step + 7] = (short)(item.ResistivityCheck ? 1 : 0);
        //            SCRecipeList[Step + 8] = (short)(item.N2Bubble ? 1 : 0);
        //        });
        //        Array.Copy(SCRecipeList, 0, shorts, 200, 600);
        //        short[] postSCRecipeList = new short[100];
        //        ParallelLoopResult comState2 = Parallel.ForEach(recipe.PostSCRecipeList, (item) =>
        //        {
        //            int t = 0;
        //            short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);
        //            int Step = (steptemp - 1) * 20;
        //            postSCRecipeList[Step] = steptemp;
        //            postSCRecipeList[Step + 1] = Convert.ToInt16(item.Time);
        //            postSCRecipeList[Step + 2] = (short)(item.SlowLeak ? 1 : 0);
        //            postSCRecipeList[Step + 3] = (short)(item.QDR ? 1 : 0);
        //            postSCRecipeList[Step + 4] = (short)(item.Shower ? 1 : 0);
        //            postSCRecipeList[Step + 5] = (short)(item.Bubble ? 1 : 0);
        //            postSCRecipeList[Step + 6] = (short)(item.Agitation ? 1 : 0);
        //            postSCRecipeList[Step + 7] = (short)(item.ResistivityCheck ? 1 : 0);
        //            postSCRecipeList[Step + 8] = (short)(item.N2Bubble ? 1 : 0);
        //        });
        //        Array.Copy(postSCRecipeList, 0, shorts, 800, 100);
        //        if (PLCSelect.Instance.SelectPLC(plcEnum).Write(startAddress, shorts).IsSuccess)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ee)
        //    {
        //        return false;
        //    }
        //}
    }
}
