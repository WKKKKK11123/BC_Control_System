using BC_Control_Models.BenchConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class ModuleStatus : IStationConfig
    {
        public string StationName { get; set; } = "";
        public StationEnum StationType { get; set; }
        public int StationNo { get; set; }
        public bool HasStatus { get; set; }
        public bool HasParameter { get; set; }
        public bool HasRecipe { get; set; }
        public string ModuleName { get; set; } = "";
        public StatusClass BatchID { get; set; } = new StatusClass();
        public StatusClass FlowRecipeName { get; set; } = new StatusClass();
        public StatusClass FlowRecipeStep { get; set; } = new StatusClass();
        public StatusClass UnitRecipeName { get; set; } = new StatusClass();
        public StatusClass UnitRecipeAllTime { get; set; } = new StatusClass();
        public StatusClass UnitRecipeResidueTime { get; set; } = new StatusClass();
        public StatusClass UnitRecipeOverTime { get; set; } = new StatusClass();
        public StatusClass UnitRecipeStep { get; set; } = new StatusClass();
        public StatusClass UnitRecipeStepTime { get; set; } = new StatusClass();
        public StatusClass UnitRecipeTime { get; set; } = new StatusClass();
        public StatusClass IsWafer { get; set; } = new StatusClass();
        public StatusClass DataID { get; set; } = new StatusClass();
        public StatusClass LidLeftSensorStatus { get; set; } = new StatusClass();
        public StatusClass LidRightSensorStatus { get; set; } = new StatusClass();
        public StatusClass InError { get; set; } = new StatusClass();

    }
}
