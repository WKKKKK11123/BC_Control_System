using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualDataView_TK1")]
    public class TK1DataLogClass : ITK1DataLog, IInsertTime
    {
        public DateTime InsertTime { get; set; }
        public string Step { get; set; } = "";
        [Description("ProcessTemp")]
        public string ProcessTemp { get; set; } = "";
        [Description("ExhaustPressure1_1")]
        public string ExhaustPressure1_1 { get; set; } = "";
        [Description("ExhaustPressure1_2")]
        public string ExhaustPressure1_2 { get; set; } = "";
        [Description("FFUDiffPressure1")]
        public string FFUDiffPressure1 { get; set; } = "";
        [Description("SYS9070FlowRate1")]
        public string SYS9070FlowRate1 { get; set; } = "";
        
    }
}
