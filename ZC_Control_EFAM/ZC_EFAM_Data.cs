using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZC_Control_EFAM.Commands;
using ZC_Control_EFAM.StatusManagement;

namespace ZC_Control_EFAM
{
    public partial class ZC_EFAM_Data
    {
        private TcpHexClient tcpHexClient { get; }

        private List<MegModel> SendQueue;
        private ConcurrentDictionary<Guid, MegModel> ReceivedQueue;
        private ConcurrentDictionary<Guid, MegModel> ReceivedQueue_1;
        private BlockingCollection<MegModel> SystemCommQueue;
        private List<MegModel> EventQueue;
        private DefineCommands _defineCommands;
        public List<HEX_EN> List_hex;
        public event Action<MegModel> EventTrigger;
        public AutoResetEvent autoEvent = new AutoResetEvent(false);
        public AutoResetEvent autoEvent1 = new AutoResetEvent(false);
        public event Action<string> DataReceived;
        public CTCTraceDataClass CTCTraceData { get; set; }
        public WFRStationState FTR_Data = new WFRStationState()
        {
            StationName = "FTR",
            StationID = StationID.FTR
        };

        public bool BufferPlaceSenser { get; set; }



        public List<LoadportState> Loadport_Data = new List<LoadportState>()
        {
            new LoadportState()
            {
                StationName = "LP1",
                StationInfo = new StationInfo() { Form_LP = StationID.LP_1, },
                StationID = StationID.Control_Door_1,
                FTRStationID = FTRStationID.LP_1,
            },
            new LoadportState()
            {
                StationName = "LP2",
                StationInfo = new StationInfo() { Form_LP = StationID.LP_2, },
                StationID = StationID.Control_Door_2,
                FTRStationID = FTRStationID.LP_2
            },
            //new LoadportState()
            //{
            //    StationName = "LP3",
            //    StationInfo = new StationInfo() { Form_LP = StationID.LP_3, },
            //    StationID = StationID.Control_Door_3,
            //    FTRStationID = FTRStationID.LP_3
            //},
            //new LoadportState()
            //{
            //    StationName = "LP4",
            //    StationInfo = new StationInfo() { Form_LP = StationID.LP_4, },
            //    StationID = StationID.Control_Door_4,
            //    FTRStationID = FTRStationID.LP_4
            //},
        };

        public List<StorageStation> Storage_Data = new List<StorageStation>()
        {
            new StorageStation()
            {
                StationName = "Storage_1",
                StationID = StationID.Storage_1,
                FTRStationID = FTRStationID.Storage_1
            },
            new StorageStation()
            {
                StationName = "Storage_2",
                StationID = StationID.Storage_2,
                FTRStationID = FTRStationID.Storage_2
            },
            new StorageStation()
            {
                StationName = "Storage_3",
                StationID = StationID.Storage_3,
                FTRStationID = FTRStationID.Storage_3
            },
            new StorageStation()
            {
                StationName = "Storage_4",
                StationID = StationID.Storage_4,
                FTRStationID = FTRStationID.Storage_4
            },
            new StorageStation()
            {
                StationName = "Storage_5",
                StationID = StationID.Storage_5,
                FTRStationID = FTRStationID.Storage_5
            },
            new StorageStation()
            {
                StationName = "Storage_6",
                StationID = StationID.Storage_6,
                FTRStationID = FTRStationID.Storage_6
            },
            new StorageStation()
            {
                StationName = "Storage_7",
                StationID = StationID.Storage_7,
                FTRStationID = FTRStationID.Storage_7
            },
            new StorageStation()
            {
                StationName = "Storage_8",
                StationID = StationID.Storage_8,
                FTRStationID = FTRStationID.Storage_8
            },
            new StorageStation()
            {
                StationName = "Storage_9",
                StationID = StationID.Storage_9,
                FTRStationID = FTRStationID.Storage_9
            },
            new StorageStation()
            {
                StationName = "Storage_10",
                StationID = StationID.Storage_10,
                FTRStationID = FTRStationID.Storage_10
            },
            new StorageStation()
            {
                StationName = "Storage_11",
                StationID = StationID.Storage_11,
                FTRStationID = FTRStationID.Storage_11
            },
            new StorageStation()
            {
                StationName = "Storage_12",
                StationID = StationID.Storage_12,
                FTRStationID = FTRStationID.Storage_12
            },
            new StorageStation()
            {
                StationName = "Storage_13",
                StationID = StationID.Storage_13,
                FTRStationID = FTRStationID.Storage_13
            },
            new StorageStation()
            {
                StationName = "Storage_14",
                StationID = StationID.Storage_14,
                FTRStationID = FTRStationID.Storage_14
            },
            new StorageStation()
            {
                StationName = "Storage_15",
                StationID = StationID.Storage_15,
                FTRStationID = FTRStationID.Storage_15
            },
            new StorageStation()
            {
                StationName = "Storage_16",
                StationID = StationID.Storage_16,
                FTRStationID = FTRStationID.Storage_16
            },
            new StorageStation()
            {
                StationName = "Storage_17",
                StationID = StationID.Storage_17,
                FTRStationID = FTRStationID.Storage_17
            },
            new StorageStation()
            {
                StationName = "Storage_18",
                StationID = StationID.Storage_18,
                FTRStationID = FTRStationID.Storage_18
            },
        };

        public List<OpenerStationState> Opener_Data = new List<OpenerStationState>()
        {
            new OpenerStationState()
            {
                StationName = "Opener_1",
                StationID = StationID.Opener_1,
                FTRStationID = FTRStationID.Opener_1
            },
            new OpenerStationState()
            {
                StationName = "Opener_2",
                StationID = StationID.Opener_2,
                FTRStationID = FTRStationID.Opener_2
            }
        };

        public HVStationState HV_Data = new HVStationState()
        {
            StationName = "HV",
            StationID = StationID.HV
        };

        public WHRStationState WHR_Data = new WHRStationState()
        {
            StationName = "WHR",
            StationID = StationID.WHR
        };

        public PusherStationState Pusher_Data = new PusherStationState()
        {
            StationName = "Pusher",
            StationID = StationID.Pusher
        };

        public void UpdataStationData()
        {
            Opener_Data[0].UpdataProperty();
            Opener_Data[1].UpdataProperty();
            foreach (var item in Loadport_Data)
            {
                item.UpdataProperty();
            }
            foreach (var item in Storage_Data)
            {
                item.UpdataProperty();
            }
            HV_Data.UpdataProperty();
            WHR_Data.UpdataProperty();
            FTR_Data.UpdataProperty();
            Pusher_Data.UpdataProperty();
        }

        private void ReadJsonFile()
        {
            FTR_Data = JsonConvert.DeserializeObject<WFRStationState>(
                File.ReadAllText(
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Status", "FTR.json")
                )
            );

            FTR_Data._loadComplete = true;
            for (int i = 0; i < 2; i++)
            {
                Loadport_Data[i] = JsonConvert.DeserializeObject<LoadportState>(
                    File.ReadAllText(
                        System.IO.Path.Combine(
                            Environment.CurrentDirectory,
                            "Status",
                            $"LP{(i + 1).ToString()}.json"
                        )
                    )
                );
                Loadport_Data[i].PreDatetime = DateTime.Now;

                Loadport_Data[i]._loadComplete = true;
            }

            for (int i = 0; i < 18; i++)
            {
                Storage_Data[i] = JsonConvert.DeserializeObject<StorageStation>(
                    File.ReadAllText(
                        System.IO.Path.Combine(
                            Environment.CurrentDirectory,
                            "Status",
                            $"Storage_{(i + 1).ToString()}.json"
                        )
                    )
                );

                Storage_Data[i]._loadComplete = true;
            }

            Opener_Data[0] = JsonConvert.DeserializeObject<OpenerStationState>(
                File.ReadAllText(
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Status", "Opener_1.json")
                )
            );

            Opener_Data[0]._loadComplete = true;
            Opener_Data[1] = JsonConvert.DeserializeObject<OpenerStationState>(
                File.ReadAllText(
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Status", "Opener_2.json")
                )
            );

            Opener_Data[1]._loadComplete = true;

            HV_Data = JsonConvert.DeserializeObject<HVStationState>(
                File.ReadAllText(
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Status", "HV.json")
                )
            );

            HV_Data._loadComplete = true;
            WHR_Data = JsonConvert.DeserializeObject<WHRStationState>(
                File.ReadAllText(
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Status", "WHR.json")
                )
            );

            WHR_Data._loadComplete = true;
            Pusher_Data = JsonConvert.DeserializeObject<PusherStationState>(
                File.ReadAllText(
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Status", "Pusher.json")
                )
            );

            Pusher_Data._loadComplete = true;
        }

        /// <summary>
        /// 当前的控制模式
        /// </summary>
        public bool ControlMode { get; set; }
        private int waferSpecification;
        public int WaferSpecification
        {
            get { return waferSpecification; }
            set
            {
                if (value < 0 || value > 3) return;
                if (value == waferSpecification)
                {
                    return;
                }
                if (ChangeThickness(value))
                {
                    waferSpecification = value;
                }
                else
                {
                    throw new ArgumentException($"变更厚度失败");
                }

            }
        }


        public bool IsConnected => tcpHexClient.IsConnected;

        public ZC_EFAM_Data(string ip, int port)
        {
            ReadJsonFile();
            SendQueue = new List<MegModel>();
            ReceivedQueue = new ConcurrentDictionary<Guid, MegModel>();
            ReceivedQueue_1 = new ConcurrentDictionary<Guid, MegModel>();
            EventQueue = new List<MegModel>();
            tcpHexClient = new TcpHexClient(ip, port);
            tcpHexClient.OnHexDataReceived += Client_OnHexDataReceived;
            tcpHexClient.OnConnected += Client_OnConnected;
            tcpHexClient.OnDisconnected += Client_OnDisconnected;
            List_hex = new List<HEX_EN>();
            _defineCommands = new DefineCommands();
            DataTable dt = NPOIHelper.ExcelToTableForXLS("EFEM_A\\commandA.xls");
            foreach (DataRow item in dt.Rows)
            {
                List_hex.Add(
                    new HEX_EN
                    {
                        f_type = item["F_TYPE"].ToString(),
                        f_HEXName = item["F_NAME"].ToString(),
                        f_5t = item["F_BYTE5"].ToString(),
                        f_desc = item["F_DESC"].ToString(),
                        f_6t = item["F_BYTE6"].ToString(),
                        f_7t = item["F_BYTE7"].ToString(),
                        f_8t = item["F_BYTE8"].ToString(),
                        f_9t = item["F_BYTE9"].ToString(),
                        f_10t = item["F_BYTE10"].ToString(),
                        f_11t = item["F_BYTE11"].ToString(),
                        f_full =
                            item["F_BYTE5"].ToString()
                            + item["F_BYTE6"].ToString()
                            + item["F_BYTE7"].ToString()
                            + item["F_BYTE8"].ToString()
                            + item["F_BYTE9"].ToString()
                            + item["F_BYTE10"].ToString()
                            + item["F_BYTE11"].ToString(),
                        f_return = item["F_R"].ToString()
                    }
                );
            }

            Task.Run(() =>
            {
                //ReadWTSStatus();
            });
        }

        #region 通讯断开和连接后的事件
        private void Client_OnDisconnected() { }

        private void Client_OnConnected() { }
        #endregion
        /// <summary>
        /// 切换厚度指令
        /// </summary>
        /// <param name="thikckness">指定厚度 最大3</param>
        /// <returns></returns>
        public bool ChangeThickness(int thikckness)
        {
            try
            {
                if (thikckness > 3)
                {
                    return false;
                }
                if (!tcpHexClient.IsConnected)
                {
                    return false;
                }
                for (int i = 0; i < 4; i++)
                {
                    HEX_EN b = _defineCommands.WaferSpecificationSwitchCommand(i, thikckness);
                    long a = tcpHexClient.SendHexPayload(b.f_full);
                    
                    if (ChackReveState(a).GetAwaiter().GetResult().Result == false)
                    {
                        return false;
                    } 
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        /// <summary>
        /// 接收到报文，根据报文进行分发处理
        /// </summary>
        /// <param name="Len">报文标识符</param>
        /// <param name="list">报文字节列表</param>
        private void Client_OnHexDataReceived(long Len, List<byte> list)
        {
            switch (list[5])
            {
                case 0:
                    if (list[6] != 0)
                    {
                        ReceivedQueue.TryAdd(
                            Guid.NewGuid(),
                            new MegModel()
                            {
                                UUID = Len,

                                Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                                MesType = "SubSystem",
                                Result = false,
                                ErrorCode = BitConverter
                                    .ToString(list.Skip(7).Take(4).ToArray())
                                    .Replace("-", ""),
                            }
                        );
                    }

                    break;
                case 1:
                    if (list[6] == 0x80)
                    {
                        ReceivedQueue.TryAdd(
                            Guid.NewGuid(),
                            new MegModel()
                            {
                                UUID = Len,
                                Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                                MesType = "SubSystem",
                                Result = false,
                                ErrorCode = BitConverter
                                    .ToString(list.Skip(7).Take(2).ToArray())
                                    .Replace("-", ""),
                            }
                        );
                    }
                    else
                    {
                        SubCommDataParser(Len, list);
                    }

                    break;
                case 2:
                    if (list[6] != 0)
                    {
                        ReceivedQueue.TryAdd(
                            Guid.NewGuid(),
                            new MegModel()
                            {
                                UUID = Len,
                                Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                                MesType = "DefinedCommand",
                                Result = false,
                                ErrorCode = BitConverter
                                    .ToString(list.Skip(7).Take(2).ToArray())
                                    .Replace("-", ""),
                            }
                        );
                        
                    }                   
                    else
                    {
                        ReceivedQueue.TryAdd(
                               Guid.NewGuid(),
                               new MegModel()
                               {
                                   Result = true,
                                   UUID = Len,
                                   Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                                   MesType = "DefinedCommand"
                               });
                    }
                    ;
                      
                    break;
                case 3:
                    SystemDataParser(Len, list);
                    break;
                case 4:
                    //EventQueue.Add(
                    //    new MegModel()
                    //    {
                    //        UUID = Len,
                    //        Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                    //        EventID = list[7],
                    //        MesType = "Event"
                    //    }
                    //);
                    EventDataParser(Len, list);
                    EventTrigger?.Invoke(
                        new MegModel()
                        {
                            UUID = Len,
                            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
                            EventID = list[7],
                            MesType = "Event"
                        }
                    );
                    break;
                default:
                    break;
            }

            //else
            //    EventQueue.Add(
            //        new MegModel()
            //        {
            //            UUID = Len,
            //            Message = BitConverter.ToString(list.ToArray()).Replace("-", " "),
            //            EventID = list[6]
            //        }
            //    );
            DataReceived?.Invoke(BitConverter.ToString(list.ToArray()).Replace("-", " "));
        }

        /// <summary>
        /// 通讯连接建立
        /// </summary>
        /// <returns></returns>
        public bool Connent() => tcpHexClient.Connect();

        /// <summary>
        /// 通讯连接建立
        /// </summary>
        /// <returns></returns>
        public void Disconnect() => tcpHexClient.Disconnect();

        /// <summary>
        /// 从字节中抽取Bit位
        /// </summary>
        /// <param name="b">字节数据</param>
        /// <param name="bitPosition">1-8的Bit位</param>
        /// <returns>抽取Bit位的值</returns>
        /// 位运算原理：
        //1 << bitPosition：创建一个掩码(mask)，将1左移到指定位置
        //例如bitPosition = 3时，得到00001000(二进制)
        //b & mask：按位与运算，只保留b中与mask对应位相同的位
        //其他位都会被置为0
        //!= 0：检查结果是否为0，如果不是0，说明该位是1
        public static bool GetBit(byte b, int bitPosition) => (b & (1 << (bitPosition - 1))) != 0;

        // RemoveAll 方法内部实现
        public void ReceivedQueueRemoveAll(Predicate<MegModel> match)
        {
            foreach (var kvp in ReceivedQueue)
            {
                if (match(kvp.Value)) // 检查当前元素是否满足删除条件
                    ReceivedQueue.TryRemove(kvp.Key, out _); // 如果满足，则删除
            }
        }

        public void ReceivedQueue_1RemoveAll(Predicate<MegModel> match)
        {
            foreach (var kvp in ReceivedQueue_1)
            {
                if (match(kvp.Value)) // 检查当前元素是否满足删除条件
                    ReceivedQueue_1.TryRemove(kvp.Key, out _); // 如果满足，则删除
            }
        }

        private async Task<MegModel> ChackReveState(long number)
        {
            MegModel megModel = new MegModel() { Result = false };
            return await Task.Run(async () =>
            {
                await Task.Delay(10);
                var sw = Stopwatch.StartNew();
                while (true)
                {
                    //var ss = ReceivedQueue.Find(d => d != null && d.UUID == number);
                    var ss = ReceivedQueue.FirstOrDefault(d => d.Value.UUID == number);

                    if (ss.Value != null)
                    {
                        string reciveMessage = $"信息ID: {ss.Value.UUID} 工位号：{ss.Value.Station} 接收状态:{ss.Value.State} 接收信息:{ss.Value.Message} 错误代码:{ss.Value.ErrorCode}"; //20251011
                        LogU.LogInfo("Recive:   " + reciveMessage);
                        ReceivedQueueRemoveAll(c => c.UUID == number);
                        return ss.Value;
                    }
                    var ss1 = ReceivedQueue_1.FirstOrDefault(d => d.Value.UUID == number);
                    if (ss1.Value != null)
                    {
                        ReceivedQueue_1RemoveAll(c => c.UUID == number);
                        //megModel = ss1;
                        return ss1.Value;
                    }
                    if (sw.Elapsed >= TimeSpan.FromMilliseconds(60000))
                    {
                        megModel.ErrorCode = $"接收超时！{sw.Elapsed.ToString()}";
                        return megModel;
                    }
                    await Task.Delay(100);
                }
            });
        }
    }
}
