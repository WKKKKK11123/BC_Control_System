using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    [SugarTable("ActualData_DataLog")]
    public class ActualDataClass
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public string TK1ProcessTemp { get; set; }
        public string TK1CoolingTemp { get; set; }
        public string TK1WaterTankTemp { get; set; }
        public string TK1U_sonic { get; set; }
        public string TK1ExhaustPressure1_1 { get; set; }
        public string TK1ExhaustPressure1_2 { get; set; }
        public string TK1FFUDiffPressure1 { get; set; }
        public string SYS9070FlowRate1 { get; set; }
        public string TK2ProcessTemp { get; set; }
        public string TK2CoolingTemp { get; set; }
        public string TK2WaterTankTemp { get; set; }
        public string TK2U_sonic { get; set; }
        public string TK2ExhaustPressure2_1 { get; set; }
        public string TK2ExhaustPressure2_2 { get; set; }
        public string TK2FFUDiffPressure1 { get; set; }
        public string SYS9070FlowRate2 { get; set; }

    }
}
