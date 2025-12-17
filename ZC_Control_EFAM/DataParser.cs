using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZC_Control_EFAM
{
    public partial class ZC_EFAM_Data
    {
        /// <summary>
        /// 读取系统状态的方法
        /// </summary>
        private async Task ReadWTSStatus()
        {
            MegModel result;
            while (true)
            {
                if (tcpHexClient.IsConnected)
                {
                    result = await GetStatusCommand(StationID.HV);
                    result = await GetStatusCommand(StationID.Pusher);
                    result = await GetStatusCommand(StationID.Opener_1);
                    result = await GetStatusCommand(StationID.Opener_2);
                    result = await GetStatusCommand(StationID.WHR);
                    result = await GetStatusCommand(StationID.FTR);
                    result = await Sys_Get_LPStatus();
                    result = await Sys_Get_AllStorage_Status();
                }
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 子命令报文分发（因机构回复有所差别所以分发到具体的模块来解析）
        /// </summary>
        /// <param name="len">报文标识符</param>
        /// <param name="list">报文</param>
        private void SubCommDataParser(long len, List<byte> list)
        {
            switch (list[6])
            {
                case 0x00:
                case 0x01:
                    Opener_SubCommandParser(len, list);
                    break;
                case var a when (a >= 0x14 && a <= 0x17):
                    ControlDoor_SubCommandParser(len, list);
                    break;
                case 0x18:
                    RFID_SubCommandParser(len, list);
                    break;
                case 0x19:
                    FTR_SubCommandParser(len, list);
                    break;

                case 0x1A:
                    Pusher_SubCommandParser(len, list);
                    break;

                case 0x1B:
                    break;
                case 0x1C:
                    HV_SubCommandParser(len, list);
                    break;

                case 0x1D:
                    WHR_SubCommandParser(len, list);
                    break;
            }
        }

        /// <summary>
        /// Pusher报文解析方法
        /// </summary>
        /// <param name="len">报文标识符</param>
        /// <param name="list">报文</param>
        private void Pusher_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x61:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            State = new byte[] { list[7 + 12] },
                            Station = (StationID)list[6]
                        }
                    );

                    Pusher_Data.PlaceSenser = GetBit(list[7 + 12], 8);
                    Pusher_Data.Rotate_0_Degree = GetBit(list[7 + 12], 7);
                    Pusher_Data.Rotate_180_Degree = GetBit(list[7 + 12], 6);
                    Pusher_Data.Dirty_Comb = GetBit(list[7 + 12], 5);
                    Pusher_Data.Clean_Comb = GetBit(list[7 + 12], 4);
                    BufferPlaceSenser = GetBit(list[7 + 12], 3);

                    break;

                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(10).Take(1).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x66:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(3).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x73:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;
                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        /// <summary>
        /// LP报文解析方法
        /// </summary>
        /// <param name="len">报文标识符</param>
        /// <param name="list">报文</param>
        private void ControlDoor_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x61:
                    //ReceivedQueue.TryAdd(
                    //    Guid.NewGuid(),
                    //    new MegModel()
                    //    {
                    //        UUID = len,
                    //        Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                    //        Result = true,
                    //        MesType = "SubSystem",
                    //        State = new byte[] { list[7 + 12]},
                    //        Station = (StationID)list[6]
                    //    }
                    //);


                    //switch ((StationID)list[6])
                    //{
                    //    case StationID.Control_Door_1:
                    //        Loadport_Data[0].PlaceSenser = GetBit(list[7 + 12], 3) && GetBit(list[7 + 12], 4) && GetBit(list[7 + 12], 5) && GetBit(list[7 + 12], 6);
                    //        break;
                    //    case StationID.Control_Door_2:
                    //        Loadport_Data[1].PlaceSenser = GetBit(list[7 + 12], 3) && GetBit(list[7 + 12], 4) && GetBit(list[7 + 12], 5) && GetBit(list[7 + 12], 6);
                    //        break;
                    //    case StationID.Control_Door_3:
                    //        Loadport_Data[2].PlaceSenser = GetBit(list[7 + 12], 3) && GetBit(list[7 + 12], 4) && GetBit(list[7 + 12], 5) && GetBit(list[7 + 12], 6);
                    //        break;
                    //    case StationID.Control_Door_4:
                    //        Loadport_Data[3].PlaceSenser = GetBit(list[7 + 12], 3) && GetBit(list[7 + 12], 4) && GetBit(list[7 + 12], 5) && GetBit(list[7 + 12], 6);
                    //        break;

                    //    default:
                    //        break;
                    //}
                    break;

                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(10).Take(1).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x66:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(3).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        /// <summary>
        /// HV报文解析方法
        /// </summary>
        /// <param name="len">报文标识符</param>
        /// <param name="list">报文</param>
        private void HV_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x61:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            State = new byte[] { list[7 + 12], list[7 + 13], list[7 + 14] },
                            Station = (StationID)list[6]
                        }
                    );
                    HV_Data.PlaceSenser = GetBit(list[7 + 12], 4);

                    HV_Data.Flip_0_Degree = GetBit(list[7 + 12], 8);
                    HV_Data.Flip_90_Degree = GetBit(list[7 + 12], 7);
                    HV_Data.Flip_180_Degree = GetBit(list[7 + 12], 6);
                    HV_Data.Flip_270_Degree = GetBit(list[7 + 12], 5);

                    HV_Data.H1_Clean_Open = GetBit(list[7 + 13], 7);
                    HV_Data.H1_Clean_Close = GetBit(list[7 + 13], 6);
                    HV_Data.H1_Dirty_Open = GetBit(list[7 + 13], 5);
                    HV_Data.H1_Dirty_Close = GetBit(list[7 + 13], 4);

                    HV_Data.H2_Clean_Open = GetBit(list[7 + 13], 3);
                    HV_Data.H2_Clean_Close = GetBit(list[7 + 13], 2);
                    HV_Data.H2_Dirty_Open = GetBit(list[7 + 13], 1);
                    HV_Data.H2_Dirty_Close = GetBit(list[7 + 14], 8);

                    break;

                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(10).Take(1).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x66:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(3).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        /// <summary>
        /// 读取RFID报文解析方法
        /// </summary>
        /// <param name="len">报文标识符</param>
        /// <param name="list">报文</param>
        private void RFID_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(1).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x71:
                    Loadport_Data[list[8] - 1].StationInfo.RFID = Encoding.ASCII.GetString(
                        list.Skip(10).Take(list.Count - 10).ToArray()
                    );
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = Loadport_Data[list[8] - 1].StationInfo.RFID.Length > 5,
                            MesType = "SubSystem",
                            Station = (StationID)list[6],
                            AdditionalData = Encoding.ASCII.GetString(
                                list.Skip(10).Take(list.Count - 10).ToArray()
                            )
                        }
                    );

                    break;
                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        /// <summary>
        /// Opener报文解析方法
        /// </summary>
        /// <param name="len">报文标识符</param>
        /// <param name="list">报文</param>
        private void Opener_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x61:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            State = new byte[] { list[7 + 12] },
                            Station = (StationID)list[6]
                        }
                    );
                    Opener_Data[list[6]].PlaceSenser =
                        GetBit(list[7 + 12], 8)
                        && GetBit(list[7 + 12], 7)
                        && GetBit(list[7 + 12], 6);
                    Opener_Data[list[6]].StandbyPos = GetBit(list[7 + 12], 2);
                    break;

                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(1).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x66:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(3).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        /// <summary>
        /// WHR报文解析方法
        /// </summary>
        /// <param name="len"></param>
        /// <param name="list"></param>
        private void WHR_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x61:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            State = new byte[] { list[7 + 12] },
                            Station = (StationID)list[6]
                        }
                    );

                    WHR_Data.CleanArmPos = GetBit(list[7 + 12], 3);

                    WHR_Data.DirtyArmPos = GetBit(list[7 + 12], 4);

                    WHR_Data.PlaceSenser = GetBit(list[7 + 12], 8);
                    break;

                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(10).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x66:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(3).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        /// <summary>
        /// FTR报文解析方法
        /// </summary>
        /// <param name="len"></param>
        /// <param name="list"></param>
        private void FTR_SubCommandParser(long len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x61:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            State = new byte[] { list[7 + 12] },
                            Station = (StationID)list[6]
                        }
                    );

                    FTR_Data.PlaceSenser =
                        GetBit(list[7 + 12], 8)
                        && GetBit(list[7 + 12], 7)
                        && GetBit(list[7 + 12], 6);
                    break;

                case 0x62:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = true,
                            MesType = "SubSystem",
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x63:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(10).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;

                case 0x66:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(3).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
                case 0xFF:
                    ReceivedQueue.TryAdd(
                        Guid.NewGuid(),
                        new MegModel()
                        {
                            UUID = len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            Result = false,
                            MesType = "SubSystem",
                            ErrorCode = BitConverter
                                .ToString(list.Skip(8).Take(2).ToArray())
                                .Replace("-", ""),
                            Station = (StationID)list[6]
                        }
                    );
                    break;
            }
        }

        private void SystemDataParser(long Len, List<byte> list)
        {
            bool result = false;
            switch (list[7])
            {
                case 0:
                    break;
                case 1:
                    result = true; //后边数据需要解析
                    //Parameter 1~m: Opener And LoadPort m Box Status(0: Box not present; 1: Box Present,
                    //0xFF: Unknown status).
                    Loadport_Data[0].PlaceSenser = list[10] == 1;
                    Loadport_Data[1].PlaceSenser = list[11] == 1;
                    Loadport_Data[2].PlaceSenser = list[12] == 1;
                    Loadport_Data[3].PlaceSenser = list[13] == 1;
                    break;
                case 2:
                    result = true; //后边数据需要解析
                    //Parameter 1: WHR Station
                    //Parameter 2~: Slot wafer present status(‘P’ (0x50): Present; ‘E’ (0x45): Empty; ‘C’ (0x43):
                    //Crossed; ‘?’ (0x3F): Unknown; ‘D’ (0x44): Double; ‘#’ (0x23): Not found on Pick Up)
                    break;
                case 3:
                    result = true; //后边数据需要解析
                    //Parameter 1: Control Mode (0: Local Mode; 1: Remote Mode)
                    ControlMode = list[8] == 1;
                    break;
                case 5:
                    result = true; //后边数据需要解析
                    //Parameter 1~m: Subsystem Status(0: Normal; 1: Not Normal).
                    //Where m is subsystem number.
                    break;
                case 6:
                    result = true; //后边数据需要解析
                    //Parameter 1: WHR end-effecter 1 present status (0: Not present; 1: Present; 0xFF: Unknown)
                    break;
                case 8:
                    result = true;
                    //: Command completed.
                    break;
                case 0x18:
                    result = true;

                    for (int i = 0; i < 18; i++)
                    {
                        Storage_Data[i].PlaceSenser = list[8 + i] == 1;
                    }
                    //Parameter 1~n: Storage present status.n = Number of Storage Station.
                    //1 = Present
                    //0 = Not present or unknown.
                    break;
                case 0x1A:
                    result = true;
                    //Parameter 1: Present Status.
                    //            1 = Present
                    //            0 = Empty
                    //            0xFF: Unknown status
                    break;
                case 0x20:
                    result = true;
                    //Get E84 Access Mode response.
                    //Parameter 1: LP Station
                    //            0x02: LP 1
                    //            0x03: LP 2
                    //            0x04: LP 3
                    //            0x05: LP 4
                    break;
                case 0x21:
                    result = true;
                    //Get Subsystem Wafer Present Status response.
                    //Parameter 1: Subsystem ID
                    //Parameter 2: Present Status.
                    //1 = Present
                    //0 = Empty
                    //0xFF: Unknown status
                    break;
                case 0x22:
                    result = true;
                    //Get E84 Timeout setting
                    //    Parameter 1: LP Station
                    //    0x02: LP 1
                    //    0x03: LP 2
                    //    0x04: LP 3
                    //    0x05: LP 4
                    //    Parameter 2:
                    //    0x01: TP1
                    //    0x02: TP2
                    //    0x03: TP3
                    //    0x04: TP4
                    //    0x05: TP5
                    //    0x06: TP6
                    //    Parameter 3 - 6:
                    //    Value(1 - 999)

                    break;
                case 0x24:
                    result = true;
                    //Get Pusher Comb response.
                    //    Parameter 1: Comb used
                    //    0 = Dirty Comb
                    //    1 = Clean Comb

                    break;
            }
            ReceivedQueue_1.TryAdd(
                Guid.NewGuid(),
                new MegModel()
                {
                    UUID = Len,
                    Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                    MesType = "SystemCommand",
                    Result = result
                }
            );
            //SystemCommQueue.Add(
            //    new MegModel()
            //    {
            //        UUID = Len,
            //        Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
            //        MesType = "SystemCommand",
            //        Result = result
            //    }
            //);
        }

        private void EventDataParser(long Len, List<byte> list)
        {
            switch (list[7])
            {
                case 0x02:
                    //在位状态发生改变
                    switch (list[8])
                    {
                        case 0x00:
                            Opener_Data[0].PlaceSenser = true;
                            break;
                        case 0x01:
                            Opener_Data[1].PlaceSenser = true;
                            break;
                        case 0x02:
                            Loadport_Data[0].PlaceSenser = true;
                            break;
                        case 0x03:
                            Loadport_Data[1].PlaceSenser = true;
                            break;
                        case 0x04:
                            Loadport_Data[2].PlaceSenser = true;
                            break;
                        case 0x05:
                            Loadport_Data[3].PlaceSenser = true;
                            break;
                        case 0x19:
                            FTR_Data.PlaceSenser = true;
                            break;
                        case 0x1A:
                            Pusher_Data.PlaceSenser = true;
                            break;
                        case 0x1B:
                            BufferPlaceSenser = true;
                            break;
                        case 0x1C:
                            HV_Data.PlaceSenser = true;
                            break;
                        case 0x1D:
                            WHR_Data.PlaceSenser = true;
                            break;
                        case 0x1E:
                            break;
                        case 0x1F:
                            break;
                        default:
                            break;
                    }
                    break;

                case 0x03:
                    //在位状态发生改变

                    switch (list[8])
                    {
                        case 0x00:
                            Opener_Data[0].PlaceSenser = false;
                            break;
                        case 0x01:
                            Opener_Data[1].PlaceSenser = false;
                            break;
                        case 0x02:
                            Loadport_Data[0].PlaceSenser = false;
                            break;
                        case 0x03:
                            Loadport_Data[1].PlaceSenser = false;
                            break;
                        case 0x04:
                            Loadport_Data[2].PlaceSenser = false;
                            break;
                        case 0x05:
                            Loadport_Data[3].PlaceSenser = false;
                            break;

                        case 0x19:
                            FTR_Data.PlaceSenser = false;
                            break;
                        case 0x1A:
                            Pusher_Data.PlaceSenser = false;
                            break;
                        case 0x1B:
                            BufferPlaceSenser = false;
                            break;
                        case 0x1C:
                            HV_Data.PlaceSenser = false;
                            break;
                        case 0x1D:
                            WHR_Data.PlaceSenser = false;
                            break;
                        case 0x1E:
                            break;
                        case 0x1F:
                            break;
                        default:
                            break;
                    }
                    break;

                case 0x05:
                    //控制模式发生改变
                    ControlMode = list[8] == 1;
                    break;

                case 0x21:
                    //在位状态发生改变
                    Storage_Data[list[8]].PlaceSenser = list[9] == 0 ? true : false;

                    break;

                case 0x28:
                    //Opener Map数据
                    //Opener_Data[list[8]].WaferMap = new List<WaferMapStation>(25);
                    List<WaferMapStation> qwe = new List<WaferMapStation>(25);
                    for (int i = 0; i < 25; i++)
                    {
                        //qwe.Add(
                        //    list[9 + i] switch
                        //    {
                        //        0x50 => WaferMapStation.Present,
                        //        0x45 => WaferMapStation.Empty,
                        //        0x43 => WaferMapStation.Crossed,
                        //        0x3F => WaferMapStation.Unknown,
                        //        0x44 => WaferMapStation.Double,
                        //        0x23 => WaferMapStation.Not_found_on_Pick_Up,
                        //        _ => WaferMapStation.Empty // 默认情况
                        //    }
                        //);
                        byte value = list[9 + i];
                        qwe.Add(
                            value == 0x50
                                ? WaferMapStation.Present
                                : value == 0x45
                                    ? WaferMapStation.Empty
                                    : value == 0x43
                                        ? WaferMapStation.Crossed
                                        : value == 0x3F
                                            ? WaferMapStation.Unknown
                                            : value == 0x44
                                                ? WaferMapStation.Double
                                                : value == 0x23
                                                    ? WaferMapStation.Not_found_on_Pick_Up
                                                    : WaferMapStation.Empty
                        );
                    }
                    Opener_Data[list[8]].StationInfo.WaferMap = qwe;
                    Opener_Data[list[8]].StationInfo.WaferCount = qwe.Where(c =>
                            c == WaferMapStation.Present
                        )
                        .Count();
                    Opener_Data[list[8]].StationInfo.MAPError =
                        Opener_Data[list[8]]
                            .StationInfo.WaferMap.Count(c =>
                                c != WaferMapStation.Empty && c != WaferMapStation.Present
                            ) > 0;
                    Opener_Data[list[8]].IsMapResult = true;
                    break;

                case 0x29:
                    //Pusher Map数据
                    Pusher_Data.WaferMap = new List<WaferMapStation>(50);

                    for (int i = 0; i < 50; i++)
                    {
                        //Pusher_Data.WaferMap.Add(
                        //    list[9 + i] switch
                        //    {
                        //        0x50 => WaferMapStation.Present,
                        //        0x45 => WaferMapStation.Empty,
                        //        0x43 => WaferMapStation.Crossed,
                        //        0x3F => WaferMapStation.Unknown,
                        //        0x44 => WaferMapStation.Double,
                        //        0x23 => WaferMapStation.Not_found_on_Pick_Up,
                        //        _ => WaferMapStation.Empty // 默认情况
                        //    }
                        //);


                        byte value = list[9 + i];
                        Pusher_Data.WaferMap.Add(
                            value == 0x50
                                ? WaferMapStation.Present
                                : value == 0x45
                                    ? WaferMapStation.Empty
                                    : value == 0x43
                                        ? WaferMapStation.Crossed
                                        : value == 0x3F
                                            ? WaferMapStation.Unknown
                                            : value == 0x44
                                                ? WaferMapStation.Double
                                                : value == 0x23
                                                    ? WaferMapStation.Not_found_on_Pick_Up
                                                    : WaferMapStation.Empty // 默认情况
                        );
                    }

                    Pusher_Data.WaferCount = Pusher_Data
                        .WaferMap.Where(c => c == WaferMapStation.Present)
                        .Count();
                    Pusher_Data.IsMapResult = true;
                    break;

                default:
                    break;
            }
        }
    }
}
