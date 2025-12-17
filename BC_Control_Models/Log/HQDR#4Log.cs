using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    [SugarTable("ActualData_HQDR_4")]
    public class HQDR_4Log
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;
        //Tank Temp
        public string TankTemp { get; set; } = "0";
        ////Concentration Temp
        //public string ConcentrationTemp { get; set; } = "0";
        //DIW Flow Rate
        public string DIWFlowRate { get; set; } = "0";
        //Hot DIW Flow Rate
        public string HotDIWFlowRate { get; set; } = "0";
        ////Shower Flow Rate
        //public string ShowerFlowRate { get; set; } = "0";
        //Resistivity
        public string Resistivity { get; set; } = "0";
    }
}
