using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BC_Control_Models.RecipeModel;
using BC_Control_Models;
using BC_Control_Models.RecipeModel.RecipeBase;
using Org.BouncyCastle.Utilities;
using BC_Control_BLL.recipedownload;
using BC_Control_Models.RecipeModel.RecipeEntity;

namespace BC_Control_Helper.RecipeDownLoad
{
    public class LPDRecipeControl : ModuleRecipeServiceBase<LPDTankModuleRecipeBase, LPDRecipeClassBase>
    {
        public LPDRecipeControl(IPLCHelper plcHelper) : base(plcHelper)
        {

        }
        public override short[] RecipeStepToShortArray(LPDTankModuleRecipeBase t)
        {
            try
            {
                short[] shorts = new short[20];
                short steptemp = Convert.ToInt16(Regex.Match(t.Step, @"-?\d+(\.\d+)?").Value);
                int Step = (steptemp - 1) * 20;
                var shortstemp = floatTointArray(steptemp);
                Array.Copy(shortstemp, 0, shorts, 0, shortstemp.Length);
                shortstemp = floatTointArray(t.Time);
                Array.Copy(shortstemp, 0, shorts, 2, shortstemp.Length);
                shortstemp = floatTointArray((int)t.DIW);
                Array.Copy(shortstemp, 0, shorts, 4, shortstemp.Length);
                shortstemp = floatTointArray((int)t.BlowPattern);
                Array.Copy(shortstemp, 0, shorts, 6, shortstemp.Length);
                shortstemp = floatTointArray(t.N2Flow);
                Array.Copy(shortstemp, 0, shorts, 8, shortstemp.Length);
                shortstemp = floatTointArray(t.IPAN2Flow);
                Array.Copy(shortstemp, 0, shorts, 10, shortstemp.Length);
                shortstemp = floatTointArray(t.QDR ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 12, shortstemp.Length);
                shortstemp = floatTointArray(t.Vacuum ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 14, shortstemp.Length);
                shortstemp = floatTointArray((int)t.LFR);
                Array.Copy(shortstemp, 0, shorts, 14, shortstemp.Length);
                shortstemp = floatTointArray(t.ResistivityCheck ? 1 : 0);
                Array.Copy(shortstemp, 0, shorts, 16, shortstemp.Length);
                shortstemp = floatTointArray(t.LFRSpeed);
                Array.Copy(shortstemp, 0, shorts, 18, shortstemp.Length);
                //shorts[Step + 4] = (short)t.DSM;
                return shorts;
                //                PreSCRecipeList[Step] = steptemp;//Step Number
                //                PreSCRecipeList[Step + 1] = Convert.ToInt16(item.Time);//Step Time
                //                PreSCRecipeList[Step + 2] = (short)(item.DIW);
                //                PreSCRecipeList[Step + 3] = (short)(item.BlowPattern);
                //                PreSCRecipeList[Step + 4] = (short)(item.N2Flow);
                //                PreSCRecipeList[Step + 5] = (short)(item.IPAN2Flow);
                //                PreSCRecipeList[Step + 6] = (short)(item.QDR ? 1 : 0);
                //                PreSCRecipeList[Step + 7] = (short)(item.Vacuum ? 1 : 0);
                //                PreSCRecipeList[Step + 8] = (short)(item.LFR);
                //                PreSCRecipeList[Step + 9] = (short)(item.ResistivityCheck ? 1 : 0);
                //                PreSCRecipeList[Step + 10] = (short)(item.LFRSpeed);
            }
            catch (Exception)
            {
                throw;
            }

        }
        //    public string path { get; set; }
        //    public LPDRecipeClassBase recipe { get; set; }
        //    public LPDRecipeControl()
        //    {
        //        recipe = new LPDRecipeClassBase();
        //        path = "";
        //    }
        //    public void GetRecipe()
        //    {
        //        //string tempRecipe = File.ReadAllText(path);
        //        recipe = JSONHelper.JSONToEntity<LPDRecipeClassBase>(path);

        //    }
        //    public short[] stringTointArray(string s, int specifiedLength)
        //    {
        //        try
        //        {
        //            string str = s;
        //            byte[] byteArray = Encoding.UTF8.GetBytes(str);
        //            int lengthToProcess = Math.Min(byteArray.Length, specifiedLength * 2);
        //            byte[] truncatedArray = new byte[lengthToProcess];
        //            Array.Copy(byteArray, truncatedArray, lengthToProcess);
        //            short[] shortArray = new short[specifiedLength];
        //            for (int i = 0; i < lengthToProcess; i += 2)
        //            {
        //                // 组合 2 个字节为一个 short
        //                short value = 0;
        //                for (int j = 0; j < 2 && (i + j) < byteArray.Length; j++)
        //                {
        //                    value |= (short)(byteArray[i + j] << (8 * j)); // 按字节合并成一个 short
        //                }
        //                shortArray[i / 2] = value;
        //            }
        //            return shortArray;
        //        }
        //        catch (Exception ee)
        //        {

        //            return new short[specifiedLength];
        //        }
        //    }
        //    private short[] IRevisioninterfaceToShortArray(IRevisioninterface revisioninterface)
        //    {
        //        try
        //        {
        //            short[] tempshort= new short[70];
        //            short[] nametemp = stringTointArray(recipe.Name, 20);//Name
        //            Array.Copy(nametemp, 0, tempshort, 0, 20);
        //            short[] nameComment =stringTointArray(recipe.Comment, 20);//Comment
        //            Array.Copy(nameComment, 0, tempshort, 20, 20);
        //            short[] revisionNotemp = stringTointArray(recipe.RevisionNo, 10);//Revisi N0
        //            Array.Copy(revisionNotemp, 0, tempshort, 40, 10);
        //            short[] revCommenttemp = stringTointArray(recipe.RevComment, 20);//Rev. Comment
        //            Array.Copy(revCommenttemp, 0, tempshort, 50, 20);

        //            return tempshort;   
        //        }
        //        catch (Exception ex)
        //        {

        //            return new short[70];
        //        }
        //    }
        //    public bool DownLoad(string startAddress, PlcEnum plcEnum = PlcEnum.PLC1)
        //    {
        //        try
        //        {
        //            GetRecipe();
        //            short[] shorts = new short[1000];
        //            if (!PLCSelect.Instance.SelectPLC(plcEnum).Write(startAddress, shorts).IsSuccess)
        //            {
        //                return false;
        //            }

        //            short[] nametemp = GlobalMethodHelper.Instance.stringTointArray(recipe.Name, 20);//Name
        //            Array.Copy(nametemp, 0, shorts, 0, 20);
        //            short[] nameComment = GlobalMethodHelper.Instance.stringTointArray(recipe.Comment, 20);//Comment
        //            Array.Copy(nameComment, 0, shorts, 20, 20);
        //            short[] revisionNotemp = GlobalMethodHelper.Instance.stringTointArray(recipe.RevisionNo, 10);//Revisi N0
        //            Array.Copy(revisionNotemp, 0, shorts, 40, 10);
        //            short[] revCommenttemp = GlobalMethodHelper.Instance.stringTointArray(recipe.RevComment, 20);//Rev. Comment
        //            Array.Copy(revCommenttemp, 0, shorts, 50, 20);

        //            shorts[80] = recipe.LiftSpeedPre;
        //            shorts[82] = recipe.LiftSpeedUp1;
        //            shorts[84] = recipe.LiftSpeedUp2;
        //            shorts[86] = recipe.LiftSpeedDown1;
        //            shorts[88] = recipe.LiftSpeedDown2;

        //            var PreRecipeList = recipe.RecipeStepCollection.Where(filter => filter.StepType == ProcessStepEnum.PreStep);
        //            var RecipeList = recipe.RecipeStepCollection.Where(filter => filter.StepType == ProcessStepEnum.Step);
        //            var PostRecipeList = recipe.RecipeStepCollection.Where(filter => filter.StepType == ProcessStepEnum.PostStep);

        //            shorts[70] = (short)PreRecipeList.Sum(para => para.Time);
        //            shorts[71] = (short)RecipeList.Sum(para => para.Time);
        //            shorts[72] = (short)PostRecipeList.Sum(para => para.Time);
        //            shorts[73] = (short)(shorts[70] + shorts[71] + shorts[72] + shorts[73]);


        //            //Pre--------------------------------------------------------------------------------
        //            short[] PreSCRecipeList = new short[100];
        //            ParallelLoopResult comState = Parallel.ForEach(PreRecipeList, (item) =>
        //            {
        //                short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);//Step
        //                int Step = (steptemp - 1) * 20;//Step Address
        //                PreSCRecipeList[Step] = steptemp;//Step Number
        //                PreSCRecipeList[Step + 1] = Convert.ToInt16(item.Time);//Step Time
        //                PreSCRecipeList[Step + 2] = (short)(item.DIW);
        //                PreSCRecipeList[Step + 3] = (short)(item.BlowPattern);
        //                PreSCRecipeList[Step + 4] = (short)(item.N2Flow);
        //                PreSCRecipeList[Step + 5] = (short)(item.IPAN2Flow);
        //                PreSCRecipeList[Step + 6] = (short)(item.QDR ? 1 : 0);
        //                PreSCRecipeList[Step + 7] = (short)(item.Vacuum ? 1 : 0);
        //                PreSCRecipeList[Step + 8] = (short)(item.LFR);
        //                PreSCRecipeList[Step + 9] = (short)(item.ResistivityCheck ? 1 : 0);
        //                PreSCRecipeList[Step + 10] = (short)(item.LFRSpeed);

        //            });
        //            Array.Copy(PreSCRecipeList, 0, shorts, 100, 100);
        //            //Process--------------------------------------------------------------------------------
        //            short[] SCRecipeList = new short[600];
        //            ParallelLoopResult comState1 = Parallel.ForEach(RecipeList, (item) =>
        //            {
        //                short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);
        //                int Step = (steptemp - 1) * 20;
        //                SCRecipeList[Step] = steptemp;
        //                SCRecipeList[Step + 1] = Convert.ToInt16(item.Time);
        //                SCRecipeList[Step + 2] = (short)(item.DIW);
        //                SCRecipeList[Step + 3] = (short)(item.BlowPattern);
        //                SCRecipeList[Step + 4] = (short)(item.N2Flow);
        //                SCRecipeList[Step + 5] = (short)(item.IPAN2Flow);
        //                SCRecipeList[Step + 6] = (short)(item.QDR ? 1 : 0);
        //                SCRecipeList[Step + 7] = (short)(item.Vacuum ? 1 : 0);
        //                SCRecipeList[Step + 8] = (short)(item.LFR);
        //                SCRecipeList[Step + 9] = (short)(item.ResistivityCheck ? 1 : 0);
        //                SCRecipeList[Step + 10] = (short)(item.LFRSpeed);
        //            });
        //            Array.Copy(SCRecipeList, 0, shorts, 200, 600);
        //            //Post--------------------------------------------------------------------------------
        //            short[] postSCRecipeList = new short[100];
        //            ParallelLoopResult comState2 = Parallel.ForEach(PostRecipeList, (item) =>
        //            {
        //                short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);
        //                int Step = (steptemp - 1) * 20;
        //                postSCRecipeList[Step] = steptemp;
        //                postSCRecipeList[Step + 1] = Convert.ToInt16(item.Time);
        //                postSCRecipeList[Step + 2] = (short)(item.DIW);
        //                postSCRecipeList[Step + 3] = (short)(item.BlowPattern);
        //                postSCRecipeList[Step + 4] = (short)(item.N2Flow);
        //                postSCRecipeList[Step + 5] = (short)(item.IPAN2Flow);
        //                postSCRecipeList[Step + 6] = (short)(item.QDR ? 1 : 0);
        //                postSCRecipeList[Step + 7] = (short)(item.Vacuum ? 1 : 0);
        //                postSCRecipeList[Step + 8] = (short)(item.LFR);
        //                postSCRecipeList[Step + 9] = (short)(item.ResistivityCheck ? 1 : 0);
        //                postSCRecipeList[Step + 10] = (short)(item.LFRSpeed);
        //            });
        //            Array.Copy(postSCRecipeList, 0, shorts, 800, 100);
        //            if (PLCSelect.Instance.SelectPLC(plcEnum).Write(startAddress, shorts).IsSuccess)
        //            {
        //                return true;
        //            }
        //            return false;
        //        }
        //        catch (Exception ee)
        //        {
        //            return false;
        //        }
        //    }
        //}

    }
}
