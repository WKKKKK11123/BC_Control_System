using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.BenchConfig
{
    public interface IStatusCollection
    {
        List<DataClass> ModuleDataCollection { get; set; }
        List<DataClass> IOViewDataCollection { get; set; }
        List<StatusClass> ModuleStatus { get; set; }
        List<StatusClass> BatchDataCollection { get; set; }
        List<DataClass> ControlDataCollection { get; set; }
    }
}
