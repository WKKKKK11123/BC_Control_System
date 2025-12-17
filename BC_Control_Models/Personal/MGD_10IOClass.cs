using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class MGD_10IOClass
    {
        public DataClass AV_MW_1 { get; set; }
        public DataClass AV_MW_2 { get; set; }
        public DataClass AV_MW_3 { get; set; }
        public DataClass AV_ID_4 { get; set; }
        public DataClass AV_ID_5 { get; set; }
        public DataClass AV_ID_6 { get; set; }     
        public DataClass AV_MN_2 { get; set; }
        public DataClass AV_MN_3 { get; set; }
        public DataClass AV_MN_4 { get; set; }
    }
}
