using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZC_Control_EFAM.StatusManagement;

namespace ZC_Control_EFAM.ProcessControl
{
    public partial class ProcessControl
    {
        private DeviceStateMachine deviceStateMachine;
        private OpenerStateMachine openerStateMachine_1;
        private OpenerStateMachine openerStateMachine_2;
        private HVStateMachine _hVState;
        private PusherStateMachine _pusherState;
        private WHRStateMachine _wHRState;
        public ZC_EFAM_Data eFAM_Data = new ZC_EFAM_Data("192.168.250.180", 5000);

        public event Action<PusherStationState> WTRGetCompleteEvent;
        public event Action<PusherStationState> WTRPutCompleteEvent;
        public event Action<string, PusherStationState> WTSDownRecipe;
        public bool pusherActionIsenable;
        public bool PusherActionIsenable
        {
            get { return pusherActionIsenable; }
            set
            {
                pusherActionIsenable = value;
            }
        }
        public event Action<string, int> ReadRFIDComplete;

        public event Action<string, List<WaferMapStation>, int> PutStorageComplete;

        public event Action<string, string, string> DoublePJComplete;
        public event Action<string, string> SinglePJComplete;
        public Action<string> CJComplete;
        public Action<string> ProcessStartEvent; //20251015
        private bool[] lps = new bool[4];

        private bool auto;

        public bool Auto
        {
            get { return auto; }
            set
            {
                auto = value;
                if (!value)
                {
                    deviceStateMachine.CurrentState = DeviceState.Idle;
                    openerStateMachine_1.CurrentState = OpenerState.Idle;
                    openerStateMachine_2.CurrentState = OpenerState.Idle;
                    _pusherState.CurrentState = PusherState.Idle;
                    _wHRState.CurrentState = WHRState.Idle;
                }
            }
        }

        private bool _wtrPutRequest;
        public bool WTRPutRequest
        {
            get { return _wtrPutRequest; }
            set
            {
                _wtrPutRequest = value;

                if (!value)
                {
                    if (_pusherState.CurrentState == PusherState.WaitWTRPUT)
                    {
                        _pusherState.CurrentState = PusherState.Idle;
                    }
                }
            }
        }

        public bool WillPut { get; set; }

        public bool WTRAllowPut
        {
            get
            {
                return !eFAM_Data.Pusher_Data.PlaceSenser
                    && !eFAM_Data.HV_Data.PlaceSenser
                    && !eFAM_Data.WHR_Data.PlaceSenser
                    && (
                        !Opener1PlaceSenser
                        || eFAM_Data.Opener_Data[0].StationInfo.ProcessState
                            != ProcessState.WaitProcess
                    )
                    && (
                        !Opener2PlaceSenser
                        || eFAM_Data.Opener_Data[1].StationInfo.ProcessState
                            != ProcessState.WaitProcess
                    )
                    && (
                        !FTRPlaceSenser
                        || eFAM_Data.FTR_Data.StationInfo.ProcessState != ProcessState.WaitProcess
                    )
                    && deviceStateMachine.CurrentState != DeviceState.FTR_Get_Storage_Dirty;
            }
        }

        private bool wTRPutComplete;

        public bool WTRPutComplete
        {
            get { return wTRPutComplete; }
            set
            {
                wTRPutComplete = value;
                if (value)
                {
                    wtrPutComplete();
                }
            }
        }

        private bool wTRGetComplete;

        public bool WTRGetComplete
        {
            get { return wTRGetComplete; }
            set
            {
                wTRGetComplete = value;
                if (value)
                {
                    wtrGetComplete();
                }
            }
        }

        #region 各站点在位信号的属性
        public bool BufferPlaceSenser => eFAM_Data.BufferPlaceSenser;

        public bool Lp1PlaceSenser => eFAM_Data.Loadport_Data[0].PlaceSenser;
        public bool Lp2PlaceSenser => eFAM_Data.Loadport_Data[1].PlaceSenser;
        public bool Lp3PlaceSenser => eFAM_Data.Loadport_Data[2].PlaceSenser;
        public bool Lp4PlaceSenser => eFAM_Data.Loadport_Data[3].PlaceSenser;

        public bool Opener1PlaceSenser => eFAM_Data.Opener_Data[0].PlaceSenser;
        public bool Opener2PlaceSenser => eFAM_Data.Opener_Data[1].PlaceSenser;

        public bool FTRPlaceSenser => eFAM_Data.FTR_Data.PlaceSenser;

        public bool WHRPaceSenser => eFAM_Data.WHR_Data.PlaceSenser;

        public bool HVPlaceSenser => eFAM_Data.HV_Data.PlaceSenser;

        public bool PusherPlaceSenser => eFAM_Data.Pusher_Data.PlaceSenser;

        public bool[] StoragePlaceSenser
        {
            get
            {
                var a = new List<bool>();

                foreach (var item in eFAM_Data.Storage_Data)
                {
                    a.Add(item.PlaceSenser);
                }

                return a.ToArray();
            }
        }
        #endregion


        public void CarrierIDVerifySuccess(int LPNO)
        {
            eFAM_Data.Loadport_Data[LPNO - 1].VerifyStatus = true;
        }

        public void MapVerifySuccess(int StorageNO)
        {
            eFAM_Data.Storage_Data[StorageNO - 1].VerifyStatus = true;
        }

        public void VerifySuccess(
            string jobID,
            string recipeName,
            string carrierID1,
            string carrierID2 = ""
        )
        {
            this.CarrierProcessQueue.BatchidCollection.Add(
                new CarrierProcessQueueModel()
                {
                    Batchid = jobID,
                    BatchState = BatchState.Process,
                    Priority = 0,
                    RecipeName = recipeName
                }
            );
            this.CarrierProcessQueue.OnPropertyChanged(string.Empty);

            int storageID1 = 0;
            int storageID2 = 0;

            eFAM_Data
                .Storage_Data.Where(c => c.StationInfo.RFID.Trim() == carrierID1)
                .ToList()
                .ForEach(c => storageID1 = (int)c.FTRStationID - 5);
            if (string.IsNullOrEmpty(carrierID2))
            {
                storageID2 = 0;
            }
            else
            {
                eFAM_Data
                    .Storage_Data.Where(c => c.StationInfo.RFID.Trim() == carrierID2)
                    .ToList()
                    .ForEach(c => storageID2 = (int)c.FTRStationID - 5);
            }

            this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.RecipeName = recipeName;
            this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.Batchid.Batchid = jobID;
            this.eFAM_Data.Storage_Data[storageID1].StationInfo.Batchid.BatchState =
                BatchState.Process;
            this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.Batchid.Priority = 0;
            this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.Batchid.RecipeName = recipeName;
            this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.OddEven = OddEven.Odd;
            this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.ProcessState =
                ProcessState.WaitProcess;

            if (storageID2 > 0)
            {
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.RecipeName = recipeName;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.Batchid.Batchid = jobID;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.Batchid.BatchState =
                    BatchState.Process;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.Batchid.Priority = 0;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.Batchid.RecipeName =
                    recipeName;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.OddEven = OddEven.Odd;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.ProcessState =
                    ProcessState.WaitProcess;
                this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.DoubleProcess = true;
                this.eFAM_Data.Storage_Data[storageID2 - 1].StationInfo.DoubleProcess = true;
            }
            else
            {
                this.eFAM_Data.Storage_Data[storageID1 - 1].StationInfo.DoubleProcess = false;
            }
        }

        /// <summary>
        /// 设置Storage出库的载具号
        /// </summary>
        /// <param name="StorageNO">1-18</param>
        /// <param name="LPNO">1-4</param>
        public void SetStorageOutLP(string carrierID, int lPNO)
        {
            int storageID1 = 0;
            int storageID2 = 0;
            string s = eFAM_Data.Storage_Data[0].StationInfo.RFID;
            eFAM_Data
                
                .Storage_Data.FindAll(src=>!string.IsNullOrEmpty(src.StationInfo.RFID))
                .Where(c => c.StationInfo.RFID.Trim() == carrierID && c.PlaceSenser)
                .ToList()
                .ForEach(c => storageID1 = (int)c.FTRStationID - 5);
            if (storageID1 > 0)
                eFAM_Data.Storage_Data[storageID1 - 1].Out_LP = (StationID)(lPNO + 19);
        }

        public bool PusherRequestGet => _pusherState.CurrentState == PusherState.WaitWTRGet;

        public bool PusherRequestPut => _pusherState.CurrentState == PusherState.WaitWTRPUT;

        public CarrierProcessQueue CarrierProcessQueue = new CarrierProcessQueue();
        public List<StationStateBase> CarrierOutQueue = new List<StationStateBase>();

        public ProcessControl()
        {
            deviceStateMachine = new DeviceStateMachine();
            openerStateMachine_1 = new OpenerStateMachine();
            openerStateMachine_2 = new OpenerStateMachine();
            _hVState = new HVStateMachine();
            _pusherState = new PusherStateMachine();
            _wHRState = new WHRStateMachine();

            CarrierProcessQueue = JsonConvert.DeserializeObject<CarrierProcessQueue>(
                File.ReadAllText(
                    System.IO.Path.Combine(
                        Environment.CurrentDirectory,
                        "Status",
                        "CarrierProcessQueue.json"
                    )
                )
            );
            CarrierOutQueue = new List<StationStateBase>();

            CarrierProcessQueue.CurrentBatchid = new CarrierProcessQueueModel();
            WHRStatusFunc();
            FTRStatusFunc();
            PusherStatusFunc();
            OpenerStatusFunc();
            //Run();
            Run();
        }

        private void wtrPutComplete()
        {
            if (_pusherState.CurrentState == PusherState.WaitWTRPUT)
            {
                _pusherState.CurrentState = PusherState.Idle;

                WTRPutCompleteEvent?.Invoke(eFAM_Data.Pusher_Data);
                eFAM_Data.Pusher_Data.IsMapComplete = false;
                eFAM_Data.Pusher_Data.Odd_Data.ProcessState = ProcessState.Processed;
                eFAM_Data.Pusher_Data.Even_Data.ProcessState = ProcessState.Processed;
                eFAM_Data.Pusher_Data.UpdataProperty();
            }
        }

        private void wtrGetComplete()
        {
            if (_pusherState.CurrentState == PusherState.WaitWTRGet)
            {
                _pusherState.CurrentState = PusherState.Idle;
                WTRGetCompleteEvent?.Invoke(eFAM_Data.Pusher_Data);
                eFAM_Data.Pusher_Data.Odd_Data.IsWafer = false;
                eFAM_Data.Pusher_Data.Even_Data.IsWafer = false;
            }
        }

        private bool GetHVState
        {
            get
            {
                if (
                    _wHRState.CurrentState == WHRState.Idle
                    && _pusherState.CurrentState == PusherState.Idle
                )
                {
                    _hVState.CurrentState = HVState.Idle;
                }
                return _hVState.CurrentState == HVState.Idle;
            }
        }

        public void FTR_Status()
        {
            deviceStateMachine.TriggerStateAction(deviceStateMachine.CurrentState);
        }

        public void WHR_Status()
        {
            _wHRState.TriggerStateAction(_wHRState.CurrentState);
        }

        public void Pusher_Status()
        {
            _pusherState.TriggerStateAction(_pusherState.CurrentState);
        }

        public void Opener1_Status()
        {
            openerStateMachine_1.TriggerStateAction(openerStateMachine_1.CurrentState);
        }

        public void Opener2_Status()
        {
            openerStateMachine_2.TriggerStateAction(openerStateMachine_2.CurrentState);
        }

        private bool getStatusDone;

        public async void GetStationStatus()
        {
            if (eFAM_Data.IsConnected)
            {
                await eFAM_Data.Sys_Get_LPStatus();
                await eFAM_Data.GetStatusCommand(StationID.HV);
                await eFAM_Data.GetStatusCommand(StationID.Pusher);
                await eFAM_Data.GetStatusCommand(StationID.Opener_1);
                await eFAM_Data.GetStatusCommand(StationID.Opener_2);
                await eFAM_Data.GetStatusCommand(StationID.WHR);
                await eFAM_Data.GetStatusCommand(StationID.FTR);

                await eFAM_Data.Sys_Get_AllStorage_Status();

                if (!eFAM_Data.HV_Data.PlaceSenser)
                {
                    eFAM_Data.HV_Data.StationInfo.IsWafer = false;
                }

                if (!eFAM_Data.WHR_Data.PlaceSenser)
                {
                    eFAM_Data.WHR_Data.StationInfo.IsWafer = false;
                }
                if (!eFAM_Data.Pusher_Data.PlaceSenser)
                {
                    eFAM_Data.Pusher_Data.Odd_Data.IsWafer = false;
                    eFAM_Data.Pusher_Data.Even_Data.IsWafer = false;
                }

                getStatusDone = true;
            }
        }
        
        private void Run()
        {
            
            Task.Run(() =>
            {
                while (true)
                {
                    if (Auto)
                    {
                        FTR_Status();
                    }

                    Thread.Sleep(50); // ，
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    if (Auto)
                    {
                        WHR_Status();
                    }

                    Thread.Sleep(50); // ，
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    if (Auto)
                    {
                        Pusher_Status();
                    }
                    Thread.Sleep(50); // ，
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    if (Auto)
                    {
                        Opener1_Status();
                    }
                    Thread.Sleep(50); // ，
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    if (Auto)
                    {
                        Opener2_Status();
                    }
                    Thread.Sleep(50); // ，
                }
            });

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        if (Auto)
            //        {
            //            foreach (
            //                var item in eFAM_Data.Loadport_Data.Where(c =>
            //                    c.StationInfo.ProcessState == ProcessState.UnProcess
            //                    && c.PlaceSenser
            //                    && c.PreDatetime.AddSeconds(5) < DateTime.Now
            //                )
            //            )
            //            {
            //                switch (item.StationName)
            //                {
            //                    case "LP1":
            //                        if (!lps[0])
            //                        {
            //                            var aaa = eFAM_Data.ReadRFIDCommand(1).Result;
            //                            if (aaa.Result)
            //                            {
            //                                item.StationInfo.RFID = aaa.AdditionalData;
            //                                ReadRFIDComplete?.Invoke(item.StationInfo.RFID, 1);
            //                            }
            //                        }
            //                        break;
            //                    case "LP2":
            //                        if (!lps[1])
            //                        {
            //                            var aaa = eFAM_Data.ReadRFIDCommand(2).Result;
            //                            if (aaa.Result)
            //                            {
            //                                item.StationInfo.RFID = aaa.AdditionalData;
            //                                ReadRFIDComplete?.Invoke(item.StationInfo.RFID, 2);
            //                            }
            //                        }
            //                        break;
            //                    case "LP3":
            //                        if (!lps[2])
            //                        {
            //                            var aaa = eFAM_Data.ReadRFIDCommand(3).Result;
            //                            if (aaa.Result)
            //                            {
            //                                item.StationInfo.RFID = aaa.AdditionalData;
            //                                ReadRFIDComplete?.Invoke(item.StationInfo.RFID, 3);
            //                            }
            //                        }
            //                        break;
            //                    case "LP4":
            //                        if (!lps[3])
            //                        {
            //                            var aaa = eFAM_Data.ReadRFIDCommand(4).Result;
            //                            if (aaa.Result)
            //                            {
            //                                item.StationInfo.RFID = aaa.AdditionalData;
            //                                ReadRFIDComplete?.Invoke(item.StationInfo.RFID, 4);
            //                            }
            //                        }
            //                        break;
            //                    default:
            //                        break;
            //                }
            //            }
            //        }

            //        lps[0] = eFAM_Data.Loadport_Data[0].PlaceSenser;
            //        lps[1] = eFAM_Data.Loadport_Data[1].PlaceSenser;
            //        lps[2] = eFAM_Data.Loadport_Data[2].PlaceSenser;
            //        lps[3] = eFAM_Data.Loadport_Data[3].PlaceSenser;
            //        Thread.Sleep(200); //
            //    }
            //});
        }
    }
}
