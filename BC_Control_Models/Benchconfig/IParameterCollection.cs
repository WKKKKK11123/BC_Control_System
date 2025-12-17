using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.BenchConfig
{
    public interface IParameterCollection
    {
        List<ParameterModel> ParameterCollections { get; set; }
    }
}
