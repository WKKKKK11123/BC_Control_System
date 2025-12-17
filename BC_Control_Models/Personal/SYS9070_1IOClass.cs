using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class SYS9070_1IOClass
    {
        public DataClass AV_SY_1 { get; set; }
        public DataClass AV_SY_2 { get; set; }
        public DataClass AV_SY_3 { get; set; }
        public DataClass AV_SY_4 { get; set; }
        public DataClass AV_SY_5 { get; set; }
        public DataClass AV_SY_6 { get; set; }
        public DataClass AV_SY_7 { get; set; }
        public DataClass AV_SY_8 { get; set; }
        public DataClass AV_SY_10 { get; set; }
        public DataClass AV_DI_1 { get; set; }
        public DataClass AV_CW_1 { get; set; }
        public DataClass AV_IP_1 { get; set; }
        public DataClass AV_IP_2 { get; set; }
        public DataClass AV_SY_25 { get; set; }
        public DataClass PumpState { get; set; }

        public DataClass HeaterState { get; set; }
    }
}
