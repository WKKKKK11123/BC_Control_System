using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    public class WaferSummary
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BatchCount { get; set; }
        public int TotalWaferCount { get; set; }
    }
}
