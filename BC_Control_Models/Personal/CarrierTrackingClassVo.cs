using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    public class CarrierTrackingClassVo
    {
        public bool InProcess { get; set; }
        public int No { get; set; }
        public string Lp_RFID { get; set; }
        public string Opener_RFID { get; set; }
        public string IDVerifcation { get; set; }
        public string LotMapVerifcatid { get; set; }
        //public string CarrierState { get; set; }
        public string PorcessState { get; set; }
        public string ErrorState { get; set; }
        public int Wafers { get; set; }
        public string WafersMap { get; set; }
        public string Location { get; set; }
        public string Loadingport { get; set; }
        public PlcEnum PLC { get; set; }
    }
}
