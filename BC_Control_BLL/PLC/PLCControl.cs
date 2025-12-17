using BC_Control_Models;
using ZCCommunication;

namespace BC_Control_Helper.interfaceHelper
{
    public class PLCControl
    {
        private readonly OperateResult TrueoperateResult;
        private readonly OperateResult DataTypeErrorResult;

        private IPLCHelper _plcHelper;

        public PLCControl(IPLCHelper plcHelper)
        {
            _plcHelper = plcHelper;
            TrueoperateResult = new OperateResult()
            {
                IsSuccess = true,
                Message = "Sucessful"
            };
            DataTypeErrorResult = new OperateResult()
            {
                IsSuccess = false,
                Message = "Error : 未找到变量值"
            };
        }

        public OperateResult SetBoolValue(string Address, PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                if (!_plcHelper.ConnectAll())
                {
                    return new OperateResult()
                    {
                        IsSuccess = false,
                        Message = "PLC未连接成功"
                    };
                }
                if (_plcHelper.FindVariable(Address, plcEnum).DataType == "Bool")
                {
                    _plcHelper.CommonWrite(Address, "True", plcEnum);
                    return TrueoperateResult;
                }
                else
                {
                    return _plcHelper.SelectPLC(plcEnum).Write(Address, true);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public OperateResult ResetBoolValue(string Address, PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                if (!_plcHelper.ConnectAll())
                {
                    return new OperateResult()
                    {
                        IsSuccess = false,
                        Message = "PLC未连接成功"
                    };
                }
                if (_plcHelper.FindVariable(Address, plcEnum).DataType == "Bool")
                {
                    _plcHelper.CommonWrite(Address, false.ToString(), plcEnum);
                    return TrueoperateResult;
                }
                else
                {
                    return _plcHelper.SelectPLC(plcEnum).Write(Address, false);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public OperateResult InvetralVarible(string Address, PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                bool b;
                if (_plcHelper.FindVariable(Address, plcEnum).DataType == "Bool")
                {
                    b = (bool)_plcHelper.FindVariable(Address, plcEnum).VarValue;
                    _plcHelper.CommonWrite(Address, (!b).ToString(), plcEnum);
                    return TrueoperateResult;
                }
                b = _plcHelper.SelectPLC(plcEnum).ReadBool(Address).Content;
                return _plcHelper.SelectPLC(plcEnum).Write(Address, !b);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public OperateResult WriteValue(string Address, PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                bool b;
                if (_plcHelper.FindVariable(Address, plcEnum).DataType == "Bool")
                {
                    b = (bool)_plcHelper.FindVariable(Address, plcEnum).VarValue;
                    _plcHelper.CommonWrite(Address, (!b).ToString(), plcEnum);
                    return TrueoperateResult;
                }
                b = _plcHelper.SelectPLC(plcEnum).ReadBool(Address).Content;
                return _plcHelper.SelectPLC(plcEnum).Write(Address, !b);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
