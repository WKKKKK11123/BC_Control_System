using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public interface ITK2DataLog
    {
        string TK2ProcessTemp { get; set; }
        string TK2CoolingTemp { get; set; }
        string TK2WaterTankTemp { get; set; }
        string TK2U_sonic { get; set; }
        string TK2ExhaustPressure2_1 { get; set; }
        string TK2ExhaustPressure2_2 { get; set; }
        string TK2FFUDiffPressure1 { get; set; }
        string SYS9070FlowRate2 { get; set; }
    }
}
