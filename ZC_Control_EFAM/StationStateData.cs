using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace ZC_Control_EFAM
{
    [AddINotifyPropertyChangedInterface]
    public class StationInfo
    {
        public CarrierProcessQueueModel Batchid { get; set; } = new CarrierProcessQueueModel();

        /// <summary>
        /// 是否是空Foup
        /// </summary>
        //public bool IsNullFoup { get; set; }

        /// <summary>
        /// 制程状态
        /// </summary>
        public ProcessState ProcessState { get; set; }

        /// <summary>
        /// 工位RFID
        /// </summary>
        public string RFID { get; set; }

        public string RecipeName { get; set; }

        /// <summary>
        /// 工位的MAP数据
        /// </summary>
        public List<WaferMapStation> WaferMap { get; set; } = new List<WaferMapStation>();

        /// <summary>
        /// 清洗后的MAP数据
        /// </summary>
        public List<WaferMapStation> ClearWaferMap { get; set; } = new List<WaferMapStation>();

        /// <summary>
        /// 工位的晶圆总数
        /// </summary>
        public int? WaferCount { get; set; }

        /// <summary>
        /// 是否有货
        /// </summary>
        public bool IsWafer { get; set; }

        /// <summary>
        /// 表示工位的晶圆是否清洗过 false:脏  true:干净的
        /// </summary>
        //public ArmType DirtyClean { get; set; }

        /// <summary>
        /// 表示工位的晶圆来自哪个LP
        /// </summary>
        public StationID Form_LP { get; set; }

        /// <summary>
        /// 表示工位的晶圆的FOUP放在哪个Storage
        /// </summary>
        public StorageID StorageID { get; set; }

        /// <summary>
        /// 表示工位的奇偶  1:奇  2:偶
        /// </summary>
        public OddEven OddEven { get; set; }

        /// <summary>
        /// 表示是否是双批作业
        /// </summary>
        public bool DoubleProcess { get; set; }

        /// <summary>
        /// Map报错
        /// </summary>
        public bool MAPError { get; set; }

        public event Action propertyChanged;

        public void OnIsWaferChanged()
        {
            propertyChanged?.Invoke();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class StationStateBase
    {
        public StationStateBase()
        {
            StationInfo.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            PreDatetime = DateTime.Now;
            UpdataProperty();
        }

        [JsonIgnore]
        [DoNotNotify]
        public DateTime PreDatetime { get; set; }

        [JsonIgnore]
        [DoNotNotify]
        public bool _loadComplete { get; set; }

        [JsonIgnore]
        [DoNotNotify]
        public bool VerifyStatus { get; set; }

        /// <summary>
        /// 表示工位的晶圆来自哪个LP
        /// </summary>

        [JsonIgnore]
        [DoNotNotify]
        public StationID Out_LP { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public StationID StationID { get; set; }

        /// <summary>
        /// 在FTR区域的工位ID
        /// </summary>
        public FTRStationID FTRStationID { get; set; }

        /// <summary>
        /// 在WHR区域的工位ID
        /// </summary>
        public WHRStationID WHRStationID { get; set; }

        [AlsoNotifyFor(nameof(StationInfo))]
        public StationInfo StationInfo { get; set; } = new StationInfo() { };

        /// <summary>
        /// 是否MAP完成
        /// </summary>
        public bool IsMapComplete { get; set; }

        public string StationName { get; set; }

        /// <summary>
        /// 检知Senser
        /// </summary>
        public bool PlaceSenser { get; set; }

        /// <summary>
        /// 表示工位的的错误信息
        /// </summary>
        public string ErrorCode { get; set; }

        public override string ToString()
        {
            // 获取当前对象的所有公共属性
            var properties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead); // 只取可读属性

            // 拼接属性名和值
            var result = string.Join(
                ", ",
                properties.Select(prop => $"{prop.Name}: {prop.GetValue(this) ?? "null"}")
            );

            return result;
        }

        public virtual void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public void OnPlaceSenserChanged()
        {
            if (
                StationName == "LP1"
                || StationName == "LP2"
                || StationName == "LP3"
                || StationName == "LP4"
            )
            {
                if (!PlaceSenser)
                {
                    //StationInfo.DirtyClean = ArmType.DirtyArm;
                    StationInfo.ProcessState = ProcessState.UnProcess;

                    PreDatetime = DateTime.Now;
                }
                PreDatetime = DateTime.Now;
                UpdataProperty();
            }

            if (!PlaceSenser)
            {
                //VerifyStatus = false;
                Out_LP = 0;
            }
        }

        public virtual void OnStationInfoChanged()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }
    }

    public class StorageStation : StationStateBase
    {
        public StorageStation()
        {
            StationInfo.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            UpdataProperty();
        }

        public override void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public override void OnStationInfoChanged()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }
    }

    public class OpenerStationState : StationStateBase
    {
        public OpenerStationState()
        {
            StationInfo.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            UpdataProperty();
        }

        public bool StandbyPos { get; set; }

        public bool IsMapResult { get; set; }

        public override void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public override void OnStationInfoChanged()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }
    }

    public class WFRStationState : StationStateBase
    {
        public WFRStationState()
        {
            StationInfo.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            UpdataProperty();
        }

        public FTRStationID GetFTRStationID { get; set; }

        public FTRStationID PutFTRStationID { get; set; }

        public StationID GetGlobalStationID { get; set; }

        public StationID PutGlobalStationID { get; set; }

        public override void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public override void OnStationInfoChanged()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }
    }

    public class WHRStationState : StationStateBase
    {
        public WHRStationState()
        {
            StationInfo.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            UpdataProperty();
        }

        public WHRStationID GetWHRStationID { get; set; }
        public WHRStationID PutWHRStationID { get; set; }
        public bool DirtyArmPos { get; set; }

        public bool CleanArmPos { get; set; }

        public override void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public override void OnStationInfoChanged()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }
    }

    public class HVStationState : StationStateBase
    {
        public HVStationState()
        {
            StationInfo.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            UpdataProperty();
        }

        public bool Flip_0_Degree { get; set; }

        public bool Flip_90_Degree { get; set; }

        public bool Flip_180_Degree { get; set; }

        public bool Flip_270_Degree { get; set; }

        public bool H1_Clean_Open { get; set; }
        public bool H1_Clean_Close { get; set; }
        public bool H2_Clean_Open { get; set; }
        public bool H2_Clean_Close { get; set; }

        public bool H1_Dirty_Open { get; set; }
        public bool H1_Dirty_Close { get; set; }
        public bool H2_Dirty_Open { get; set; }
        public bool H2_Dirty_Close { get; set; }

        public override void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public override void OnStationInfoChanged()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class PusherStationState
    {
        public PusherStationState()
        {
            Odd_Data.propertyChanged += StationInfo_propertyChanged;
            Even_Data.propertyChanged += StationInfo_propertyChanged;
        }

        private void StationInfo_propertyChanged()
        {
            UpdataProperty();
        }

        [JsonIgnore]
        [DoNotNotify]
        public bool _loadComplete { get; set; }
        public StationInfo Odd_Data { get; set; } = new StationInfo();

        public StationInfo Even_Data { get; set; } = new StationInfo();

        /// <summary>
        /// 工位的MAP数据
        /// </summary>
        public List<WaferMapStation> WaferMap { get; set; } = new List<WaferMapStation>();

        /// <summary>
        /// 清洗后的MAP数据
        /// </summary>
        public List<WaferMapStation> ClearWaferMap { get; set; } = new List<WaferMapStation>();

        /// <summary>
        /// 清洗配方
        /// </summary>
        public string RecipeName
        {
            get
            {
                string a = string.Empty;
                if (this.Odd_Data.IsWafer)
                {
                    a = this.Odd_Data.RecipeName;
                }
                if (this.Even_Data.IsWafer)
                {
                    a = this.Even_Data.RecipeName;
                }
                return a;
            }
        }

        /// <summary>
        /// 工位的晶圆总数
        /// </summary>
        public int? WaferCount { get; set; }

        public string StationName { get; set; }

        public OddEven PutType { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public StationID StationID { get; set; }

        /// <summary>
        /// 检知Senser
        /// </summary>
        public bool PlaceSenser { get; set; }

        public bool IsMapResult { get; set; }

        /// <summary>
        /// 是否MAP完成
        /// </summary>
        public bool IsMapComplete { get; set; }

        public bool Rotate_0_Degree { get; set; }

        public bool Rotate_180_Degree { get; set; }

        public bool Dirty_Comb { get; set; }

        public bool Clean_Comb { get; set; }

        public void UpdataProperty()
        {
            if (_loadComplete)
            {
                File.WriteAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        this.StationName + ".json"
                    ),
                    JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented)
                );
            }
        }

        public void OnPlaceSenserChanged()
        {
            if (!PlaceSenser)
            {
                //Odd_Data.IsWafer = false;
                //Even_Data.IsWafer = false;
                IsMapComplete = false;
            }
        }
    }

    public static class CopyState
    {
        public static void CopyStationInfo(this StationInfo tigger, StationInfo source)
        {
            tigger.Batchid.Batchid = source.Batchid.Batchid;
            tigger.Batchid.BatchState = source.Batchid.BatchState;
            tigger.Batchid.RecipeName = source.Batchid.RecipeName;
            tigger.Batchid.Priority = source.Batchid.Priority;
            //tigger.IsNeedPutWafer = source.IsNeedPutWafer;
            tigger.ProcessState = source.ProcessState;
            //tigger.IsNullFoup = source.IsNullFoup;
            tigger.RFID = source.RFID;
            tigger.RecipeName = source.RecipeName;
            tigger.WaferMap =
                source.WaferMap != null
                    ? JsonConvert.DeserializeObject<List<WaferMapStation>>(
                        JsonConvert.SerializeObject(source.WaferMap)
                    )
                    : null;
            tigger.ClearWaferMap =
                source.WaferMap != null
                    ? JsonConvert.DeserializeObject<List<WaferMapStation>>(
                        JsonConvert.SerializeObject(source.ClearWaferMap)
                    )
                    : null;
            tigger.WaferCount = source.WaferCount;

            //tigger.DirtyClean = source.DirtyClean;
            tigger.Form_LP = source.Form_LP;
            tigger.StorageID = source.StorageID;
            tigger.OddEven = source.OddEven;
            tigger.DoubleProcess = source.DoubleProcess;
            tigger.MAPError = source.MAPError;
            tigger.IsWafer = source.IsWafer;
        }
    }
}
