using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public interface ITK1DataLog
    {
        DateTime InsertTime { get; set; }
        string ProcessTemp { get; set; }          
        string ExhaustPressure1_1 { get; set; }
        string ExhaustPressure1_2 { get; set; }
        string FFUDiffPressure1 { get; set; }
        string SYS9070FlowRate1 { get; set; }
    }
}
