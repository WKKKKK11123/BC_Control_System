using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    [AddINotifyPropertyChangedInterface]
    public class DoubleSensor
    {
        public bool Enable { get; set; } = true;
        public StatusClass Sensor1 { get; set; }=new StatusClass();
        public StatusClass Sensor2 { get; set; } = new StatusClass();
    }
}
