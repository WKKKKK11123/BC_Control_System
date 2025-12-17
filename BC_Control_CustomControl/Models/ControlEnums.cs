using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_CustomControl
{
    class ControlEnums
    {
    }
    public enum WaferStatus
    {
        Empty = 0,      // 无晶圆
        Processing = 1,  // 处理中(黄色)
        Ready = 2,       // 准备就绪(青色)
        Completed = 3,   // 完成(绿色)
        Error = 4        // 错误状态(红色)
    }
}
