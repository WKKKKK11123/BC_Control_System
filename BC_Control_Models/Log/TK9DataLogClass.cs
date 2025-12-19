using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualDataView_TK9")]
    public class TK9DataLogClass : IInsertTime
    {
        public DateTime InsertTime { get; set; }
        [Description("ProcessTemp")]
        public string ProcessTemp { get; set; }
        [Description("ExhaustPressure1_1")]
        public string ExhaustPressure1_1 { get; set; }
        [Description("ExhaustPressure1_2")]
        public string ExhaustPressure1_2 { get; set; }
        [Description("FFUDiffPressure1")]
        public string FFUDiffPressure1 { get; set; }
        [Description("SYS9070FlowRate1")]
        public string SYS9070FlowRate1 { get; set; }
        public string Step { get; set; }
    }
}
