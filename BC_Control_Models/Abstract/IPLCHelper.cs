using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using ZCCommunication.Core.Address;
using thinger.DataConvertLib;

namespace BC_Control_Models
{
    public interface IPLCHelper
    {
        event Action<bool, Variable, PlcEnum> Device_AlarmTrigEvent;
        event Action<Variable> Device_OperatorTrigEvent;
        event Action<bool, Variable, PlcEnum> Device_TrigEvent;
        void LoadInfo();
        bool ConnectAll();
        string GetValue(IPLCValue t);
        bool ConnectState(PlcEnum plcEnum = PlcEnum.PLC1);
        Device SelectDevice(PlcEnum plcEnum = PlcEnum.PLC1);
        bool CommonWrite(string ValueAddress, string Value, PlcEnum plcEnum = PlcEnum.PLC1);
        MitsubishiPLC SelectPLC(PlcEnum plcEnum = PlcEnum.PLC1);
        Variable FindVariable(string varName, PlcEnum plcEnum = PlcEnum.PLC1);
        void SetVariableType(string valueAddress, DataType dataType, PlcEnum plcEnum);
        void SetVariableType(string valueAddress, IPLCValue plcValue);
    }
}
