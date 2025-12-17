using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Valve
{
    public class MixTankIOBase
    {
        [Description("药液1注液阀")]
        public virtual DataClass Cheminal1Fill { get; set; } = new DataClass();
        [Description("药液2注液阀")]
        public virtual DataClass Cheminal2Fill { get; set; } = new DataClass();
        [Description("DIW注液阀")]
        public virtual DataClass DIWFill { get; set; } = new DataClass();
        [Description("循环阀")]
        public virtual DataClass Circulation { get; set; } = new DataClass();
        [Description("高浓度排放阀")]
        public virtual DataClass HighConcentrationDrain { get; set; } = new DataClass();
        [Description("低浓度排放阀")]
        public virtual DataClass LowConcentrationDrain { get; set; } = new DataClass();
        [Description("循环泵")]
        public virtual DataClass CyclePump { get; set; } = new DataClass();
        [Description("槽体液位")]
        public virtual DataClass TankLevel { get; set; } = new DataClass();
        [Description("PCWIN")]
        public virtual DataClass PCWIN { get; set; } = new DataClass();
        [Description("PCWOut")]
        public virtual DataClass PCWOut { get; set; } = new DataClass();
    }
}
