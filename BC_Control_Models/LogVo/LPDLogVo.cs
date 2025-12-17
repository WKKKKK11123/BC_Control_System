using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.LogVo;

namespace BC_Control_Models.Log
{
    public class LPDLogVo
    {
        public LPDLogVo() 
        {
            foreach (var item in this.GetType().GetProperties())
            {               
                item.SetValue(this,new BaseAddress());
            }
        }

        public BaseAddress BatchID { get; set; }
        public BaseAddress Temp1 { get; set; }
        public BaseAddress Temp2 { get; set; }
        public BaseAddress Temp3 { get; set; }
        public BaseAddress Temp4 { get; set; }
        public BaseAddress Temp5 { get; set; }
        public BaseAddress Temp6 { get; set; }
        public BaseAddress Temp7 { get; set; }
        public BaseAddress Temp8 { get; set; }
        public BaseAddress Temp9 { get; set; }
        public BaseAddress Temp10 { get; set; }
        public BaseAddress Temp11 { get; set; }
        public BaseAddress Temp12 { get; set; }
        public BaseAddress Cheminal1 { get; set; }
        public BaseAddress Concentration1 { get; set; }
        public BaseAddress Chemica2 { get; set; }
        public BaseAddress Concentration2 { get; set; }
        public BaseAddress DIWPress { get; set; }
        public BaseAddress DIWSupply { get; set; }
        public BaseAddress Pressure { get; set; }
        public BaseAddress IPAPurgeBlow { get; set; }
        public BaseAddress IPABubblingBlow { get; set; }
        public BaseAddress N2MonitorBlow { get; set; }
        public BaseAddress ExN2Blow { get; set; }
        public BaseAddress N2Pressure4 { get; set; }
        public BaseAddress ChemicalFlowTH1 { get; set; }
        public BaseAddress ChemicalFlowTH3 { get; set; }
        public BaseAddress ExhaustPress { get; set; }
        public BaseAddress ExhaustPress2 { get; set; }
        public BaseAddress ExhaustPressAcid { get; set; }
        public BaseAddress ExhaustPressAcid2 { get; set; }
        public BaseAddress ExhaustPressIPA { get; set; }
        public BaseAddress ExhaustPressIPA2 { get; set; }
        public BaseAddress N2PressPurge200L { get; set; }
        public BaseAddress N2PressPurge300L { get; set; }
        public BaseAddress FFUDifPress1 { get; set; }
        public BaseAddress FFUDifPress2 { get; set; }



    }
}
