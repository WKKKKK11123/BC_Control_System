using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.BenchConfig
{
    public class StationCollection : IStationConfig, IParameterCollection, IStatusCollection
    {
        public string StationName { get; set; } = "";
        public StationEnum StationType { get; set; }
        public int StationNo { get; set; }
        public bool HasStatus { get; set; }
        public bool HasParameter { get; set; }
        public bool HasRecipe { get; set; }
        public List<DataClass> ModuleDataCollection { get; set; }=new List<DataClass>();
        public List<DataClass> IOViewDataCollection { get; set; } = new List<DataClass>();  
        public List<StatusClass> ModuleStatus { get; set; } = new List<StatusClass>();
        public List<StatusClass> BatchDataCollection { get; set; } = new List<StatusClass>();
        public List<DataClass> ControlDataCollection { get; set; } = new List<DataClass>();
        public List<ParameterModel> ParameterCollections { get; set; } = new List<ParameterModel>();

    }
}
