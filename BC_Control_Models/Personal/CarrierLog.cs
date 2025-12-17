using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    public class CarrierLog
    {
        public int No { get; set; }
        public string CarrierID { get; set; }
        public string CarrierState { get; set; }
        public string ProcessState { get; set; }
        public string ErrorState { get; set; }
        public string Wafers { get; set; }
        public string WaferMap { get; set; }
        public string Location { get; set; }
        public string LoadingPort { get; set; }


    }
}
