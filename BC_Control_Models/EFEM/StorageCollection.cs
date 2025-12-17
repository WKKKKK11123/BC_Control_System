using PropertyChanged;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    [AddINotifyPropertyChangedInterface]
    [SugarTable("ActualData_BufferInfo")]
    public class StorageCollection
    {
        public string CarrierID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int CarrierState { get; set; } //1  运行中  2：
        public int ProcessState{ get; set; }
        public string ErrorState { get; set; }
        public int Wafers { get; set; }
        public string WaferMap { get; set; }
        public int Location { get; set; }
        public int LoadingPort { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public Guid FoupGuid { get; set; }

    }
}
