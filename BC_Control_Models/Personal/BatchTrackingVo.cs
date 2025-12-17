using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    public class BatchTrackingVo
    {
        public string InProcess { get; set; }
        public int BatchID { get; set; }
        public string RecipeName { get; set; }
        public string SourceCarrier { get; set; }
        public string DistinationCarrier { get; set; }
        public string DeleteBatch { get; set; }
        public string MoveBatch { get; set; }
        public string ProcessPause { get; set; }
        public string ProcessRestart { get; set; }
        public string ProcessStop { get; set; }
        public PlcEnum PLC { get; set; }
    }
}
