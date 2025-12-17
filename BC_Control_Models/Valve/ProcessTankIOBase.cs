using NPOI.SS.Formula.Functions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Valve
{
    public class ProcessTankIOBase
    {
        [Description("内槽排放阀")]
        public virtual DataClass? InnertankDrain { get; set; } = new DataClass();
        [Description("外槽排放阀")]
        public virtual DataClass? OuterTankDrain { get; set; } = new DataClass();
        [Description("循环阀")]
        public virtual DataClass? Circulation { get; set; } = new DataClass();
        [Description("过滤器排放阀")]
        public virtual DataClass? FilterDrain { get; set; } = new DataClass();
        [Description("高浓度排放阀")]
        public virtual DataClass? HighConcentrationDrain { get; set; } = new DataClass();
        [Description("低浓度排放阀")]
        public virtual DataClass? LowConcentrationDrain { get; set; } = new DataClass();
        [Description("取样阀")]
        public virtual DataClass? Sampling { get; set; } = new DataClass();
        [Description("药液1注液阀")]
        public virtual DataClass? Cheminal1Fill { get; set; } = new DataClass();
        [Description("药液2注液阀")]
        public virtual DataClass? Cheminal2Fill { get; set; } = new DataClass();
        [Description("DIW注液阀")]
        public virtual DataClass? Cheminal3Fill { get; set; } = new DataClass();
        [Description("DIW注液阀(外槽)")]
        public virtual DataClass? DIWFillOuterTank { get; set; } = new DataClass();
        [Description("混液进内槽")]
        public virtual DataClass? MixFill { get; set; } = new DataClass();
        [Description("循环泵")]
        public virtual DataClass? CyclePump { get; set; } = new DataClass();
        [Description("加热器")]
        public virtual DataClass? Heater { get; set; } = new DataClass();
        [Description("内槽液位")]
        public virtual DataClass? InnerTankLevel { get; set; } = new DataClass();
        [Description("外槽液位")]
        public virtual DataClass? OuterTankLevel { get; set; } = new DataClass();
    }
}
