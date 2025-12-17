using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Models.Valve;

namespace BC_Control_System.model
{
    [AddINotifyPropertyChangedInterface]
    public class Ag_1IOModel:ProcessTankIOBase
    {
        [Description("H2SO4进外槽")]
        public override DataClass Cheminal1Fill { get ; set ; }
        [Description("HNO3进外槽")]
        public override DataClass Cheminal2Fill { get; set; }
        [Description("DIW进外槽")]
        public override DataClass Cheminal3Fill { get; set; }

    }
}
