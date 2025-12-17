using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BC_Control_Models.LogVo
{
    public class BaseAddress
    {
        [XmlIgnore]
        public string Value { get; set; } = "0";
        [XmlAttribute]
        public string ValueAddress { get; set; } = "D10000";
        [XmlAttribute]
        public string DataType { get; set; }="16";
        [XmlAttribute]
        public string Scale { get; set; } = "1";
        [XmlAttribute]
        public string PLC { get; set; } = "PLC1";
    }
}
