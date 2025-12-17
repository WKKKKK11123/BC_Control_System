using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.LogVo;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualData_DHF")]
    public class DHFLog
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public string BatchID { get; set; }
        [Description("Temp1")]
        public string Temp1 { get; set; }
        [Description("Temp2")]
        public string Temp2 { get; set; }
        public string Cheminal1 { get; set; }
        [Description("Concentration1")]
        public string Concentration1 { get; set; }
        public string CheminalSupply1 { get; set; }
        [Description("CheminalSupplySum1")]
        public string CheminalSupplySum1 { get; set; }
        [Description("CheminalRepFlow1")]
        public string CheminalRepFlow1 { get; set; }
        //public string Cheminal2 { get; set; }
        //public string Concentration2 { get; set; }
        //public string CheminalSupply2 { get; set; }
        //public string CheminalSupplySum2 { get; set; }
        //public string CheminalRepFlow2 { get; set; }
        //public string PumpFlow { get; set; }
        public string DIWSupply { get; set; }
        public string DIWSupplySum { get; set; }
        public string DIWRepFlow { get; set; }
        public string ExhausLine { get; set; }
        //public string CoolingFlowInTank { get; set; }
        public string ExhaustPress { get; set; }
        public string ExhaustPress2 { get; set; }
    }
}