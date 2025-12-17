using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BC_Control_Models
{
    [XmlRoot("ManualControls")]
    public class ManualControls
    {      
        public ManualControls() 
        {
            Address = "D10000";
            PLCSelect = "PLC3";
            Name = "SC11";
            Text = "Fill";

        }
        //Manual规则默认为bool，有其他情况的界面单独做

        [XmlAttribute]
        public string Address { get; set; }
        [XmlAttribute]
        public string PLCSelect { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Text { get; set; }

    }
}
