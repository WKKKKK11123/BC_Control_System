using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_EFAM
{
    public class MegModel
    {
        public string DateTime
        {
            get => System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public long UUID { get; set; }

        public StationID Station { get; set; }

        public string StationName
        {
            get => Enum.GetName(typeof(StationID), Station);
        }

        public string Message { get; set; }

        public int EventID { get; set; }

        public string MesType { get; set; }

        public bool Result { get; set; }

        public string ErrorCode { get; set; }

        public byte[] State { get; set; }

        public string AdditionalData { get; set; }
    }

    public enum StationID
    {
        Opener_1,
        Opener_2,
        LP_1,
        LP_2,
        LP_3,
        LP_4,
        Storage_1,
        Storage_2,
        Storage_3,
        Storage_4,
        Storage_5,
        Storage_6,
        Storage_7,
        Storage_8,
        Storage_9,
        Storage_10,
        Storage_11,
        Storage_12,
        Storage_13,
        Storage_14,
        Control_Door_1,
        Control_Door_2,
        Control_Door_3,
        Control_Door_4,
        RFID_Reader,
        FTR,
        Pusher,
        Optional,
        HV,
        WHR,
        FoupBufferClamp,
        Mapper,
        Storage_15,
        Storage_16,
        Storage_17,
        Storage_18,
    }

    public enum StorageID
    {
        Storage_1,
        Storage_2,
        Storage_3,
        Storage_4,
        Storage_5,
        Storage_6,
        Storage_7,
        Storage_8,
        Storage_9,
        Storage_10,
        Storage_11,
        Storage_12,
        Storage_13,
        Storage_14,
        Storage_15,
        Storage_16,
        Storage_17,
        Storage_18,
    }

    public enum FTRStationID
    {
        LP_1,
        LP_2,
        LP_3,
        LP_4,
        Opener_1,
        Opener_2,
        Storage_1,
        Storage_2,
        Storage_3,
        Storage_4,
        Storage_5,
        Storage_6,
        Storage_7,
        Storage_8,
        Storage_9,
        Storage_10,
        Storage_11,
        Storage_12,
        Storage_13,
        Storage_14,
        Storage_15,
        Storage_16,
        Storage_17,
        Storage_18,
    }

    public enum WHRStationID
    {
        Opener_1 = 0x08,
        Opener_2 = 0x05,
        HV = 0x16,
    }

    public enum ActionType
    {
        Get = 0x0C,
        Put,
        Ready
    }

    public enum WHRActionType
    {
        Get = 0x2C,
        Put = 0x2D,
        Ready = 0x20
    }

    public enum ArmType
    {
        DirtyArm,
        CleanArm,
    }

    public enum WTRArmType
    {
        DirtyArm,
        CleanArm,
        DirtyArmNoReturn,
        DirtyArmReturn,
        CleanArmNoReturn,
        CleanArmReturn,
    }

    public enum OddEven
    {
        Odd,
        Even,
    }

    public enum PusherMoveMode
    {
        Move = 0x14,
        Track_Move = 0x10,
    }

    public enum PusherToHV_Func
    {
        Get,
        Put,
    }

    public enum LPFuncType
    {
        Open_Door = 0x0B,
        Close_Door = 0x0C,
        Lock = 0x0E,
        LockUnlock = 0x0F
    }

    public enum HV_FlipFuncType
    {
        Flip_Degree0,
        Flip_Degree90,
        Flip_Degree180,
        Flip_Degree270
    }

    public enum HV_LockFuncType
    {
        Dirty_Lock,
        Clean_Lock,
        Dirty_UnLock,
        Clean_UnLock
    }

    public enum SysMode
    {
        Local,
        Remote,
    }

    public enum WaferMapStation
    {
        Empty,
        Present,
        Crossed,
        Unknown,
        Double,
        Not_found_on_Pick_Up
    }

    public enum ProcessState
    {
        UnProcess,
        Processing,
        WaitProcess,
        UnSelectProcess,
        Processed,
        MapError
    }

    public enum BatchState
    {
        Process,
        Stop,
        Pause,
        Abort
    }
    
}
