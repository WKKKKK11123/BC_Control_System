using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class QDR_7IOClass
    {
        public DataClass AV_DI_9 { get; set; }
        public DataClass AV_DI_10 { get; set; }
        public DataClass AV_DI_11 { get; set; }
        public DataClass AV_C_1 { get; set; }
        public DataClass AV_DR_2 { get; set; }
        public DataClass AV_DR_3 { get; set; }
    }
}
