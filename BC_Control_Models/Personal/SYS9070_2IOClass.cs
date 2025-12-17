using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class SYS9070_2IOClass
    {
        public DataClass AV_SY_11 { get; set; }
        public DataClass AV_SY_12 { get; set; }
        public DataClass AV_SY_13 { get; set; }
        public DataClass AV_SY_14 { get; set; }
        public DataClass AV_SY_15 { get; set; }
        public DataClass AV_SY_16 { get; set; }
        public DataClass AV_SY_17 { get; set; }
        public DataClass AV_SY_18 { get; set; }
        public DataClass AV_SY_20 { get; set; }
        public DataClass AV_DI_2 { get; set; }
        public DataClass AV_CW_3 { get; set; }
        public DataClass AV_IP_3 { get; set; }
        public DataClass AV_IP_4 { get; set; }
        public DataClass HeaterState { get; set; }
        public DataClass AV_SY_25 { get; set; }
        public DataClass PumpState { get; set; }
    }
}
