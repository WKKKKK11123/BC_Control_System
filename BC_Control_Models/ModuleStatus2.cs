using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class ModuleStatus2
    {
        public string ModuleName2 { get; set; }
        public StatusClass Current { get; set; }
        public StatusClass SettingUpLime { get; set; }
        public StatusClass SettingDownLime { get; set; }
        public StatusClass SettingDelayTime { get; set; }


    }
}
