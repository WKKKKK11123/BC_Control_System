using BC_Control_Models;
using ZCCommunication;

namespace BC_Control_BLL.Abstract
{
    public interface IPLCControl
    {
        OperateResult InvetralVarible(string Address, PlcEnum plcEnum = PlcEnum.PLC1);
        OperateResult ResetBoolValue(string Address, PlcEnum plcEnum = PlcEnum.PLC1);
        OperateResult SetBoolValue(string Address, PlcEnum plcEnum = PlcEnum.PLC1);
        OperateResult WriteValue(string Address, PlcEnum plcEnum = PlcEnum.PLC1);
    }
}