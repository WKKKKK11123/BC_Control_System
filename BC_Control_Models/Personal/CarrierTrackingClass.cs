using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    public class CarrierTrackingClass
    {
        public StatusClass InProcess { get; set; }
        public int No { get; set; }
        public StatusClass CarrierID { get; set; }
        public StatusClass CarrierState { get; set; }
        public StatusClass PorcessState { get; set; }
        public StatusClass ErrorState { get; set; }
        public StatusClass Wafers { get; set; }
        public StatusClass WafersMap { get; set; }
        public StatusClass Location { get; set; }
        public StatusClass Loadingport { get; set; }
        public PlcEnum PLC { get; set; }
    }
}
