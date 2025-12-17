using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    public class WTRIOClass
    {
        public DataClass VerticalEnable { get; set; }
        public DataClass VerticalInit { get; set; }
        public DataClass VerticalError { get; set; }
        public DataClass VerticalAlarm { get; set; }
        public DataClass VerticalBusy { get; set; }
        public DataClass VerticalAxisFLS { get; set; }
        public DataClass VerticalAxisDOG { get; set; }
        public DataClass VerticalAxisRLS { get; set; }


        public DataClass LateralEnable { get; set; }
        public DataClass LateralInit { get; set; }
        public DataClass LateralError { get; set; }
        public DataClass LateralAlarm { get; set; }
        public DataClass LateralBusy { get; set; }
        public DataClass LateralAxisFLS { get; set; }
        public DataClass LateralAxisDOG { get; set; }
        public DataClass LateralAxisRLS { get; set; }

        public DataClass ChuckOpen { get; set; }
        public DataClass ChuckClose { get; set; }

        public DataClass LateralAxisPos1 { get; set; }
        public DataClass LateralAxisPos2 { get; set; }
        public DataClass LateralAxisPos3 { get; set; }
        public DataClass LateralAxisPos4 { get; set; }
        public DataClass LateralAxisPos5 { get; set; }
        public DataClass LateralAxisPos6 { get; set; }
        public DataClass LateralAxisPos7 { get; set; }
        public DataClass LateralAxisPos8 { get; set; }
        public DataClass LateralAxisPos9 { get; set; }
        public DataClass LateralAxisPos10 { get; set; }
        public DataClass LateralAxisPos11 { get; set; }

        public DataClass VerticalAxisPos1 { get; set; }
        public DataClass VerticalAxisPos2 { get; set; }


    }
}
