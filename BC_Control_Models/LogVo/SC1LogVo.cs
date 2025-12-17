using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.LogVo
{
    public class SC1LogVo
    {
        public SC1LogVo() 
        {
            foreach (var item in this.GetType().GetProperties())
            {
                item.SetValue(this, new BaseAddress());
            }
        }
        public BaseAddress BatchID { get; set; }
        public BaseAddress Temp1 { get; set; }
        public BaseAddress Temp2 { get; set; }
        public BaseAddress Temp3 { get; set; }
        public BaseAddress Temp4 { get; set; }
        public BaseAddress Cheminal1 { get; set; }
        public BaseAddress Concentration1 { get; set; }
        public BaseAddress CheminalSupply1 { get; set; }
        public BaseAddress CheminalSupplySum1 { get; set; }
        public BaseAddress CheminalRepFlow1 { get; set; }
        public BaseAddress Cheminal2 { get; set; }
        public BaseAddress Concentration2 { get; set; }
        public BaseAddress CheminalSupply2 { get; set; }
        public BaseAddress CheminalSupplySum2 { get; set; }
        public BaseAddress CheminalRepFlow2 { get; set; }
        public BaseAddress PumpFlow { get; set; }
        public BaseAddress DIWSupply { get; set; }
        public BaseAddress DIWSupplySum { get; set; }
        public BaseAddress DIWRepFlow { get; set; }
        public BaseAddress ExhausLine { get; set; }
        public BaseAddress CoolingFlowInTank { get; set; }
        public BaseAddress ExhaustPress { get; set; }
        public BaseAddress ExhaustPress2 { get; set; }
        public BaseAddress DSMIntensity1 { get; set; }
        public BaseAddress DSMIntensity2 { get; set; }
        public BaseAddress DSMOutput1 { get; set; }
        public BaseAddress DSMOutput2 { get; set; }
        public BaseAddress FFUDiffPress1 { get; set; }
        public BaseAddress FFUDiffPress2 { get; set; }
    }
}
