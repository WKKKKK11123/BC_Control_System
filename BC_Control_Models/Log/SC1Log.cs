using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualData_SC1")]
    public class SC1Log
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public string BatchID { get; set; }
        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public string Temp4 { get; set; }
        public string Cheminal1 { get; set; }
        public string Concentration1 { get; set; }
        public string CheminalSupply1 { get; set; }
        public string CheminalSupplySum1 { get; set; }
        public string CheminalRepFlow1 { get; set; }
        public string Cheminal2 { get; set; }
        public string Concentration2 { get; set; }
        public string CheminalSupply2 { get; set; }
        public string CheminalSupplySum2 { get; set; }
        public string CheminalRepFlow2 { get; set; }
        public string PumpFlow { get; set; }
        public string DIWSupply { get; set; }
        public string DIWSupplySum { get; set; }
        public string DIWRepFlow { get; set; }
        public string ExhausLine { get; set; }
        public string CoolingFlowInTank { get; set; }
        public string ExhaustPress { get; set; }
        public string ExhaustPress2 { get; set; }
        public string DSMIntensity1 { get; set; }
        public string DSMIntensity2 { get; set; }
        public string DSMOutput1 { get; set; }
        public string DSMOutput2 { get; set; }
        public string FFUDiffPress1 { get; set; }
        public string FFUDiffPress2 { get;set; }


    }
}
