using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class NMP_3IOClass
    {
        public DataClass AV_NMP_1 { get; set; }
        public DataClass AV_NMP_2 { get; set; }
        public DataClass AV_NMP_3 { get; set; }
        public DataClass AV_NMP_4 { get; set; }
        public DataClass AV_NMP_5 { get; set; }
        public DataClass AV_NMP_6 { get; set; }
        public DataClass AV_NMP_7 { get; set; }
        public DataClass AV_NMP_8 { get; set; }
        public DataClass AV_NMP_10 { get; set; }
        public DataClass AV_DI_3 { get; set; }
        public DataClass AV_CW_5 { get; set; }
        public DataClass AV_IP_5 { get; set; }
        public DataClass AV_IP_6 { get; set; }
        public DataClass HeaterState { get; set; }
        public DataClass AV_NMP_25 { get; set; }
        public DataClass PumpState { get; set; }
    }
}
