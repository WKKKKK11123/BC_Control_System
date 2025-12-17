using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.DataConvertLib;
using BC_Control_Models;

namespace BC_Control_Helper
{
    public class PLCValue : IPLCValue
    {
        public string ParameterName { get; set; }
        public PlcEnum PLC { get; set; }
        public string ValueAddress { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
        public float Scale { get; set; }
        public int OffsetOrLength { get; set; }
    }
}
