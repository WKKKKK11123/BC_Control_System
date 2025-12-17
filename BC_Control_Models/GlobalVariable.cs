using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class GlobalVariable
    {
        public List<AlarmLog> actualAlarmList { get; set; } = new List<AlarmLog>();
        public TKClass tk9lpd { get; set; }

    }
}
