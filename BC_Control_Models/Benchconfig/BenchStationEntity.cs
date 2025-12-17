using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.BenchConfig
{
    public class BenchStationEntity : IBenchStationEntity
    {
        public List<StationCollection> Stations { get; set; }
    }
}
