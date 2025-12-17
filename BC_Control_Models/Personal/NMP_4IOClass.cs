using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class NMP_4IOClass
    {
        public DataClass AV_NMP_11 { get; set; }
        public DataClass AV_NMP_12 { get; set; }
        public DataClass AV_NMP_13 { get; set; }
        public DataClass AV_NMP_14 { get; set; }
        public DataClass AV_NMP_15 { get; set; }
        public DataClass AV_NMP_16 { get; set; }
        public DataClass AV_NMP_17 { get; set; }
        public DataClass AV_NMP_18 { get; set; }
        public DataClass AV_NMP_20 { get; set; }
        public DataClass AV_CW_7 { get; set; }
        public DataClass AV_DI_4 { get; set; }
        public DataClass AV_IP_7 { get; set; }
        public DataClass AV_IP_8 { get; set; }
        public DataClass HeaterState { get; set; }
        public DataClass AV_NMP_25 { get; set; }
        public DataClass PumpState { get; set; }
    }
}
