using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class BatchTrackBase
    {
        public StatusClass InProcess { get; set; }
        public int BatchID { get; set; }
        public StatusClass RecipeName { get; set; }
        public StatusClass SourceCarrier { get; set; }
        public StatusClass DistinationCarrier { get; set; }
        public StatusClass DeleteBatch { get; set; }
        public StatusClass MoveBatch { get; set; }
        public StatusClass ProcessPause { get; set; }
        public StatusClass ProcessRestart { get; set; }


    }
}
