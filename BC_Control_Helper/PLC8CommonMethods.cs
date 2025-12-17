using Newtonsoft.Json;
using thinger.DataConvertLib;
using BC_Control_Models;
using ZCCommunication.Core.Address;

namespace BC_Control_Helper
{
    public static class PLC8CommonMethods
    {
        // 创建一个设备对象
        public static Device Device
        {
            get; set;
        }
        /// <summary>
        /// 确定大小端
        /// </summary>
        public static thinger.DataConvertLib.DataFormat dataFormat = thinger.DataConvertLib.DataFormat.DCBA;

        /// <summary>
        /// 总配方集合
        /// </summary>
        public static List<ToolRecipeModels> ToolRecipeList = new List<ToolRecipeModels>();

        /// <summary>
        /// Json 文件自动缩进
        /// </summary>
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置为Indented，即使用缩进和换行符
        };



        /// <summary>
        /// 创建一个静态的系统委托，用于日志添加
        /// </summary>
        public static Action<int, string> AddLog;

        /// <summary>
        /// Modbus通信对象
        /// </summary>
        public static ModbusTCP Modbus
        {
            get; set;
        }

        /// <summary>
        /// PLC通信对象
        /// </summary>
        public static MitsubishiPLC PLC
        {
            get; set;
        }

        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public static SysAdmin CurrentAdmin
        {
            get; set;
        }


        /// <summary>
        /// 通过标签名程找变量
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static Variable FindVariable(string varName)
        {
            foreach (var item in Device.GroupList)
            {
                var res = item.VarList.Find(c => c.VarName == varName);

                if (res != null)
                {
                    return res;
                }

            }

            return null;
        }


        /// <summary>
        /// 获取当前日期
        /// </summary>
        public static string CurrentDate
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd");
            }

        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        public static string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }

        }
        /// <summary>
        /// 获取当前的日期加时间
        /// </summary>
        public static string CurrentDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }

        }

        /// <summary>
        /// 通用写入的方法
        /// </summary>
        /// <param name="varName">变量名称</param>
        /// <param name="varValue">变量值</param>
        /// <returns></returns>
        public static int[] CommonRead(string address, ushort lengtht)
        {
            int[] Result = new int[30];
            //   Result = PLC.ReadInt16;
            return Result;
        }

        public static bool CommonWrite(string varName, string varValue)
        {
            // 1,找到变量
            var variable = FindVariable(varName);

            if (variable != null)
            {
                // 2.获取变量类型
                thinger.DataConvertLib.DataType dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType, true);

                // 3.获取写入数据
                var result = MigrationLib.SetMigrationValue(varValue, dataType, variable.Scale.ToString(), variable.Offset.ToString());

                if (result.IsSuccess)
                {
                    var gp = Device.GroupList.Find(g => g.GroupName == variable.GroupName);

                    if (gp != null)
                    {
                        var mcAddressData = McAddressData.ParseMelsecFrom(gp.StartAddress, gp.Length);
                        if (mcAddressData.IsSuccess)
                        {
                            mcAddressData.Content.AddressStart = mcAddressData.Content.AddressStart + variable.Start;

                            try
                            {
                                ZCCommunication.OperateResult writeResult = new ZCCommunication.OperateResult();

                                var variableTemp = CopyVariable(variable);
                                variableTemp.VarValue = result.Content;

                                switch (dataType)
                                {
                                    case DataType.Bool:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToBoolean(result.Content));
                                        break;
                                    case DataType.Short:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToInt16(result.Content));
                                        break;
                                    case DataType.UShort:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToUInt16(result.Content));
                                        break;
                                    case DataType.Int:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToInt32(result.Content));
                                        break;
                                    case DataType.UInt:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToUInt32(result.Content));
                                        break;
                                    case DataType.Float:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToSingle(result.Content));
                                        break;
                                    case DataType.Double:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToDouble(result.Content));
                                        break;
                                    case DataType.Long:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToInt64(result.Content));
                                        break;
                                    case DataType.ULong:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), Convert.ToUInt64(result.Content));
                                        break;
                                    case DataType.String:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), result.Content, variable.OffsetOrLength);
                                        break;
                                    case DataType.ByteArray:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), ByteArrayLib.GetByteArrayFromHexString(result.Content));
                                        break;
                                    case DataType.HexString:
                                        writeResult = PLC.Write(mcAddressData.Content.ToString(), ByteArrayLib.GetByteArrayFromHexString(result.Content));
                                        break;
                                    default:
                                        break;
                                }

                                if (writeResult.IsSuccess)
                                {
                                    Device.WriteOperatorLog(variableTemp);
                                    return true;
                                }
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }

                    }

                }
            }

            return false;

        }





        private static Variable CopyVariable(Variable variable)
        {

            return new Variable()
            {

                VarName = variable.VarName,
                Start = variable.Start,
                DataType = variable.DataType,
                OffsetOrLength = variable.OffsetOrLength,
                GroupName = variable.GroupName,
                Remark = variable.Remark,
                PosAlarm = variable.PosAlarm,
                NegAlarm = variable.NegAlarm,
                Scale = variable.Scale,
                Offset = variable.Offset,
                Priority = variable.Priority,
                OperatorLog = variable.OperatorLog,
                VarValue = variable.VarValue,
                PreVarValue = variable.PreVarValue,
                PosAlarmTime = variable.PosAlarmTime,
                NegAlarmTime = variable.NegAlarmTime,

            };

        }



        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        public static T ConvertObject<T>(object asObject) where T : new()
        {
            //此方法将object对象转换为json字符
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(asObject);
            //再将json字符转换为实体对象
            var t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}
