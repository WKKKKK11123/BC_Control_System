using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualDataView_TK11")]
    public class TK11DataLogClass : IInsertTime
    {
        public DateTime InsertTime { get; set; }
        public string Step { get; set; } = "";
        [Description("DIWFlowrate")]
        public string DIWFlowrate { get; set; } = "";
        [Description("Bubbler1N2BlowRate")]
        public string Bubbler1N2BlowRate { get; set; } = "";
        [Description("Bubbler2N2BlowRate")]
        public string Bubbler2N2BlowRate { get; set; } = "";
        [Description("Bubbler3N2BlowRate")]
        public string Bubbler3N2BlowRate { get; set; } = "";
        [Description("ProcessTemp")]
        public string ProcessTemp { get; set; } = "";
        [Description("Bubbler1Temp")]
        public string Bubbler1Temp { get; set; } = "";
        [Description("Bubbler2Temp")]
        public string Bubbler2Temp { get; set; } = "";
        [Description("Bubbler3Temp")]
        public string Bubbler3Temp { get; set; } = "";
        [Description("VaporTemp")]
        public string VaporTemp { get; set; } = "";
        [Description("N2HeartTemp")]
        public string N2HeartTemp { get; set; } = "";
        [Description("Resistivity")]
        public string Resistivity { get; set; } = "";
        [Description("ExhaustPressure")]
        public string ExhaustPressure { get; set; } = "";
        [Description("FFUDiffPressure1")]
        public string FFUDiffPressure1 { get; set; } = "";
        [Description("FFUDiffPressure2")]
        public string FFUDiffPressure2 { get; set; } = "";
    }
}
