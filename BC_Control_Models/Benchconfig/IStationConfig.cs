using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.BenchConfig
{
    public interface IStationConfig
    {
        string StationName { get; set; }
        StationEnum StationType { get; set; } 
        int StationNo { get; set; }
        bool HasStatus { get; set; }
        bool HasParameter { get; set; }
        bool HasRecipe { get; set; }
    }
}
