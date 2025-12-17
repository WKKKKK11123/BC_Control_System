using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.LogVo
{
    public class ActualDataClassVo
    {
        public DateTime InsertTime { get; set; }
        public StatusClass TK1ProcessTemp { get; set; }
        public StatusClass TK1CoolingTemp { get; set; }
        public StatusClass TK1WaterTankTemp { get; set; }
        public StatusClass TK1U_sonic { get; set; }
        public StatusClass TK1ExhaustPressure1_1 { get; set; }
        public StatusClass TK1ExhaustPressure1_2 { get; set; }
        public StatusClass TK1FFUDiffPressure1 { get; set; }
        public StatusClass SYS9070FlowRate1 { get; set; }
        public StatusClass TK2ProcessTemp { get; set; }
        public StatusClass TK2CoolingTemp { get; set; }
        public StatusClass TK2WaterTankTemp { get; set; }
        public StatusClass TK2U_sonic { get; set; }
        public StatusClass TK2ExhaustPressure1_1 { get; set; }
        public StatusClass TK2ExhaustPressure1_2 { get; set; }
        public StatusClass TK2FFUDiffPressure1 { get; set; }
        public StatusClass SYS9070FlowRate2 { get; set; }
    }
}
