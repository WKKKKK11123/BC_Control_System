using MiniExcelLibs;
using System.Data;
using System.Diagnostics;
using System.Text;
using thinger.DataConvertLib;
using BC_Control_Models;
using BC_Control_Helper;
using ZCCommunication.Core.Address;
using Microsoft.IdentityModel.Tokens;
using Dm;
using Dm.util;
using BC_Control_BLL.Services;

namespace BC_Control_Helper
{
    public class PLCSelect : IPLCHelper
    {
        #region
        private CancellationTokenSource _cts;
        private string devPath =
           "C:" + "\\PLCConfig" + "\\Config\\Device.ini";
        private string groupPath =
            "C:" + "\\PLCConfig" + "\\Config\\Group.xlsx";
        private string variablePath =
           "C:" + "\\PLCConfig" + "\\Config\\Variable.xlsx";
        private string devPath2 =
            "C:" + "\\PLCConfig" + "\\PLC2Config\\Device.ini";
        private string groupPath2 =
            "C:" + "\\PLCConfig" + "\\PLC2Config\\Group.xlsx";
        private string variablePath2 =
            "C:" + "\\PLCConfig" + "\\PLC2Config\\Variable.xlsx";
        private string devPath3 =
            "C:" + "\\PLCConfig" + "\\PLC3Config\\Device.ini";
        private string groupPath3 =
            "C:" + "\\PLCConfig" + "\\PLC3Config\\Group.xlsx";
        private string variablePath3 =
            "C:" + "\\PLCConfig" + "\\PLC3Config\\Variable.xlsx";
        private string devPath4 =
            "C:" + "\\PLCConfig" + "\\PLC4Config\\Device.ini";
        private string groupPath4 =
            "C:" + "\\PLCConfig" + "\\PLC4Config\\Group.xlsx";
        private string variablePath4 =
            "C:" + "\\PLCConfig" + "\\PLC4Config\\Variable.xlsx";
        private string devPath5 =
            "C:" + "\\PLCConfig" + "\\PLC5Config\\Device.ini";
        private string groupPath5 =
            "C:" + "\\PLCConfig" + "\\PLC5Config\\Group.xlsx";
        private string variablePath5 =
            "C:" + "\\PLCConfig" + "\\PLC5Config\\Variable.xlsx";
        private string devPath6 =
            "C:" + "\\PLCConfig" + "\\PLC6Config\\Device.ini";
        private string groupPath6 =
            "C:" + "\\PLCConfig" + "\\PLC6Config\\Group.xlsx";
        private string variablePath6 =
            "C:" + "\\PLCConfig" + "\\PLC6Config\\Variable.xlsx";

        #endregion
        #region
        public event Action<bool, Variable, PlcEnum> Device_AlarmTrigEvent;
        public event Action<Variable> Device_OperatorTrigEvent;
        public event Action<bool, Variable, PlcEnum> Device_TrigEvent;
        #endregion
        private static PLCSelect _instance;
        public static PLCSelect Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PLCSelect();
                return _instance;
            }
        }
        public PLCSelect()
        {
            _cts = new CancellationTokenSource();
        }
        public Device SelectDevice(PlcEnum plcEnum = PlcEnum.PLC1)
        {
            Device device;
            switch (plcEnum)
            {
                case PlcEnum.PLC2:
                    device = PLC2CommonMethods.Device;
                    break;
                case PlcEnum.PLC3:
                    device = PLC3CommonMethods.Device;
                    break;
                case PlcEnum.PLC4:
                    device = PLC4CommonMethods.Device;
                    break;
                case PlcEnum.PLC5:
                    device = PLC5CommonMethods.Device;
                    break;
                case PlcEnum.PLC6:
                    device = PLC6CommonMethods.Device;
                    break;
                case PlcEnum.PLC7:
                    device = PLC7CommonMethods.Device;
                    break;
                case PlcEnum.PLC8:
                    device = PLC8CommonMethods.Device;
                    break;
                case PlcEnum.PLC9:
                    device = PLC9CommonMethods.Device;
                    break;
                case PlcEnum.PLC10:
                    device = PLC10CommonMethods.Device;
                    break;
                case PlcEnum.PLC11:
                    device = PLC11CommonMethods.Device;
                    break;
                case PlcEnum.PLC12:
                    device = PLC12CommonMethods.Device;
                    break;
                case PlcEnum.PLC13:
                    device = PLC13CommonMethods.Device;
                    break;
                default:
                    device = CommonMethods.Device;
                    break;
            }
            return device;
        }
        public bool CommonWrite(IPLCValue t, string Value)
        {
            bool result = false;
            try
            {
                if (SelectDevice(t.PLC).IsConnected == false) return result;
                switch (t.PLC)
                {
                    case PlcEnum.PLC2:
                        result = PLC2CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC3:
                        result = PLC3CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC4:
                        result = PLC4CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC5:
                        result = PLC5CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC6:
                        result = PLC6CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC7:
                        result = PLC7CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC8:
                        result = PLC8CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC9:
                        result = PLC9CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC10:
                        result = PLC10CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC11:
                        result = PLC11CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC12:
                        result = PLC12CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    case PlcEnum.PLC13:
                        result = PLC13CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                    default:
                        result = CommonMethods.CommonWrite(t.ValueAddress, Value);
                        break;
                }
                return result;

            }
            catch (Exception)
            {

                return result;
            }
        }
        public bool CommonWrite(string ValueAddress, string Value, PlcEnum plc)
        {
            bool result = false;
            if (SelectDevice(plc).IsConnected == false) return result;
            try
            {
                switch (plc)
                {
                    case PlcEnum.PLC2:
                        result = PLC2CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC3:
                        result = PLC3CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC4:
                        result = PLC4CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC5:
                        result = PLC5CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC6:
                        result = PLC6CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC7:
                        result = PLC7CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC8:
                        result = PLC8CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC9:
                        result = PLC9CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC10:
                        result = PLC10CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC11:
                        result = PLC11CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC12:
                        result = PLC12CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    case PlcEnum.PLC13:
                        result = PLC13CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                    default:
                        result = CommonMethods.CommonWrite(ValueAddress, Value);
                        break;
                }
                return result;
            }
            catch (Exception)
            {

                return result;
            }
        }
        public MitsubishiPLC SelectPLC(PlcEnum plcEnum = PlcEnum.PLC1)
        {
            MitsubishiPLC device;
            switch (plcEnum)
            {
                case PlcEnum.PLC2:
                    device = PLC2CommonMethods.PLC;
                    break;
                case PlcEnum.PLC3:
                    device = PLC3CommonMethods.PLC;
                    break;
                case PlcEnum.PLC4:
                    device = PLC4CommonMethods.PLC;
                    break;
                case PlcEnum.PLC5:
                    device = PLC5CommonMethods.PLC;
                    break;
                case PlcEnum.PLC6:
                    device = PLC6CommonMethods.PLC;
                    break;
                case PlcEnum.PLC7:
                    device = PLC7CommonMethods.PLC;
                    break;
                case PlcEnum.PLC8:
                    device = PLC8CommonMethods.PLC;
                    break;
                case PlcEnum.PLC9:
                    device = PLC9CommonMethods.PLC;
                    break;
                case PlcEnum.PLC10:
                    device = PLC10CommonMethods.PLC;
                    break;
                case PlcEnum.PLC11:
                    device = PLC11CommonMethods.PLC;
                    break;
                case PlcEnum.PLC12:
                    device = PLC12CommonMethods.PLC;
                    break;
                case PlcEnum.PLC13:
                    device = PLC13CommonMethods.PLC;
                    break;
                default:
                    device = CommonMethods.PLC;
                    break;
            }
            return device;
        }
        public Variable FindVariable(string varName, PlcEnum plcEnum = PlcEnum.PLC1)
        {

            try
            {
                Variable variable;
                switch (plcEnum)
                {
                    case PlcEnum.PLC2:
                        return PLC2CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC3:
                        return PLC3CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC4:
                        return PLC4CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC5:
                        return PLC5CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC6:
                        return PLC6CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC7:
                        return PLC7CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC8:
                        return PLC8CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC9:
                        return PLC9CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC10:
                        return PLC10CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC11:
                        return PLC11CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC12:
                        return PLC12CommonMethods.FindVariable(varName);
                    case PlcEnum.PLC13:
                        return PLC13CommonMethods.FindVariable(varName);
                    default:
                        return CommonMethods.FindVariable(varName);
                }
            }
            catch (Exception ex)
            {

                return new Variable();
            }



        }
        public string GetValue(IPLCValue t)
        {
            try
            {
                if (SelectDevice(t.PLC).IsConnected == false) return GetDefualtValue(t.DataType).ToString();
                if (string.IsNullOrEmpty(t.ValueAddress)) return GetDefualtValue(t.DataType).ToString();
                Device device = SelectDevice(t.PLC);
                if (t.ValueAddress == null)
                {
                    return GetDefualtValue(t.DataType).ToString();
                }
                return device[t.ValueAddress]?.ToString();
            }
            catch (Exception ee)
            {
                return GetDefualtValue(t.DataType).ToString();

            }
        }
        public bool ConnectState(PlcEnum plcEnum = PlcEnum.PLC1)
        {
            try
            {
                return SelectDevice(plcEnum).IsConnected;
            }
            catch (Exception EX)
            {

                return false;
            }

        }
        public bool ConnectAll()
        {
            try
            {
                bool result = true;
                result &= CommonMethods.Device.IsConnected;
                result &= PLC2CommonMethods.Device.IsConnected;
                result &= PLC3CommonMethods.Device.IsConnected;
                result &= PLC4CommonMethods.Device.IsConnected;
                result &= PLC5CommonMethods.Device.IsConnected;
                //result &= CommonMethods.Device.IsConnected;
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SetVariableType(string valueAddress, DataType dataType, PlcEnum plcEnum)
        {
            if (string.IsNullOrEmpty(valueAddress))
            {
                return;
            }
            Device device = SelectDevice(plcEnum);
            CheckVariableType(device.GroupList, valueAddress, dataType);
        }
        public void SetVariableType(string valueAddress, IPLCValue plcValue)
        {
            if (string.IsNullOrEmpty(plcValue.ValueAddress))
            {
                return;
            }
            Device device = SelectDevice(plcValue.PLC);
            CheckVariableType(device.GroupList, plcValue);
        }
        public void LoadInfo()
        {
            try
            {
                CommonMethods.Device = LoadDevice(GetFile(1, "Device.ini"), GetFile(1, "Group.xlsx"), GetFile(1, "Variable.xlsx"));
                if (CommonMethods.Device != null)
                {
                    CommonMethods.PLC = new MitsubishiPLC(
                        CommonMethods.Device.IPAddress,
                        CommonMethods.Device.Port
                    );
                    CommonMethods.PLC.SetPersistentConnection();
                    CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                CommonMethods.Device,
                                CommonMethods.PLC,
                                PlcEnum.PLC1
                            );
                        }), _cts.Token
                    );
                }
                PLC2CommonMethods.Device = LoadDevice(GetFile(2, "Device.ini"), GetFile(2, "Group.xlsx"), GetFile(2, "Variable.xlsx"));
                if (PLC2CommonMethods.Device != null)
                {

                    PLC2CommonMethods.PLC = new MitsubishiPLC(
                        PLC2CommonMethods.Device.IPAddress,
                        PLC2CommonMethods.Device.Port
                    );
                    PLC2CommonMethods.PLC.SetPersistentConnection();
                    PLC2CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC2CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC2CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC2CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC2CommonMethods.Device,
                                PLC2CommonMethods.PLC,
                                PlcEnum.PLC2
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC3CommonMethods.Device = LoadDevice(GetFile(3, "Device.ini"), GetFile(3, "Group.xlsx"), GetFile(3, "Variable.xlsx"));
                if (PLC3CommonMethods.Device != null)
                {

                    PLC3CommonMethods.PLC = new MitsubishiPLC(
                        PLC3CommonMethods.Device.IPAddress,
                        PLC3CommonMethods.Device.Port
                    );
                    PLC3CommonMethods.PLC.SetPersistentConnection();
                    PLC3CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC3CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC3CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC3CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC3CommonMethods.Device,
                                PLC3CommonMethods.PLC,
                                PlcEnum.PLC3
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC4CommonMethods.Device = LoadDevice(GetFile(4, "Device.ini"), GetFile(4, "Group.xlsx"), GetFile(4, "Variable.xlsx"));
                if (PLC4CommonMethods.Device != null)
                {

                    PLC4CommonMethods.PLC = new MitsubishiPLC(
                        PLC4CommonMethods.Device.IPAddress,
                        PLC4CommonMethods.Device.Port
                    );
                    PLC4CommonMethods.PLC.SetPersistentConnection();
                    PLC4CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC4CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC4CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC4CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC4CommonMethods.Device,
                                PLC4CommonMethods.PLC,
                                PlcEnum.PLC4
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC5CommonMethods.Device = LoadDevice(GetFile(5, "Device.ini"), GetFile(5, "Group.xlsx"), GetFile(5, "Variable.xlsx"));
                if (PLC5CommonMethods.Device != null)
                {

                    PLC5CommonMethods.PLC = new MitsubishiPLC(
                        PLC5CommonMethods.Device.IPAddress,
                        PLC5CommonMethods.Device.Port
                    );

                    PLC5CommonMethods.PLC.SetPersistentConnection();
                    PLC5CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC5CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC5CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC5CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC5CommonMethods.Device,
                                PLC5CommonMethods.PLC,
                                PlcEnum.PLC5
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC6CommonMethods.Device = LoadDevice(GetFile(6, "Device.ini"), GetFile(6, "Group.xlsx"), GetFile(6, "Variable.xlsx"));
                if (PLC6CommonMethods.Device != null)
                {
                    //PLC6CommonMethods.Device.IPAddress = "192.168.1.15";
                    //PLC6CommonMethods.Device.Port = 12115;
                    PLC6CommonMethods.PLC = new MitsubishiPLC(
                        PLC6CommonMethods.Device.IPAddress,
                        PLC6CommonMethods.Device.Port
                    );

                    PLC6CommonMethods.PLC.SetPersistentConnection();
                    //PLC6CommonMethods.Device.processLogManage = processLogManage;
                    //PLC6CommonMethods.Device.GetBatchInfoFunc = GetBatchInfo; // 传入委托用于获取批次信息
                    PLC6CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC6CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC6CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC6CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    //PLC6CommonMethods.Device.ProcessTrigEvent += Device_ProcessTrigEvent;
                    //PLC6CommonMethods.Device.TankProcessTrigEvent += Device_TankProcessTrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC6CommonMethods.Device,
                                PLC6CommonMethods.PLC,
                                PlcEnum.PLC6
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC7CommonMethods.Device = LoadDevice(GetFile(7, "Device.ini"), GetFile(7, "Group.xlsx"), GetFile(7, "Variable.xlsx"));
                if (PLC7CommonMethods.Device != null)
                {
                    //PLC7CommonMethods.Device.IPAddress = "192.178.1.15";
                    //PLC7CommonMethods.Device.Port = 12115;
                    PLC7CommonMethods.PLC = new MitsubishiPLC(
                        PLC7CommonMethods.Device.IPAddress,
                        PLC7CommonMethods.Device.Port
                    );

                    PLC7CommonMethods.PLC.SetPersistentConnection();
                    //PLC7CommonMethods.Device.processLogManage = processLogManage;
                    //PLC7CommonMethods.Device.GetBatchInfoFunc = GetBatchInfo; // 传入委托用于获取批次信息
                    PLC7CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC7CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC7CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC7CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    //PLC7CommonMethods.Device.ProcessTrigEvent += Device_ProcessTrigEvent;
                    //PLC7CommonMethods.Device.TankProcessTrigEvent += Device_TankProcessTrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC7CommonMethods.Device,
                                PLC7CommonMethods.PLC,
                                PlcEnum.PLC7
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC8CommonMethods.Device = LoadDevice(GetFile(8, "Device.ini"), GetFile(8, "Group.xlsx"), GetFile(8, "Variable.xlsx"));
                if (PLC8CommonMethods.Device != null)
                {
                    PLC8CommonMethods.PLC = new MitsubishiPLC(
                        PLC8CommonMethods.Device.IPAddress,
                        PLC8CommonMethods.Device.Port
                    );

                    PLC8CommonMethods.PLC.SetPersistentConnection();
                    PLC8CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC8CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC8CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC8CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC8CommonMethods.Device,
                                PLC8CommonMethods.PLC,
                                PlcEnum.PLC8
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC9CommonMethods.Device = LoadDevice(GetFile(9, "Device.ini"), GetFile(9, "Group.xlsx"), GetFile(9, "Variable.xlsx"));
                if (PLC9CommonMethods.Device != null)
                {
                    PLC9CommonMethods.PLC = new MitsubishiPLC(
                        PLC9CommonMethods.Device.IPAddress,
                        PLC9CommonMethods.Device.Port
                    );

                    PLC9CommonMethods.PLC.SetPersistentConnection();
                    PLC9CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC9CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC9CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC9CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC9CommonMethods.Device,
                                PLC9CommonMethods.PLC,
                                PlcEnum.PLC9
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC10CommonMethods.Device = LoadDevice(GetFile(10, "Device.ini"), GetFile(10, "Group.xlsx"), GetFile(10, "Variable.xlsx"));
                if (PLC10CommonMethods.Device != null)
                {
                    PLC10CommonMethods.PLC = new MitsubishiPLC(
                        PLC10CommonMethods.Device.IPAddress,
                        PLC10CommonMethods.Device.Port
                    );

                    PLC10CommonMethods.PLC.SetPersistentConnection();
                    PLC10CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC10CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC10CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC10CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC10CommonMethods.Device,
                                PLC10CommonMethods.PLC,
                                PlcEnum.PLC10
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC11CommonMethods.Device = LoadDevice(GetFile(11, "Device.ini"), GetFile(11, "Group.xlsx"), GetFile(11, "Variable.xlsx"));
                if (PLC11CommonMethods.Device != null)
                {
                    PLC11CommonMethods.PLC = new MitsubishiPLC(
                        PLC11CommonMethods.Device.IPAddress,
                        PLC11CommonMethods.Device.Port
                    );

                    PLC11CommonMethods.PLC.SetPersistentConnection();
                    PLC11CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC11CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC11CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC11CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC11CommonMethods.Device,
                                PLC11CommonMethods.PLC,
                                PlcEnum.PLC11
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC12CommonMethods.Device = LoadDevice(GetFile(12, "Device.ini"), GetFile(12, "Group.xlsx"), GetFile(12, "Variable.xlsx"));
                if (PLC12CommonMethods.Device != null)
                {
                    PLC12CommonMethods.PLC = new MitsubishiPLC(
                        PLC12CommonMethods.Device.IPAddress,
                        PLC12CommonMethods.Device.Port
                    );

                    PLC12CommonMethods.PLC.SetPersistentConnection();
                    PLC12CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC12CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC12CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC12CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC12CommonMethods.Device,
                                PLC12CommonMethods.PLC,
                                PlcEnum.PLC12
                            );
                        }),
                        _cts.Token
                    );
                }
                PLC13CommonMethods.Device = LoadDevice(GetFile(13, "Device.ini"), GetFile(13, "Group.xlsx"), GetFile(13, "Variable.xlsx"));
                if (PLC13CommonMethods.Device != null)
                {
                    //PLC13CommonMethods.Device.IPAddress = "192.1138.1.15";
                    //PLC13CommonMethods.Device.Port = 12115;
                    PLC13CommonMethods.PLC = new MitsubishiPLC(
                        PLC13CommonMethods.Device.IPAddress,
                        PLC13CommonMethods.Device.Port
                    );

                    PLC13CommonMethods.PLC.SetPersistentConnection();
                    PLC13CommonMethods.AddLog?.Invoke(0, "设备信息加载成功！");
                    PLC13CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;
                    PLC13CommonMethods.Device.OperatorTrigEvent += Device_OperatorTrigEvent;
                    PLC13CommonMethods.Device.CarrierTrigEvent += Device_TrigEvent;
                    Task task = Task.Run(
                        new Action(() =>
                        {
                            DeviceCommunication(
                                PLC13CommonMethods.Device,
                                PLC13CommonMethods.PLC,
                                PlcEnum.PLC13
                            );
                        }),
                        _cts.Token
                    );
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }

        /// <summary>
        /// 加载通信组信息
        /// </summary>
        /// <param name="groupPath"></param>
        /// <param name="variablePath"></param>
        /// <returns></returns>
        private List<Group> LoadGroup(string groupPath, string variablePath)
        {
            List<Group> groupList = null;
            try
            {
                groupList = MiniExcel.Query<Group>(groupPath).ToList();
            }
            catch (Exception ex)
            {
                CommonMethods.AddLog?.Invoke(1, "通信组加载失败" + ex.Message);
                return null;
            }

            List<Variable> varList = null;
            try
            {
                varList = MiniExcel.Query<Variable>(variablePath).ToList();
            }
            catch (Exception ex)
            {
                CommonMethods.AddLog?.Invoke(1, "通信变量加载失败" + ex.Message);
                return null;
            }

            if (groupList != null && varList != null)
            {
                foreach (var group in groupList)
                {
                    group.VarList = varList.FindAll(c => c.GroupName == group.GroupName).ToList();
                }
                return groupList;
            }
            else
            {
                return null;
            }
        }
        private void DeviceCommunication(Device device, MitsubishiPLC mitsubishiPLC, PlcEnum plcEnum)
        {
            while (!_cts.IsCancellationRequested)
            {
                if (device.IsConnected)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    // 通信读取
                    foreach (var gp in device.GroupList)
                    {
                        StoreArea storeArea = (StoreArea)
                            Enum.Parse(typeof(StoreArea), gp.StoreArea, true);
                        switch (storeArea)
                        {
                            case StoreArea.线圈:
                                var res = mitsubishiPLC.ReadBool(gp.StartAddress, gp.Length);
                                if (res.IsSuccess)
                                {
                                    // 变量解析

                                    foreach (var variable in gp.VarList)
                                    {
                                        DataType dataType = (DataType)
                                            Enum.Parse(typeof(DataType), variable.DataType, true);
                                        switch (dataType)
                                        {
                                            case DataType.Bool:
                                                //variable.VarValue = res.Content[variable.Start + variable.OffsetOrLength];
                                                variable.VarValue = res.Content[variable.Start];

                                                break;
                                            default:
                                                break;
                                        }

                                        // 处理
                                        device.UpdateVariable(variable, plcEnum);
                                    }
                                }
                                else
                                {
                                    device.IsConnected = false;
                                }
                                break;
                            case StoreArea.寄存器:
                                var result = mitsubishiPLC.Read(
                                    gp.StartAddress,
                                    (ushort)(gp.Length)
                                );
                                if (result.IsSuccess)
                                {
                                    foreach (var variable in gp.VarList)
                                    {
                                        DataType dataType = (DataType)
                                            Enum.Parse(typeof(DataType), variable.DataType, true);
                                        int start = variable.Start;
                                        //if (start> gp.Length)
                                        //{
                                        //    continue;
                                        //}
                                        start *= 2;
                                        thinger.DataConvertLib.DataFormat dataFormat =
                                            CommonMethods.dataFormat;

                                        switch (dataType)
                                        {
                                            case DataType.Bool:
                                                variable.VarValue = BitLib.GetBitFrom2BytesArray(
                                                    result.Content,
                                                    start + variable.OffsetOrLength / 16 * 2,
                                                    (variable.OffsetOrLength) % 16,
                                                    dataFormat
                                                        == thinger.DataConvertLib.DataFormat.BADC
                                                        || dataFormat
                                                            == thinger
                                                                .DataConvertLib
                                                                .DataFormat
                                                                .DCBA
                                                );
                                                break;
                                            case DataType.Byte:
                                                variable.VarValue = ByteLib.GetByteFromByteArray(
                                                    result.Content,
                                                    dataFormat
                                                        == thinger.DataConvertLib.DataFormat.BADC
                                                    || dataFormat
                                                        == thinger.DataConvertLib.DataFormat.DCBA
                                                        ? start
                                                        : start + 1
                                                );
                                                break;
                                            case DataType.Short:
                                                variable.VarValue = ShortLib.GetShortFromByteArray(
                                                    result.Content,
                                                    start,
                                                    dataFormat
                                                );
                                                break;
                                            case DataType.UShort:
                                                variable.VarValue =
                                                    UShortLib.GetUShortFromByteArray(
                                                        result.Content,
                                                        start,
                                                        dataFormat
                                                    );
                                                break;
                                            case DataType.Int:
                                                variable.VarValue = IntLib.GetIntFromByteArray(
                                                    result.Content,
                                                    start,
                                                    dataFormat
                                                );
                                                break;
                                            case DataType.UInt:
                                                variable.VarValue = UIntLib.GetUIntFromByteArray(
                                                    result.Content,
                                                    start,
                                                    dataFormat
                                                );
                                                break;
                                            case DataType.Float:
                                                if (variable.OffsetOrLength != 0)
                                                {
                                                    variable.VarValue =
                                                   Math.Round(FloatLib.GetFloatFromByteArray(
                                                   result.Content,
                                                   start,
                                                   dataFormat
                                               ), variable.OffsetOrLength);

                                                }
                                                else
                                                {
                                                    variable.VarValue = FloatLib.GetFloatFromByteArray(
                                                        result.Content,
                                                        start,
                                                        dataFormat);//如果 OffsetOrLength ==0 则直接输出浮点数
                                                }

                                                break;
                                            case DataType.Double:
                                                variable.VarValue =
                                                    DoubleLib.GetDoubleFromByteArray(
                                                        result.Content,
                                                        start,
                                                        dataFormat
                                                    );
                                                break;
                                            case DataType.Long:
                                                variable.VarValue = LongLib.GetLongFromByteArray(
                                                    result.Content,
                                                    start,
                                                    dataFormat
                                                );
                                                break;
                                            case DataType.ULong:
                                                variable.VarValue = ULongLib.GetULongFromByteArray(
                                                    result.Content,
                                                    start,
                                                    dataFormat
                                                );
                                                break;
                                            case DataType.String:
                                                variable.VarValue =
                                                    StringLib.GetStringFromByteArrayByEncoding(
                                                        result.Content,
                                                        start,
                                                        variable.OffsetOrLength,
                                                        Encoding.ASCII
                                                    );
                                                break;
                                            case DataType.ByteArray:
                                                variable.VarValue =
                                                    ByteArrayLib.GetByteArrayFromByteArray(
                                                        result.Content,
                                                        start,
                                                        variable.OffsetOrLength
                                                    );
                                                break;
                                            case DataType.HexString:
                                                variable.VarValue =
                                                    StringLib.GetHexStringFromByteArray(
                                                        result.Content,
                                                        start,
                                                        variable.OffsetOrLength
                                                    );
                                                break;
                                            default:
                                                break;
                                        }

                                        // 处理
                                        // 先做线性转换，再更新
                                        variable.VarValue = MigrationLib
                                            .GetMigrationValue(
                                                variable.VarValue,
                                                variable.Scale.ToString(),
                                                variable.Offset.ToString()
                                            )
                                            .Content;
                                        device.UpdateVariable(variable, plcEnum);
                                    }
                                }
                                else
                                {
                                    device.IsConnected = false;
                                }
                                break;
                        }
                    }
                    sw.Stop();
                }
                else
                {
                    Thread.Sleep(1000);
                    var a = mitsubishiPLC.ConnectServer();
                    device.IsConnected = a.IsSuccess;
                }
            }
        }
        private Device LoadDevice(string devicePath, string groupPath, string variablePath)
        {
            if (!System.IO.File.Exists(devicePath))
            {
                CommonMethods.AddLog?.Invoke(1, "设备文件不存在！");
                return null;
            }

            List<Group> groupList = LoadGroup(groupPath, variablePath);

            if (groupList != null && groupList.Count > 0)
            {
                try
                {
                    return new Device()
                    {
                        IPAddress = IniConfigHelper.ReadIniData(
                            "设备参数",
                            "IP地址",
                            "127.0.0.1",
                            devicePath
                        ),
                        Port = Convert.ToInt32(
                            IniConfigHelper.ReadIniData("设备参数", "端口号", "502", devicePath)
                        ),
                        GroupList = groupList,
                    };
                }
                catch
                {
                    // 日志写入
                    CommonMethods.AddLog?.Invoke(0, "加载设备信息失败！");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取变量默认值
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <returns>返回默认数据类型值</returns>
        private object GetDefualtValue(DataType dataType)
        {
            try
            {
                switch (dataType)
                {
                    case DataType.Bool:
                        return default(bool);
                    case DataType.Byte:
                        return default(byte);
                    case DataType.Short:
                        return default(short);
                    case DataType.UShort:
                        return default(ushort);
                    case DataType.Int:
                        return default(int);
                    case DataType.UInt:
                        return default(uint);
                    case DataType.Float:
                        return default(float);
                    case DataType.Double:
                        return default(double);
                    case DataType.Long:
                        return default(long);
                    case DataType.ULong:
                        return default(ulong);
                    case DataType.String:
                        return default(string);
                    case DataType.ByteArray:
                        return default(Byte[]);
                    case DataType.HexString:
                        return default(string);
                    default:
                        return default(bool);
                }
            }
            catch (Exception ex)
            {
                return default(bool);
            }
        }

        private void CheckVariableType(List<Group> groups, IPLCValue plcValue)
        {
            try
            {
                McAddressData addressData = McAddressData.ParseMelsecFrom(plcValue.ValueAddress, 1).Content;
                int addressStart = addressData.AddressStart;
                string code = addressData.McDataType.AsciiCode;
                foreach (var item1 in groups)
                {
                    var aa = McAddressData.ParseMelsecFrom(item1.StartAddress, 1).Content;
                    if (
                        aa.McDataType.AsciiCode == code
                        && aa.AddressStart <= addressStart
                        && (aa.AddressStart + item1.Length) >= addressStart
                    )
                    {
                        var ddd = item1
                            .VarList.Where(dd => dd.VarName == plcValue.ValueAddress)
                            .FirstOrDefault();
                        if (ddd != null)
                        {
                            ddd.DataType = plcValue.DataType.ToString();
                            ddd.OffsetOrLength = plcValue.OffsetOrLength;
                            ddd.Scale = plcValue.Scale;
                            ddd.Remark = plcValue.ParameterName;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private void CheckVariableType(List<Group> groups, string valueAddress, DataType dataType)
        {
            try
            {
                McAddressData addressData = McAddressData.ParseMelsecFrom(valueAddress, 1).Content;
                int addressStart = addressData.AddressStart;
                string code = addressData.McDataType.AsciiCode;
                foreach (var item1 in groups)
                {
                    var aa = McAddressData.ParseMelsecFrom(item1.StartAddress, 1).Content;
                    if (
                        aa.McDataType.AsciiCode == code
                        && aa.AddressStart <= addressStart
                        && (aa.AddressStart + item1.Length) >= addressStart
                    )
                    {
                        var ddd = item1
                            .VarList.Where(dd => dd.VarName == valueAddress)
                            .FirstOrDefault();
                        if (ddd != null)
                        {
                            ddd.DataType = dataType.ToString();
                            if (dataType == DataType.String)
                            {
                                ddd.OffsetOrLength = 40;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private string GetFile(int No, string FileName)
        {
            try
            {
                string baseplcpath = @"C:\PLCConfig";
                string baseplcfolder = $"PLC{No}Config";
                return Path.Combine(baseplcpath, baseplcfolder, FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}

