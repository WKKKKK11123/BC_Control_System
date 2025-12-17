using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//作为socket 消息传递的所有接口
namespace BC_Control_System.SubEvent
{
    public class SocketDataReceivedEvent : PubSubEvent<string> { }
    public class SocketResponseEvent : PubSubEvent<string> { }
}
