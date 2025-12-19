using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualDataView_TK2")]
    public class TK2DataLogClass : IInsertTime
    {
        public DateTime InsertTime { get; set; }
        [Description("ShowerFlowRate")]
        public string ShowerFlowRate { get; set; }
        [Description("DIWFlowRate")]
        public string DIWFlowRate { get; set; }
        [Description("WaterTankTemp")]
        public string WaterTankTemp { get; set; }
        [Description("Resistivity")]
        public string Resistivity { get; set; }
        [Description("Dsonic")]
        public string Dsonic { get; set; }
        [Description("FFUDiffPressure1")]
        public string FFUDiffPressure1 { get; set; }
        [Description("FFUDiffPressure2")]
        public string FFUDiffPressure2 { get; set; }
        public string Step { get; set; }
    }
}
