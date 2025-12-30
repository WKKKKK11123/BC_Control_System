using Prism.Commands;
using System.Data;
using System.IO;
using System.Text;
using thinger.DataConvertLib;
using ZC_Control_EFAM;
using BC_Control_Helper;
using BC_Control_Models;
using ZCCommunication.Core.Address;

namespace BC_Control_System.Command
{
    public static class CommonStaticMethods
    {
        private static IPLCHelper _plcHelper;
        public static DelegateCommand<object> ClickCommand { get; } =
            new DelegateCommand<object>(ExportExcel);
        public static DelegateCommand<object> ClickValveCommand { get; } =
            new DelegateCommand<object>(SetValveStatus);
        public static DelegateCommand<string> OpenRecipeViewCommand { get; }
        public static void OnInitialized(IPLCHelper pLCHelper)
        {
            _plcHelper = pLCHelper;
        }
        public static void ExportExcel(object actualTable)
        {
            
        }

        public static void SetValveStatus(object AddressValve)
        {
            try
            {
                if (!(AddressValve is DataClass))
                    return;
                DataClass dt = AddressValve as DataClass;
                bool result = false;
                if (string.IsNullOrEmpty(dt.SettingValueAddress))
                {
                    return;
                }
                if (!bool.TryParse(dt.SettingValue, out result))
                {
                    return;
                }
                _plcHelper.SelectPLC(dt.PLC).Write(dt.SettingValueAddress, !result);
            }
            catch (Exception ee) { }
        }

        public static void SetVariableType(string valueAddress, DataType dataType, PlcEnum plcEnum)
        {
            switch (plcEnum)
            {
                case PlcEnum.PLC1:
                    CheckVariableType(CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                case PlcEnum.PLC2:
                    CheckVariableType(PLC2CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                case PlcEnum.PLC3:
                    CheckVariableType(PLC3CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                case PlcEnum.PLC4:
                    CheckVariableType(PLC4CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                case PlcEnum.PLC5:
                    CheckVariableType(PLC5CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                //case PlcEnum.PLC6:
                //    CheckVariableType(PLC6CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                default:
                    CheckVariableType(CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
            }
        }
        public static void SetVariableType(string valueAddress, IPLCValue plcValue)
        {
            switch (plcValue.PLC)
            {
                case PlcEnum.PLC1:
                    CheckVariableType(CommonMethods.Device.GroupList, plcValue);
                    break;
                case PlcEnum.PLC2:
                    CheckVariableType(PLC2CommonMethods.Device.GroupList, plcValue);
                    break;
                case PlcEnum.PLC3:
                    CheckVariableType(PLC3CommonMethods.Device.GroupList, plcValue);
                    break;
                case PlcEnum.PLC4:
                    CheckVariableType(PLC4CommonMethods.Device.GroupList, plcValue);
                    break;
                case PlcEnum.PLC5:
                    CheckVariableType(PLC5CommonMethods.Device.GroupList, plcValue);
                    break;
                    //case PlcEnum.PLC6:
                    //    CheckVariableType(PLC6CommonMethods.Device.GroupList, valueAddress, dataType);
                    break;
                default:
                    CheckVariableType(CommonMethods.Device.GroupList, plcValue);
                    break;
            }
        }
        private static void CheckVariableType(
           List<Group> groups,
           IPLCValue plcValue
       )
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
                        ddd.Remark= plcValue.ParameterName;
                    }
                    break;
                }
            }
        }
        private static void CheckVariableType(
            List<Group> groups,
            string valueAddress,
            DataType dataType
        )
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
        public static void CopyListVariableType(PlcEnum plcEnum,List<IVariable> variableCollection)
        {
            try
            {
                switch (plcEnum)
                {
                    case PlcEnum.PLC1:
                        
                        break;  
                    case PlcEnum.PLC2:
                        break;
                    case PlcEnum.PLC3:
                        break;
                    case PlcEnum.PLC4:
                        break;
                    case PlcEnum.PLC5:
                        break;
                    case PlcEnum.PLC6:
                        break;
                    case PlcEnum.PLC7:
                        break;
                    case PlcEnum.PLC8:
                        break;
                    case PlcEnum.PLC9:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static void ReadPLCToPusher(PusherStationState pusherStationState)
        {
            if (CommonMethods.Device.IsConnected)
            {
                var a = CommonMethods.PLC.ReadInt16("D5020", 300);
                if (a.IsSuccess)
                {
                    pusherStationState.WaferCount = a.Content[169];

                    pusherStationState.Even_Data.RecipeName = CommonMethods.PLC.ReadString("D5120",20).ToString();
                    pusherStationState.Odd_Data.RecipeName = CommonMethods.PLC.ReadString("D5120", 20).ToString();

                    for (int i = 0; i < 50; i++)
                    {
                        pusherStationState.ClearWaferMap[i] = (WaferMapStation)a.Content[170 + i];
                    }

                    pusherStationState.Odd_Data.RFID = CommonMethods
                        .PLC.ReadString("D5020", 20)
                        .Content.Replace("\0", "");
                    ;
                    pusherStationState.Odd_Data.StorageID = (StorageID)a.Content[10];
                    pusherStationState.Odd_Data.WaferCount = a.Content[11];

                    for (int i = 0; i < 25; i++)
                    {
                        pusherStationState.Odd_Data.ClearWaferMap[i] = (WaferMapStation)
                            a.Content[20 + i];
                    }
                    pusherStationState.Odd_Data.OddEven = (OddEven)a.Content[45];
                    pusherStationState.Odd_Data.IsWafer = a.Content[46] == 1;

                    pusherStationState.Even_Data.RFID = CommonMethods
                        .PLC.ReadString("D5070", 20)
                        .Content.Replace("\0", "");
                    pusherStationState.Even_Data.StorageID = (StorageID)a.Content[60];
                    pusherStationState.Even_Data.WaferCount = a.Content[61];

                    for (int i = 0; i < 25; i++)
                    {
                        pusherStationState.Even_Data.ClearWaferMap[i] = (WaferMapStation)
                            a.Content[70 + i];
                    }
                    pusherStationState.Even_Data.OddEven = (OddEven)a.Content[95];
                    pusherStationState.Even_Data.IsWafer = a.Content[96] == 1;
                }
            }
        }

        public static void WritePusherToPLC(PusherStationState pusherStationState)
        {
            while (!CommonMethods.Device.IsConnected)
            {
                Thread.Sleep(50);
            }
            var a = Enumerable.Repeat((short)0, 300).ToList();
            CommonMethods.PLC.Write("D5020", a.ToArray());//新增 清除数据

            a[169] = (short)pusherStationState.WaferCount;

            for (int i = 0; i < 50; i++)
            {
                a[170 + i] = (short)pusherStationState.WaferMap[i];
            }
            var b = ConvertStringToShortArray(pusherStationState.Odd_Data.RFID);

            for (int i = 0; i < b.Count; i++)
            {
                a[i] = b[i];
            }
            a[10] = (short)pusherStationState.Odd_Data.StorageID;
            a[11] = (short)pusherStationState.Odd_Data.WaferCount;

            for (int i = 0; i < 25; i++)
            {
                a[20 + i] = (short)pusherStationState.Odd_Data.WaferMap[i];
            }
            a[45] = (short)pusherStationState.Odd_Data.OddEven;
            a[46] = pusherStationState.Odd_Data.IsWafer ? (short)1 : (short)0;

            var c = ConvertStringToShortArray(pusherStationState.Even_Data.RFID);
            for (int i = 0; i < c.Count; i++)
            {
                a[i + 50] = c[i];
            }
            a[60] = (short)pusherStationState.Even_Data.StorageID;
            a[61] = (short)pusherStationState.Even_Data.WaferCount;

            for (int i = 0; i < 25; i++)
            {
                a[70 + i] = (short)pusherStationState.Even_Data.WaferMap[i];
            }
            a[95] = (short)pusherStationState.Even_Data.OddEven;
            a[96] = pusherStationState.Even_Data.IsWafer ? (short)1 : (short)0;

            while (!CommonMethods.Device.IsConnected)
            {
                Thread.Sleep(50);
            }
            CommonMethods.PLC.Write("D5020", a.ToArray());
            // CommonMethods.PLC.Write("M4135",true);
        }

        public static List<short> ConvertStringToShortArray(string recipeName)
        {
            // 1. 字符串转UTF8字节数组
            byte[] bytes = thinger.DataConvertLib.ByteArrayLib.GetByteArrayFromString(
                recipeName,
                Encoding.UTF8
            );

            // 2. 处理奇数长度字节数组（补零）
            if (bytes.Length % 2 != 0)
            {
                Array.Resize(ref bytes, bytes.Length + 1);
                bytes[bytes.Length - 1] = 0; //
            }

            // 3. 字节数组转short数组（DCBA格式）
            List<short> result = thinger
                .DataConvertLib.ShortLib.GetShortArrayFromByteArray(
                    bytes,
                    thinger.DataConvertLib.DataFormat.DCBA
                )
                .ToList();

            return result;
        }
    }
}
