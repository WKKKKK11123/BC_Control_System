using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Valve
{
    [AddINotifyPropertyChangedInterface]
    public class CHCLTankIOBase
    {
        [Description("上喷淋阀")]
        public virtual DataClass UpperSpray { get; set; } = new DataClass();
        [Description("DIW下给水")]
        public virtual DataClass LowerFill { get; set; } = new DataClass();
        [Description("上喷淋总进水")]
        public virtual DataClass UpperSprayInner { get; set; } = new DataClass();
        [Description("槽体排放阀")]
        public virtual DataClass Drain { get; set; } = new DataClass();
        [Description("N2进(上风刀）")]
        public virtual DataClass UpperN2 { get; set; } = new DataClass();
        [Description("N2进(下风刀）")]
        public virtual DataClass LowerN2 { get; set; } = new DataClass();
        public virtual DataClass TankLevel { get; set; } = new DataClass();

    }
}
