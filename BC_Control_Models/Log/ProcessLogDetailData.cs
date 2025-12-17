using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.BenchConfig;
using SqlSugar;

namespace BC_Control_Models.Log
{
    [SugarTable("TankProcess")]
    public class TankProcess:IStationConfig
    {        
        public int DataId { get; set; }
        public string ModuleRecipeName { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [SugarColumn(ColumnName = "TankName")]
        public string StationName { get; set; } = "";
        public int StationNo { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string FullInfo => $"Tank{StationNo}_{StationName}";
        [SugarColumn(IsIgnore = true)]
        public StationEnum StationType { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool HasStatus { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool HasParameter { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool HasRecipe { get; set; }
    }
    [SugarTable("TankParameter")]
    public class TankParameter
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] // ✅ 主键，自增
        public int Id { get; set; }
        public int TankProcessId { get; set; }
        public DateTime ParamTime { get; set; }
        [Description("CoolingTemp")]
        public float CoolingTemp { get; set; }
        [Description("ExhaustPressure1_1")]
        public float ExhaustPressure1_1 { get; set; }
        [Description("ExhaustPressure1_2")]
        public float ExhaustPressure1_2 { get; set; }
        [Description("FlowRate1")]
        public float FlowRate1 { get; set; }
        [Description("FFUDiffPressure1")]
        public float FFUDiffPressure1 { get; set; }
        [Description("ProcessTemp")]
        public float ProcessTemp { get; set; }
        [Description("U_sonic")]
        public float U_sonic { get; set; }
        [Description("WaterTankTemp")]
        public float WaterTankTemp { get; set; }
    }
}
