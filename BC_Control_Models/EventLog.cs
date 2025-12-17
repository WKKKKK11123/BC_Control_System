using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [SugarTable("EventLog")]
    public class EventLog
    {
        public DateTime InsertTime { get; set; }
        public string Comment { get; set; }
        public string VarName { get; set; }
        public string GroupName { get; set; }
        public string Priority { get; set; }
        public string Controller { get; set; }
    }
}
