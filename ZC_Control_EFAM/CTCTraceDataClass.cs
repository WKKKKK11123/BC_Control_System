using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_EFAM
{
    public class CTCTraceDataClass
    {
        public int ID { get; set; } //从主站中获取的关联编号
        public string BatchID { get; set; }//批次编号
        public string RecipeName { get; set; }//配方名称
        public string CarrierIDOdd { get; set; }
        public string CarrierIDEven { get; set; }
        public int CarrierSrorageOdd { get; set; }
        public int CarrierSrorageEven { get; set; }
    }
}
