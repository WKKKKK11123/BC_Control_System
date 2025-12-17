using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_System.Event
{
    public class AlarmUpdateEvent:PubSubEvent<List<AlarmLog>>
    {

    }
}
