using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCCommunication.Profinet.Melsec;
namespace BC_Control_Models
{
    public class MitsubishiPLC : MelsecMcNet
    {
        public MitsubishiPLC():base()
        {
        }

        public MitsubishiPLC(string ip, int port) : base(ip, port)
        {
        }
    }
}
