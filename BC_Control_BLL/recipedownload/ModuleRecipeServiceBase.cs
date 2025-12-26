using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.RecipeModel;
using BC_Control_Models.RecipeModel.RecipeBase;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZCCommunication.Profinet.Melsec.Helper;

namespace BC_Control_BLL.recipedownload
{
    public abstract class ModuleRecipeServiceBase<T, TEntity> : IRecipeDownLoad
        where TEntity : ModuleRecipeClassBase<T>, new()
        where T : class, IRecipeStep, new()
    {
        private readonly IPLCHelper _helper;
        public TEntity RecipeEntity { get; set; }
        public string path { get; set; }
        public ModuleRecipeServiceBase(IPLCHelper helper)
        {
            _helper= helper;
            RecipeEntity = new TEntity();
            path = "";
        }
        public virtual bool DownLoad(string startAddress, PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                GetRecipe();
                short[] shorts = new short[1000];
                short[] IRevisionShorts = IRevisioninterfaceToShortArray(RecipeEntity);
                Array.Copy(IRevisionShorts, 0, shorts, 0, 70);
                short[] ILiftShorts = ILiftSpeedinterfaceToShortArray(RecipeEntity);
                Array.Copy(ILiftShorts, 0, shorts, 80, 10);
                var PreRecipeList = RecipeEntity.RecipeStepCollection.Where(filter => filter.StepType == ProcessStepEnum.PreStep).ToList();
                var RecipeList = RecipeEntity.RecipeStepCollection.Where(filter => filter.StepType == ProcessStepEnum.Step).ToList();
                var PostRecipeList = RecipeEntity.RecipeStepCollection.Where(filter => filter.StepType == ProcessStepEnum.PostStep).ToList();
                short[] temp1 = RecipeCollectionToShortArray(PreRecipeList, 100);
                Array.Copy(temp1, 0, shorts, 100, 100);
                short[] temp2 = RecipeCollectionToShortArray(RecipeList, 600);
                Array.Copy(temp2, 0, shorts, 200, 600);
                short[] temp3 = RecipeCollectionToShortArray(PostRecipeList, 100);
                Array.Copy(temp3, 0, shorts, 800, 100);
                //总时间计算
                int prestepTime = PreRecipeList.Select(prop => prop.Time).Sum();
                short[] tempTime = floatTointArray(prestepTime);
                Array.Copy(tempTime, 0, shorts, 908, 2);
                int stepTime = RecipeList.Select(prop => prop.Time).Sum();
                tempTime = floatTointArray(stepTime);
                Array.Copy(tempTime, 0, shorts, 918, 2);
                int poststepTime = PostRecipeList.Select(prop => prop.Time).Sum();
                tempTime = floatTointArray(poststepTime);
                Array.Copy(tempTime, 0, shorts, 928, 2);
                short TotalTime = (short)(prestepTime + stepTime + poststepTime);
                shorts[70]= (short)prestepTime;
                var result = _helper.SelectPLC(plcEnum).Write(startAddress, shorts);
                if (result.IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public abstract short[] RecipeStepToShortArray(T t);
        public virtual short[] RecipeCollectionToShortArray(List<T> RecipeList, int Lenth)
        {
            try
            {
                short[] shorts = new short[Lenth];
                ParallelLoopResult comState = Parallel.ForEach(RecipeList, (item) =>
                {
                    short steptemp = Convert.ToInt16(Regex.Match(item.Step, @"-?\d+(\.\d+)?").Value);//Step
                    int Step = (steptemp - 1) * 20;//Step Address
                    short[] tempshort = RecipeStepToShortArray(item);
                    Array.Copy(tempshort, 0, shorts, Step, 20);
                });
                return shorts;
            }
            catch (Exception)
            {
                return new short[Lenth];
            }

        }
        public virtual void GetRecipe()
        {
            RecipeEntity = JSONHelper.JSONToEntity<TEntity>(path);

        }
        public virtual short[] IRevisioninterfaceToShortArray(IRevisioninterface revisioninterface)
        {
            try
            {
                short[] tempshort = new short[70];
                short[] nametemp = stringTointArray(revisioninterface.Name, 20);//Name
                Array.Copy(nametemp, 0, tempshort, 0, 20);
                short[] nameComment = stringTointArray(revisioninterface.Comment, 20);//Comment
                Array.Copy(nameComment, 0, tempshort, 20, 20);
                short[] revisionNotemp = stringTointArray(revisioninterface.RevisionNo, 10);//Revisi N0
                Array.Copy(revisionNotemp, 0, tempshort, 40, 10);
                short[] revCommenttemp = stringTointArray(revisioninterface.RevComment, 20);//Rev. Comment
                Array.Copy(revCommenttemp, 0, tempshort, 50, 20);
                return tempshort;
            }
            catch (Exception ex)
            {
                return new short[70];
            }
        }
        public virtual short[] ILiftSpeedinterfaceToShortArray(ILIftSpeedInterface liftSpeedInterface)
        {
            try
            {
                short[] tempshort = new short[10];
                var shortSpeedPre = floatTointArray((float)liftSpeedInterface.LiftSpeedPre);
                Array.Copy(shortSpeedPre, 0, tempshort, 0, 2);
                var shortSpeedUp1 = floatTointArray((float)liftSpeedInterface.LiftSpeedUp1);
                Array.Copy(shortSpeedPre, 0, tempshort, 2, 2);
                var shortSpeedUp2 = floatTointArray((float)liftSpeedInterface.LiftSpeedUp2);
                Array.Copy(shortSpeedPre, 0, tempshort, 4, 2);
                var shortSpeedDown1 = floatTointArray((float)liftSpeedInterface.LiftSpeedDown1);
                Array.Copy(shortSpeedPre, 0, tempshort, 6, 2);
                var shortLiftSpeedDown2 = floatTointArray((float)liftSpeedInterface.LiftSpeedDown2);
                Array.Copy(shortSpeedPre, 0, tempshort, 8, 2);
                return tempshort;
            }
            catch (Exception ex)
            {
                return new short[10];
            }
        }
        public short[] stringTointArray(string s, int specifiedLength)
        {
            try
            {
                string str = s;
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                int lengthToProcess = Math.Min(byteArray.Length, specifiedLength * 2);
                byte[] truncatedArray = new byte[lengthToProcess];
                Array.Copy(byteArray, truncatedArray, lengthToProcess);
                short[] shortArray = new short[specifiedLength];
                for (int i = 0; i < lengthToProcess; i += 2)
                {
                    // 组合 2 个字节为一个 short
                    short value = 0;
                    for (int j = 0; j < 2 && (i + j) < byteArray.Length; j++)
                    {
                        value |= (short)(byteArray[i + j] << (8 * j)); // 按字节合并成一个 short
                    }
                    shortArray[i / 2] = value;
                }
                return shortArray;
            }
            catch (Exception ee)
            {

                return new short[specifiedLength];
            }
        }
        public short[] floatTointArray(float f)
        {
            try
            {
                float floatNumber = f;  // 示例浮点数

                // 1. 将浮点数转换为字节数组（4字节）
                byte[] byteArray = BitConverter.GetBytes(floatNumber);

                // 2. 将字节数组转换为short数组（每2个字节转换为一个short）
                short[] shortArray = new short[2];  // 保证足够的空间来存储short

                for (int i = 0; i < byteArray.Length; i += 2)
                {
                    short value = 0;
                    for (int j = 0; j < 2 && (i + j) < byteArray.Length; j++)
                    {
                        value |= (short)(byteArray[i + j] << (8 * j));  // 按字节合并成一个short
                    }
                    shortArray[i / 2] = value;
                }
                return shortArray;
            }
            catch (Exception)
            {

                throw;
            }



        }

    }
}
