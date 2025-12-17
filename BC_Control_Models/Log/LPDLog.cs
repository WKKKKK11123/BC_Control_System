using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualData_LPD")]
    public class LPDLog
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public string BatchID { get; set; } = "1";
        public string Temp1 { get; set; } = "1";
        public string Temp2 { get; set; } = "1";
        public string Temp3 { get; set; } = "0";
        public string Temp4 { get; set; } = "0";
        public string Temp5 { get; set; } = "0";
        public string Temp6 { get; set; } = "0";
        public string Temp7 { get; set; } = "0";
        public string Temp8 { get; set; } = "0";
        public string Temp9 { get; set; } = "0";
        public string Temp10 { get; set; } = "0";
        public string Temp11 { get; set; } = "0";
        public string Temp12 { get; set; } = "0";
        public string Cheminal1 { get; set; } = "0";
        public string Concentration1 { get; set; } = "0";
        public string Chemica2 { get; set; } = "0";
        public string Concentration2 { get; set; } = "0";
        public string DIWPress { get; set; } = "0";
        public string DIWSupply { get; set; } = "0";
        public string Pressure { get; set; } = "0";
        public string IPAPurgeBlow { get; set; } = "0";
        public string IPABubblingBlow { get; set; } = "0";
        public string N2MonitorBlow { get; set; } = "0";
        public string ExN2Blow { get; set; } = "0";
        public string N2Pressure4 { get; set; } = "0";
        public string ChemicalFlowTH1 { get; set; } = "0";
        public string ChemicalFlowTH3 { get; set; } = "0";
        public string ExhaustPress { get; set; } = "0";
        public string ExhaustPress2 { get; set; } = "0";
        public string ExhaustPressAcid { get; set; } = "0";
        public string ExhaustPressAcid2 { get; set; } = "0";
        public string ExhaustPressIPA { get; set; } = "0";
        public string ExhaustPressIPA2 { get; set; } = "0";
        public string N2PressPurge200L { get; set; } = "0";
        public string N2PressPurge300L { get; set; } = "0";
        public string FFUDifPress1 { get; set; } = "0";
        public string FFUDifPress2 { get; set; } = "0";


    }
}
