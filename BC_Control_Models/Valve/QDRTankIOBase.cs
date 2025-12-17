using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Valve
{
    public class QDRTankIOBase
    {
        [Description("上喷淋阀")]
        public virtual DataClass UpperSpray { get; set; } = new DataClass();
        [Description("快进水阀")]
        public virtual DataClass QuickFill { get; set; } = new DataClass();
        [Description("慢进水阀")]
        public virtual DataClass SlowFill { get; set; } = new DataClass();
        [Description("总进水阀")]
        public virtual DataClass MainFill { get; set; } = new DataClass();
        [Description("水阻检测阀")]
        public virtual DataClass Resistance { get; set; } = new DataClass();
        [Description("快排阀")]
        public virtual DataClass QuickDrain { get; set; } = new DataClass();
        [Description("高浓度排")]
        public virtual DataClass HighConcentrationDrain { get; set; } = new DataClass();
        [Description("低浓度排")]
        public virtual DataClass LowConcentrationDrain { get; set; } = new DataClass();
        public virtual DataClass TankLevel { get; set; } = new DataClass();
    }
}
