using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZC_Control_EFAM.StatusManagement;

namespace ZC_Control_EFAM.ProcessControl
{
    public partial class ProcessControl
    {
        private void FTRStatusFunc()
        {
            #region FTR
            //Idle
            deviceStateMachine.RegisterExecutor(
                DeviceState.Idle,
                () =>
                {
                    return FTRIdleFunc();
                }
            );

            //Read_RFID
            deviceStateMachine.RegisterExecutor(
                DeviceState.Read_RFID,
                () =>
                {
                    return Read_RFID();
                }
            );

            //LPDoor_Open_And_ToLPReady_Get
            deviceStateMachine.RegisterExecutor(
                DeviceState.LPDoor_Open_And_ToLPReady_Get,
                () =>
                {
                    return LPDoor_Open_And_ToLPReady_Get();
                }
            );

            //LPDoor_Open_And_ToLPReady_Put
            deviceStateMachine.RegisterExecutor(
                DeviceState.LPDoor_Open_And_ToLPReady_Put,
                () =>
                {
                    return LPDoor_Open_And_ToLPReady_Put();
                }
            );

            //FTR_Get_LP
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Get_LP,
                () =>
                {
                    return FTR_Get_LP();
                }
            );

            //FTR_Put_LP
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Put_LP,
                () =>
                {
                    return FTR_Put_LP();
                }
            );

            //FTR_Get_Storage_Dirty
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Get_Storage_Dirty,
                () =>
                {
                    return FTR_Get_Storage_Dirty();
                }
            );

            //FTR_Get_Storage_Clean
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Get_Storage_Clean,
                () =>
                {
                    return FTR_Get_Storage_Clean();
                }
            );

            //FTR_Put_Storage
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Put_Storage,
                () =>
                {
                    return FTR_Put_Storage();
                }
            );

            //FTR_Get_Opener
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Get_Opener,
                () =>
                {
                    return FTR_Get_Opener();
                }
            );

            //FTR_Put_Opener
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Put_Opener,
                () =>
                {
                    return FTR_Put_Opener();
                }
            );

            //LPDoor_Close
            deviceStateMachine.RegisterExecutor(
                DeviceState.LPDoor_Close,
                () =>
                {
                    return LPDoor_Close();
                }
            );

            //FTR_Put_Opener_And_LPDoor_Close
            deviceStateMachine.RegisterExecutor(
                DeviceState.FTR_Put_Opener_And_LPDoor_Close,
                () =>
                {
                    return FTR_Put_Opener_And_LPDoor_Close();
                }
            );

            #endregion
        }

        private DeviceEvent FTRIdleFunc()
        {
            for (int i = 0; i < 2; i++) //20251015 FTR处于IDLE 状态的时候  所有 传递锁定解除
            {
                eFAM_Data.Loadport_Data[i].FTRTransferState = false;
            }
           
            if (
                !eFAM_Data.ControlMode
                || !eFAM_Data.IsConnected
                || eFAM_Data.FTR_Data.PlaceSenser
                || !getStatusDone
            )
                return DeviceEvent.None;

            bool opener = eFAM_Data.Opener_Data.Where(c => !c.PlaceSenser).Count() > 0;

            //只要任意一个有货，并且不是清洗完成状态，就不允许进料
            bool opener1 =
                eFAM_Data
                    .Opener_Data.Where(c =>
                        c.PlaceSenser
                        && (
                            c.StationInfo.ProcessState == ProcessState.Processing
                            || c.StationInfo.ProcessState == ProcessState.WaitProcess
                        )
                    )
                    .Count() > 0;

            #region 制程判断
            if (
                !WillPut
                && !WTRPutRequest
                && _wHRState.CurrentState == WHRState.Idle
                && _pusherState.CurrentState == PusherState.Idle
                && _hVState.CurrentState == HVState.Idle
                && !eFAM_Data.WHR_Data.PlaceSenser
                && !eFAM_Data.HV_Data.PlaceSenser
                && !eFAM_Data.Pusher_Data.PlaceSenser
                //&& (
                //    !eFAM_Data.Pusher_Data.PlaceSenser
                //    || (
                //        eFAM_Data.Pusher_Data.Odd_Data.IsWafer
                //        && eFAM_Data.Pusher_Data.Odd_Data.ProcessState == ProcessState.Processing
                //    )
                //    || (
                //        eFAM_Data.Pusher_Data.Even_Data.IsWafer
                //        && eFAM_Data.Pusher_Data.Even_Data.ProcessState == ProcessState.Processing
                //    )
                //)
                && CarrierProcessQueue.CurrentBatchid.Batchid == string.Empty
                && CarrierProcessQueue.BatchidCollection.Count > 0
                && CarrierProcessQueue.BatchidCollection[0].BatchState == BatchState.Process
                && opener
                && !opener1
            )
            {
                foreach (
                    var item in eFAM_Data.Storage_Data.Where(c =>
                        c.StationInfo.Batchid.Batchid
                            == CarrierProcessQueue.BatchidCollection[0].Batchid
                        && c.StationInfo.ProcessState == ProcessState.WaitProcess
                    )
                )
                {
                    eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                    eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;

                    CarrierProcessQueue.CurrentBatchid.Batchid = item.StationInfo.Batchid.Batchid;
                    CarrierProcessQueue.CurrentBatchid.BatchState = item.StationInfo
                        .Batchid
                        .BatchState;
                    CarrierProcessQueue.CurrentBatchid.Priority = item.StationInfo.Batchid.Priority;
                    CarrierProcessQueue.CurrentBatchid.RecipeName = item.StationInfo
                        .Batchid
                        .RecipeName;

                    CarrierProcessQueue.BatchidCollection.Remove(
                        CarrierProcessQueue
                            .BatchidCollection.Where(c =>
                                c.Batchid == item.StationInfo.Batchid.Batchid
                            )
                            .First()
                    );

                    CarrierProcessQueue.OnPropertyChanged(string.Empty);

                    var b = eFAM_Data.Opener_Data.Where(c => !c.PlaceSenser).FirstOrDefault();
                    eFAM_Data.FTR_Data.PutFTRStationID = b.FTRStationID;
                    eFAM_Data.FTR_Data.PutGlobalStationID = b.StationID;
                    return DeviceEvent.To_Get_Storage_Dirty_Status;
                }

                if (
                    eFAM_Data.Storage_Data.Count(c =>
                        c.StationInfo.Batchid.Batchid
                        == CarrierProcessQueue.BatchidCollection[0].Batchid
                    ) == 0
                )
                {
                    CarrierProcessQueue.BatchidCollection.RemoveAt(0);
                }
            }

            if (CarrierProcessQueue.CurrentBatchid.Batchid != string.Empty && opener)
            {
                foreach (
                    var item in eFAM_Data.Storage_Data.Where(c =>
                        c.StationInfo.Batchid.Batchid == CarrierProcessQueue.CurrentBatchid.Batchid
                    )
                )
                {
                    eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                    eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;
                    var b = eFAM_Data.Opener_Data.Where(c => !c.PlaceSenser).FirstOrDefault();
                    eFAM_Data.FTR_Data.PutFTRStationID = b.FTRStationID;
                    eFAM_Data.FTR_Data.PutGlobalStationID = b.StationID;
                    return DeviceEvent.To_Get_Storage_Dirty_Status;
                }
            }
            #endregion

            #region 制程完成后退料把空Foup放置到Opener处理

            //WHR请求空Foup判断
            if (
                opener
                && eFAM_Data.WHR_Data.StationInfo.ProcessState == ProcessState.Processed
                && eFAM_Data.WHR_Data.PlaceSenser
                && eFAM_Data.WHR_Data.StationInfo.IsWafer
            )
            {
                foreach (
                    StationStateBase item in eFAM_Data.Storage_Data.Where(c =>
                        c.StationInfo.ProcessState == ProcessState.Processing
                        && c.PlaceSenser
                        && c.StationInfo.StorageID == eFAM_Data.WHR_Data.StationInfo.StorageID
                    )
                )
                {
                    eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                    eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;

                    var b = eFAM_Data.Opener_Data.Where(c => !c.PlaceSenser).FirstOrDefault();
                    eFAM_Data.FTR_Data.PutFTRStationID = b.FTRStationID;
                    eFAM_Data.FTR_Data.PutGlobalStationID = b.StationID;
                    return DeviceEvent.To_Get_Storage_Dirty_Status;
                }
            }
            //HV请求空Foup判断
            if (
                opener
                && eFAM_Data.HV_Data.StationInfo.ProcessState == ProcessState.Processed
                && eFAM_Data.HV_Data.PlaceSenser
                && eFAM_Data.HV_Data.StationInfo.IsWafer
            )
            {
                foreach (
                    StationStateBase item in eFAM_Data.Storage_Data.Where(c =>
                        c.StationInfo.ProcessState == ProcessState.Processing
                        && c.PlaceSenser
                        && c.StationInfo.StorageID == eFAM_Data.HV_Data.StationInfo.StorageID
                    )
                )
                {
                    eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                    eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;

                    var b = eFAM_Data.Opener_Data.Where(c => !c.PlaceSenser).FirstOrDefault();
                    eFAM_Data.FTR_Data.PutFTRStationID = b.FTRStationID;
                    eFAM_Data.FTR_Data.PutGlobalStationID = b.StationID;
                    return DeviceEvent.To_Get_Storage_Dirty_Status;
                }
            }
            #endregion

            #region Opener取货判断
            foreach (var item in eFAM_Data.Opener_Data.Where(c => c.PlaceSenser && c.IsMapComplete))
            {
                eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;

                eFAM_Data.FTR_Data.PutFTRStationID = (FTRStationID)(item.StationInfo.StorageID + 6);
                return DeviceEvent.To_Get_Opener_Status;
            }
            #endregion
            //下料估计是改这个位置，感觉要加个下料队列
            #region 制程完成===》》》LP  判断
            bool lp = eFAM_Data.Loadport_Data.Where(c => !c.PlaceSenser).Count() > 0;
            //
            foreach (
                StationStateBase item in eFAM_Data.Storage_Data.Where(c =>
                    (c.StationInfo.ProcessState == ProcessState.Processed || c.StationInfo.ProcessState == ProcessState.UnProcess)
                    && c.PlaceSenser
                    && (int)c.Out_LP >= 20
                    && (int)c.Out_LP <= 23
                )
            //var item in CarrierOutQueue

            )
            {
                item.StationInfo.ProcessState = ProcessState.Processed;
                var a = eFAM_Data
                    .Loadport_Data.Where(c => c.StationID == item.Out_LP)
                    .FirstOrDefault();

                if (a != null && !a.PlaceSenser)
                {
                    a.FTRTransferState = true; //20251015 FTR处于IDLE 状态的时候 当判定下料的时候，传递锁定
                    eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                    eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;
                    eFAM_Data.FTR_Data.PutFTRStationID = a.FTRStationID;
                    eFAM_Data.FTR_Data.PutGlobalStationID = a.StationID;

                    return DeviceEvent.To_Get_Storage_Clean_Status;
                }
            }
            #endregion

            #region LP取货判断         
            foreach (
                var item in eFAM_Data.Loadport_Data.Where(c =>
                    c.StationInfo.ProcessState == ProcessState.UnProcess
                    && c.PlaceSenser
                    && c.PreDatetime.AddSeconds(5) < DateTime.Now
                    && c.VerifyStatus
                    
                    )//增加外置验证条件                                    
            )
            {
                for (int i = 0; i < eFAM_Data.Storage_Data.Count; i++)
                {
                    var aaa = (byte)eFAM_Data.Opener_Data[0].StationInfo.StorageID;
                    var bbb = (byte)eFAM_Data.Opener_Data[1].StationInfo.StorageID;

                    var aaa1 = (int)eFAM_Data.Opener_Data[0].StationInfo.StorageID;
                    var bbb1 = (int)eFAM_Data.Opener_Data[1].StationInfo.StorageID;

                    if (
                        !eFAM_Data.Storage_Data[i].PlaceSenser
                        && (
                            (int)eFAM_Data.Opener_Data[0].StationInfo.StorageID != i
                            || !eFAM_Data.Opener_Data[0].PlaceSenser
                        )
                        && (
                            (int)eFAM_Data.Opener_Data[1].StationInfo.StorageID != i
                            || !eFAM_Data.Opener_Data[1].PlaceSenser
                        )
                        && opener
                    )
                    {
                        item.StationInfo.StorageID = (StorageID)i;
                        eFAM_Data.FTR_Data.GetFTRStationID = item.FTRStationID;
                        eFAM_Data.FTR_Data.GetGlobalStationID = item.StationID;
                        var b = eFAM_Data.Opener_Data.Where(c => !c.PlaceSenser).FirstOrDefault();
                        eFAM_Data.FTR_Data.PutFTRStationID = b.FTRStationID;
                        eFAM_Data.FTR_Data.PutGlobalStationID = b.StationID;

                        return DeviceEvent.To_Read_RFID_Status;
                    }
                }
            }
            #endregion

            return DeviceEvent.None;
        }

        private DeviceEvent Read_RFID()
        {
            byte i = 1;
            foreach (var item in eFAM_Data.Loadport_Data)
            {
                if (item.FTRStationID == eFAM_Data.FTR_Data.GetFTRStationID)
                {
                    var aaa = eFAM_Data.ReadRFIDCommand(i).Result;
                    if (aaa.Result)
                    {
                        item.StationInfo.RFID = aaa.AdditionalData;
                        return DeviceEvent.Read_RFID_Complete;
                    }
                    return DeviceEvent.Fail;
                }
                i++;
            }
            return DeviceEvent.Fail;
        }

        private DeviceEvent LPDoor_Open_And_ToLPReady_Get()
        {
            // 启动两个任务
            var task1 = eFAM_Data.LoadportFuncCommand(
                eFAM_Data.FTR_Data.GetGlobalStationID,
                ZC_Control_EFAM.LPFuncType.Open_Door
            );
            var task2 = eFAM_Data.FTRFuncCommand(
                ActionType.Ready,
                eFAM_Data.FTR_Data.GetFTRStationID
            );

            // 等待所有任务完成
            Task.WaitAll(task1, task2);

            // 获取结果（如果有返回值）
            var result1 = task1.Result; // 如果LoadportFuncCommand有返回值
            var result2 = task2.Result; // 如果FTRFuncCommand有返回值

            // 处理结果...
            // 例如记录日志或检查返回值
            if (result1.Result && result2.Result)
            {
                return DeviceEvent.LPDoor_Open_And_ToLPReady_Get_Complete;
            }
            else
            {
                return DeviceEvent.Fail;
            }
        }

        private DeviceEvent LPDoor_Open_And_ToLPReady_Put()
        {
            // 启动两个任务
            var task1 = eFAM_Data.LoadportFuncCommand(
                eFAM_Data.FTR_Data.PutGlobalStationID,
                ZC_Control_EFAM.LPFuncType.Open_Door
            );
            var task2 = eFAM_Data.FTRFuncCommand(
                ActionType.Ready,
                eFAM_Data.FTR_Data.PutFTRStationID
            );

            // 等待所有任务完成
            Task.WaitAll(task1, task2);

            // 获取结果（如果有返回值）
            var result1 = task1.Result; // 如果LoadportFuncCommand有返回值
            var result2 = task2.Result; // 如果FTRFuncCommand有返回值

            // 处理结果...
            // 例如记录日志或检查返回值
            if (result1.Result && result2.Result)
            {
                return DeviceEvent.LPDoor_Open_And_ToLPReady_Put_Complete;
            }
            else
            {
                return DeviceEvent.Fail;
            }
        }

        private DeviceEvent FTR_Get_LP()
        {
            var a = eFAM_Data
                .FTRFuncCommand(ActionType.Get, eFAM_Data.FTR_Data.GetFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Loadport_Data.Find(c =>
                    c.StationID == eFAM_Data.FTR_Data.GetGlobalStationID
                );
                b.StationInfo.MAPError = false;
                b.StationInfo.Batchid = new CarrierProcessQueueModel();
                eFAM_Data.FTR_Data.StationInfo.CopyStationInfo(b.StationInfo);

                b.StationInfo.IsWafer = false;
                eFAM_Data.FTR_Data.StationInfo.Form_LP = b.StationID;
                eFAM_Data.FTR_Data.StationInfo.IsWafer = true;
                return DeviceEvent.FTR_Get_LP_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Put_LP()
        {
            var a = eFAM_Data
                .FTRFuncCommand(ActionType.Put, eFAM_Data.FTR_Data.PutFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Loadport_Data.Find(c =>
                    c.FTRStationID == eFAM_Data.FTR_Data.PutFTRStationID
                );
                b.StationInfo.CopyStationInfo(eFAM_Data.FTR_Data.StationInfo);
                b.StationInfo.MAPError = false;
                eFAM_Data.FTR_Data.StationInfo.IsWafer = false;
                return DeviceEvent.FTR_Put_LP_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Get_Storage_Dirty()
        {
            MegModel a = eFAM_Data
                .FTRFuncCommand(ActionType.Get, eFAM_Data.FTR_Data.GetFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Storage_Data.Find(c =>
                    c.StationID == eFAM_Data.FTR_Data.GetGlobalStationID
                );

                if (b.StationInfo.ProcessState != ProcessState.Processing)
                {
                    var cv = eFAM_Data.Storage_Data.Count(v =>
                        v.StationInfo.Batchid.Batchid == b.StationInfo.Batchid.Batchid
                        && v.StationInfo.ProcessState == ProcessState.WaitProcess
                    );
                    if (cv <= 1)
                    {
                        CarrierProcessQueue.CurrentBatchid = new CarrierProcessQueueModel();
                        CarrierProcessQueue.OnPropertyChanged(string.Empty);
                        //CarrierProcessQueue.BatchidCollection.RemoveAll(z =>
                        //    z.Batchid == b.StationInfo.Batchid
                        //);
                    }
                    //else
                    //{
                    //    CarrierProcessQueue.CurrentBatchid = b.StationInfo.Batchid;
                    //    CarrierProcessQueue.BatchidCollection.RemoveAll(z =>
                    //        z.Batchid == b.StationInfo.Batchid
                    //    );
                    //}
                }
                eFAM_Data.FTR_Data.StationInfo.CopyStationInfo(b.StationInfo);
                b.StationInfo.Batchid = new CarrierProcessQueueModel();
                b.StationInfo.IsWafer = false;
                ProcessStartEvent?.Invoke(eFAM_Data.FTR_Data.StationInfo.Batchid.Batchid);//20251015 ProcessStartEvent
                return DeviceEvent.FTR_Get_Storage_Dirty_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Get_Storage_Clean()
        {
            var a = eFAM_Data
                .FTRFuncCommand(ActionType.Get, eFAM_Data.FTR_Data.GetFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Storage_Data.Find(c =>
                    c.StationID == eFAM_Data.FTR_Data.GetGlobalStationID
                );
                b.StationInfo.Batchid = new CarrierProcessQueueModel();
                eFAM_Data.FTR_Data.StationInfo.CopyStationInfo(b.StationInfo);
                b.StationInfo.IsWafer = false;
                return DeviceEvent.FTR_Get_Storage_Clean_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Put_Storage()
        {
            var a = eFAM_Data
                .FTRFuncCommand(ActionType.Put, eFAM_Data.FTR_Data.PutFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Storage_Data.Find(c =>
                    c.FTRStationID == eFAM_Data.FTR_Data.PutFTRStationID
                );
                //b.StationInfo.Batchid = new CarrierProcessQueueModel();

                b.StationInfo.CopyStationInfo(eFAM_Data.FTR_Data.StationInfo);
                if (b.StationInfo.ProcessState != ProcessState.Processing)
                {
                    b.StationInfo.RecipeName = string.Empty;
                }
                b.UpdataProperty();
                eFAM_Data.FTR_Data.StationInfo.IsWafer = false;
                if (b.StationInfo.ProcessState == ProcessState.UnProcess)
                {
                    PutStorageComplete?.Invoke(
                        b.StationInfo.RFID,
                        b.StationInfo.WaferMap,
                        ((int)b.FTRStationID) - 5
                    );
                }

                if (b.StationInfo.ProcessState == ProcessState.Processed)
                {
                    if (!b.StationInfo.DoubleProcess)
                    {
                        CJComplete?.Invoke(b.StationInfo.Batchid.Batchid);
                        SinglePJComplete?.Invoke(b.StationInfo.Batchid.Batchid, b.StationInfo.RFID);
                    }
                    else
                    {
                        var aa = eFAM_Data.Storage_Data.Count(c =>
                            c.StationInfo.Batchid.Batchid == b.StationInfo.Batchid.Batchid
                            && c.StationInfo.ProcessState == ProcessState.Processed
                        );
                        if (aa == 2)
                        {
                            var bb = eFAM_Data.Storage_Data.FindAll(c =>
                                c.StationInfo.Batchid.Batchid == b.StationInfo.Batchid.Batchid
                                && c.StationInfo.ProcessState == ProcessState.Processed
                            );
                            CJComplete?.Invoke(b.StationInfo.Batchid.Batchid);
                            DoublePJComplete?.Invoke(
                                b.StationInfo.Batchid.Batchid,
                                bb[0].StationInfo.RFID,
                                bb[1].StationInfo.RFID
                            );
                        }
                    }
                }

                return DeviceEvent.FTR_Put_Storage_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Get_Opener()
        {
            var a = eFAM_Data
                .FTRFuncCommand(ActionType.Get, eFAM_Data.FTR_Data.GetFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Opener_Data.Find(c =>
                    c.FTRStationID == eFAM_Data.FTR_Data.GetFTRStationID
                );
                eFAM_Data.FTR_Data.StationInfo.CopyStationInfo(b.StationInfo);
                b.StationInfo.IsWafer = false;
                return DeviceEvent.FTR_Get_Opener_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Put_Opener()
        {
            var a = eFAM_Data
                .FTRFuncCommand(ActionType.Put, eFAM_Data.FTR_Data.PutFTRStationID)
                .Result;
            if (a.Result)
            {
                var b = eFAM_Data.Opener_Data.Find(c =>
                    c.FTRStationID == eFAM_Data.FTR_Data.PutFTRStationID
                );
                b.StationInfo.CopyStationInfo(eFAM_Data.FTR_Data.StationInfo);
                eFAM_Data.FTR_Data.StationInfo.IsWafer = false;
                b.IsMapComplete = false;
                if (eFAM_Data.FTR_Data.PutFTRStationID == FTRStationID.Opener_1)
                {
                    openerStateMachine_1.CurrentState = OpenerState.Opener_Open_Get;
                }
                else if (eFAM_Data.FTR_Data.PutFTRStationID == FTRStationID.Opener_2)
                {
                    openerStateMachine_2.CurrentState = OpenerState.Opener_Open_Get;
                }

                return DeviceEvent.FTR_Put_Opener_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent LPDoor_Close()
        {
            var a = eFAM_Data
                .LoadportFuncCommand(
                    eFAM_Data.FTR_Data.PutGlobalStationID,
                    ZC_Control_EFAM.LPFuncType.Close_Door
                )
                .Result;
            if (a.Result)
            {
                return DeviceEvent.LPDoor_Close_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{a.Message}", 1);
            return DeviceEvent.Fail;
        }

        private DeviceEvent FTR_Put_Opener_And_LPDoor_Close()
        {
            // 启动两个任务
            var task1 = eFAM_Data.LoadportFuncCommand(
                eFAM_Data.FTR_Data.GetGlobalStationID,
                ZC_Control_EFAM.LPFuncType.Close_Door
            );
            var task2 = eFAM_Data.FTRFuncCommand(
                ActionType.Put,
                eFAM_Data.FTR_Data.PutFTRStationID
            );

            // 等待所有任务完成
            Task.WaitAll(task1, task2);

            // 获取结果（如果有返回值）
            var result1 = task1.Result; // 如果LoadportFuncCommand有返回值
            var result2 = task2.Result; // 如果FTRFuncCommand有返回值

            // 处理结果...
            // 例如记录日志或检查返回值
            if (result1.Result && result2.Result)
            {
                foreach (var item in eFAM_Data.Opener_Data)
                {
                    if (item.FTRStationID == eFAM_Data.FTR_Data.PutFTRStationID)
                    {
                        item.StationInfo.CopyStationInfo(eFAM_Data.FTR_Data.StationInfo);
                        eFAM_Data.FTR_Data.StationInfo.IsWafer = false;
                        item.IsMapComplete = false;
                        if (item.StationID == StationID.Opener_1)
                        {
                            openerStateMachine_1.CurrentState = OpenerState.Opener_Open;
                        }
                        else if (item.StationID == StationID.Opener_2)
                        {
                            openerStateMachine_2.CurrentState = OpenerState.Opener_Open;
                        }
                        break;
                    }
                }

                return DeviceEvent.FTR_Put_Opener_And_LPDoor_Close_Complete;
            }
            else
            {
                return DeviceEvent.Fail;
            }
        }
    }
}
