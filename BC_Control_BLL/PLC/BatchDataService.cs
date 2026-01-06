using BC_Control_BLL.Services;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using BC_Control_Models.Log;
using Dm.util;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_BLL.PLC
{
    public class BatchDataService
    {
        private readonly IBenchStationEntity _stations;
        private List<string> TankInProcessPre;
        public event Action<ModuleStatus> UpdateModuleState;
        public List<ModuleStatus> BatchDataCollection { get; set; }
        public BatchDataService(IBenchStationEntity stations)
        {
            _stations = stations;
            UpdateModuleState = delegate { };
            BatchDataCollection = new List<ModuleStatus>();
            TankInProcessPre = new List<string>();
            GetBatchData();
        }
        public void UpdateBatchData()
        {
            try
            {

                for (int i = 0; i < BatchDataCollection.Count(); i++)
                {
                    ModuleStatus module = BatchDataCollection[i];
                    if (module.HasRecipe == false)
                    {
                        continue;
                    }
                    if (module.IsWafer.ActualValue == TankInProcessPre[i])
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(TankInProcessPre[i]))
                    {
                        continue;
                    }
                    if (module.DataID.ActualValue =="0")
                    {
                        continue;
                    }
                    if (module.IsWafer.ActualValue=="1" 
                        && !string.IsNullOrWhiteSpace(module.FlowRecipeName.ActualValue))
                    {
                        UpdateModuleState?.Invoke(module);
                    }
                    if (module.IsWafer.ActualValue == "0")
                    {
                        UpdateModuleState?.Invoke(module);
                    }
                    TankInProcessPre[i] = module.IsWafer.ActualValue;
                }
                
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 加载Batch Data 实体数据提供给外部使用
        /// </summary>
        private void GetBatchData()
        {
            List<ModuleStatus> temp = new List<ModuleStatus>();
            foreach (var module in _stations.Stations)
            {
                if (module.StationNo != 0 && (module.StationType == StationEnum.ProcessTank || module.StationType == StationEnum.BufferTank))
                {

                    BatchDataCollection.Add(new ModuleStatus()
                    {
                        StationNo = module.StationNo,
                        StationType = module.StationType,
                        StationName = module.StationName,
                        HasStatus = module.HasStatus,
                        HasParameter = module.HasParameter,
                        HasRecipe = module.HasRecipe,
                        ModuleName = $"Tank{module.StationNo}#{module.StationName}",
                        BatchID = module.BatchDataCollection.FirstOrDefault(para => para.ParameterName == "BatchID") ?? new StatusClass(),
                        FlowRecipeName = module.BatchDataCollection.FirstOrDefault(para => para.ParameterName == "FlowRecipeName") ?? new StatusClass(),
                        FlowRecipeStep = module.BatchDataCollection.FirstOrDefault(para => para.ParameterName == "FlowRecipeStep") ?? new StatusClass(),
                        UnitRecipeName = module.BatchDataCollection.FirstOrDefault(para => para.ParameterName == "UnitRecipeName") ?? new StatusClass(),
                        UnitRecipeAllTime = module.BatchDataCollection.Find(para => para.ParameterName == "UnitRecipeAllTime") ?? new StatusClass(),
                        UnitRecipeResidueTime = module.BatchDataCollection.Find(para => para.ParameterName == "UnitRecipeResidueTime") ?? new StatusClass(),
                        UnitRecipeOverTime = module.BatchDataCollection.Find(para => para.ParameterName == "UnitRecipeOverTime") ?? new StatusClass(),
                        UnitRecipeStep = module.BatchDataCollection.Find(para => para.ParameterName == "UnitRecipeStep") ?? new StatusClass(),
                        UnitRecipeStepTime = module.BatchDataCollection.Find(para => para.ParameterName == "UnitRecipeStepTime") ?? new StatusClass(),
                        IsWafer = module.BatchDataCollection.Find(para => para.ParameterName == "IsWafer") ?? new StatusClass(),
                        DataID = module.BatchDataCollection.Find(para => para.ParameterName == "DataID") ?? new StatusClass(),
                    });
                }
            }
            BatchDataCollection.Reverse();
            TankInProcessPre = BatchDataCollection.Select(src => src.IsWafer.ActualValue).ToList();
        }
    }
}
