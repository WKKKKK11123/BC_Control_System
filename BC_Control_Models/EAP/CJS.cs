using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.eap;

namespace BC_Control_Models.EAP
{
    public class CJS
    {
        public string objid { get; set; }
        public string[] carrierinputspec { get; set; }
        public Mtrloutspec mtrloutspec { get; set; }
        public List<ProcessingctrlspecClass> processingctrlspec { get; set; }
        public string startmethod { get; set; }
    }
}
