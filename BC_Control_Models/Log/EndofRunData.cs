
using MiniExcelLibs.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.ClassK.SQLService
{
    [SugarTable("ActualData_EndofRun")]
    public class EndofRunData 
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [ExcelIgnore]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; } // ✅ 可为空

        public string ModuleRecipe { get; set; }


        public string Status { get; set; }

        [ExcelIgnore]
        public int StationID { get; set; }

        public int ProcessStep { get; set; }

        public int WaferID { get; set; }

        public int MessageID { get; set; }

        public string BatchID { get; set; }
        public string FlowRecipeName { get; set; }

        public string ReturnWaferCount { get; set; }



    }
}
