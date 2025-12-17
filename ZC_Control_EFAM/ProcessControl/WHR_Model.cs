using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZC_Control_EFAM.StatusManagement;

namespace ZC_Control_EFAM.ProcessControl
{
    public partial class ProcessControl
    {
        private void WHRStatusFunc()
        {
            #region WHR
            //Idle
            _wHRState.RegisterExecutor(
                WHRState.Idle,
                () =>
                {
                    return WHRIdleFunc();
                }
            );

            //WHR_Get_Opener_And_HV_Flip_Degree0
            _wHRState.RegisterExecutor(
                WHRState.WHR_Get_Opener_And_HV_Flip_Degree0,
                () =>
                {
                    return WHR_Get_Opener_And_HV_Flip_Degree0Func();
                }
            );
            //WHR HV Unlock Dirty
            _wHRState.RegisterExecutor(
                WHRState.HV_Unlock_Dirty,
                () =>
                {
                    return HV_Unlock_Dirty2();
                }
            );
            //WHR_Put_HV1
            _wHRState.RegisterExecutor(
                WHRState.WHR_Put_HV1,
                () =>
                {
                    return WHR_Put_HV();
                }
            );

            //HV_Lock_Dirty
            _wHRState.RegisterExecutor(
                WHRState.HV_Lock_Dirty,
                () =>
                {
                    return HV_Lock_Dirty();
                }
            );

            //WHR_Put_HV2
            _wHRState.RegisterExecutor(
                WHRState.WHR_Put_HV2,
                () =>
                {
                    return WHR_Put_HV2();
                }
            );

            //HV_Flip_Degree180
            _wHRState.RegisterExecutor(
                WHRState.HV_Flip_Degree180,
                () =>
                {
                    return HV_Flip_Degree180();
                }
            );
            //WHR_Get_HV1
            _wHRState.RegisterExecutor(
                WHRState.WHR_Get_HV1,
                () =>
                {
                    return WHR_Get_HV();
                }
            );
            //HV_UnLock_Clean
            _wHRState.RegisterExecutor(
                WHRState.HV_UnLock_Clean,
                () =>
                {
                    return HV_UnLock_Clean();
                }
            );

            //WHR_Get_HV2
            _wHRState.RegisterExecutor(
                WHRState.WHR_Get_HV2,
                () =>
                {
                    return WHR_Get_HV2();
                }
            );

            //WHR_Put_Opener
            _wHRState.RegisterExecutor(
                WHRState.WHR_Put_Opener,
                () =>
                {
                    return WHR_Put_Opener();
                }
            );

            #endregion
        }

        private WHREvent WHRIdleFunc()
        {
            if (!eFAM_Data.ControlMode || !eFAM_Data.IsConnected)
                return WHREvent.None;

            if (!eFAM_Data.WHR_Data.PlaceSenser && !eFAM_Data.HV_Data.PlaceSenser && GetHVState)
            {
                if (
                    openerStateMachine_1.CurrentState == OpenerState.Wait_FuncComplete
                    && eFAM_Data.Opener_Data[0].StationInfo.ProcessState != ProcessState.Processing
                )
                {
                    eFAM_Data.WHR_Data.GetWHRStationID = WHRStationID.Opener_1;

                    return WHREvent.WHR_To_Get_Opener;
                }
                else if (
                    openerStateMachine_2.CurrentState == OpenerState.Wait_FuncComplete
                    && eFAM_Data.Opener_Data[1].StationInfo.ProcessState != ProcessState.Processing
                )
                {
                    eFAM_Data.WHR_Data.GetWHRStationID = WHRStationID.Opener_2;
                    return WHREvent.WHR_To_Get_Opener;
                }
            }

            if (
                eFAM_Data.HV_Data.StationInfo.ProcessState == ProcessState.Processed
                && eFAM_Data.HV_Data.PlaceSenser
                && eFAM_Data.HV_Data.StationInfo.IsWafer
                && !eFAM_Data.WHR_Data.PlaceSenser
                && GetHVState
            )
            {
                return WHREvent.TO_HV_Flip_Degree180;
            }

            if (
                eFAM_Data.WHR_Data.PlaceSenser
                && eFAM_Data.WHR_Data.StationInfo.ProcessState == ProcessState.Processed
            )
            {
                if (
                    openerStateMachine_1.CurrentState == OpenerState.Wait_FuncComplete
                    && eFAM_Data.Opener_Data[0].StationInfo.StorageID
                        == eFAM_Data.WHR_Data.StationInfo.StorageID
                    && eFAM_Data.Opener_Data[0].StationInfo.ProcessState == ProcessState.Processing
                    && eFAM_Data.Opener_Data[0].PlaceSenser
                )
                {
                    eFAM_Data.WHR_Data.PutWHRStationID = WHRStationID.Opener_1;
                    return WHREvent.WHR_To_Put_Opener;
                }
                else if (
                    openerStateMachine_2.CurrentState == OpenerState.Wait_FuncComplete
                    && eFAM_Data.Opener_Data[1].StationInfo.StorageID
                        == eFAM_Data.WHR_Data.StationInfo.StorageID
                    && eFAM_Data.Opener_Data[1].StationInfo.ProcessState == ProcessState.Processing
                    && eFAM_Data.Opener_Data[1].PlaceSenser
                )
                {
                    eFAM_Data.WHR_Data.PutWHRStationID = WHRStationID.Opener_2;
                    return WHREvent.WHR_To_Put_Opener;
                    ;
                }
            }

            return WHREvent.None;
        }
        
        private WHREvent WHR_Get_Opener_And_HV_Flip_Degree0Func()
        {
            _hVState.CurrentState = HVState.Busy;
            // 启动两个任务
            //GetOpener
            var task1 = eFAM_Data.WTRFuncCommand(
                WHRActionType.Get,
                eFAM_Data.WHR_Data.GetWHRStationID,
                WTRArmType.DirtyArm
            );
            var task2 = eFAM_Data.HV_FlipCommand(HV_FlipFuncType.Flip_Degree0);

            // 等待所有任务完成
            Task.WaitAll(task1, task2);

            // 获取结果（如果有返回值）
            var result1 = task1.Result;
            var result2 = task2.Result;

            // 处理结果...
            // 例如记录日志或检查返回值
            if (result1.Result && result2.Result)
            {
                if (eFAM_Data.WHR_Data.GetWHRStationID == WHRStationID.Opener_1)
                {
                    //eFAM_Data.WHR_Data.StationInfo =
                    //    JsonConvert.DeserializeObject<StationInfo>(
                    //        JsonConvert.SerializeObject(
                    //            eFAM_Data.Opener_Data[0].StationInfo
                    //        )
                    //    )!;
                    eFAM_Data.WHR_Data.StationInfo.CopyStationInfo(
                        eFAM_Data.Opener_Data[0].StationInfo
                    );
                    eFAM_Data.Opener_Data[0].StationInfo.ProcessState = ProcessState.Processing;
                    //eFAM_Data.UpdataStationData();
                    openerStateMachine_1.CurrentState = OpenerState.Opener_Close;
                }
                else if (eFAM_Data.WHR_Data.GetWHRStationID == WHRStationID.Opener_2)
                {
                    //eFAM_Data.WHR_Data.StationInfo =
                    //    JsonConvert.DeserializeObject<StationInfo>(
                    //        JsonConvert.SerializeObject(
                    //            eFAM_Data.Opener_Data[1].StationInfo
                    //        )
                    //    )!;
                    eFAM_Data.WHR_Data.StationInfo.CopyStationInfo(
                        eFAM_Data.Opener_Data[1].StationInfo
                    );
                    eFAM_Data.Opener_Data[1].StationInfo.ProcessState = ProcessState.Processing;

                    //eFAM_Data.UpdataStationData();
                    openerStateMachine_2.CurrentState = OpenerState.Opener_Close;
                }
                eFAM_Data.WHR_Data.StationInfo.IsWafer = true;
                eFAM_Data.WHR_Data.StationInfo.ProcessState = ProcessState.Processing;
                return WHREvent.WHR_Get_Opener_And_HV_Flip_Degree0_Complete;
            }
            else
            {
                return WHREvent.Fail;
            }
        }

        private WHREvent WHR_Put_HV()
        {
            var a = eFAM_Data
                .WTRFuncCommand(WHRActionType.Put, WHRStationID.HV, WTRArmType.DirtyArm)
                .Result;
            if (a.Result)
            {
                //eFAM_Data.HV_Data.StationInfo =
                //    JsonConvert.DeserializeObject<StationInfo>(
                //        JsonConvert.SerializeObject(eFAM_Data.WHR_Data.StationInfo)
                //    )!;
                //eFAM_Data.HV_Data.StationInfo.CopyStationInfo(eFAM_Data.WHR_Data.StationInfo);
                //eFAM_Data.WHR_Data.StationInfo.IsWafer = false;
                //eFAM_Data.UpdataStationData();
                return WHREvent.WHR_Put_HV1_Complete;
            }
            return WHREvent.Fail;
        }

        private WHREvent WHR_Put_HV2()
        {
            eFAM_Data.HV_Data.StationInfo.CopyStationInfo(eFAM_Data.WHR_Data.StationInfo);
            eFAM_Data.WHR_Data.StationInfo.IsWafer = false;
            _hVState.CurrentState = HVState.Idle;
            //eFAM_Data.UpdataStationData();
            return WHREvent.WHR_Put_HV2_Complete;
            var a = eFAM_Data
                .WTRFuncCommand(WHRActionType.Put, WHRStationID.HV, WTRArmType.DirtyArmReturn)
                .Result;
            if (a.Result)
            {
                //eFAM_Data.HV_Data.StationInfo =
                //    JsonConvert.DeserializeObject<StationInfo>(
                //        JsonConvert.SerializeObject(eFAM_Data.WHR_Data.StationInfo)
                //    )!;
                eFAM_Data.HV_Data.StationInfo.CopyStationInfo(eFAM_Data.WHR_Data.StationInfo);
                eFAM_Data.WHR_Data.StationInfo.IsWafer = false;
                _hVState.CurrentState = HVState.Idle;
                //eFAM_Data.UpdataStationData();
                return WHREvent.WHR_Put_HV2_Complete;
            }
            return WHREvent.Fail;
        }

        private WHREvent HV_Lock_Dirty()
        {
            var a = eFAM_Data.HV_LockCommand(HV_LockFuncType.Dirty_Lock).Result;
            if (a.Result)
            {
                //_hVState.CurrentState = HVState.Idle;
                return WHREvent.HV_Lock_Dirty_Complete;
            }
            return WHREvent.Fail;
        }
        private WHREvent HV_Unlock_Dirty2()
        {
            var a = eFAM_Data.HV_LockCommand(HV_LockFuncType.Dirty_UnLock).Result;
            if (a.Result)
            {
                //_hVState.CurrentState = HVState.Idle;
                return WHREvent.HV_Unlock_Dirty_Complete;
            }
            return WHREvent.Fail;
        }
        private WHREvent HV_Flip_Degree180()
        {
            _hVState.CurrentState = HVState.Busy;
            var a = eFAM_Data.HV_FlipCommand(HV_FlipFuncType.Flip_Degree180).Result;
            if (a.Result)
            {
                return WHREvent.HV_Flip_Degree180_Complete;
            }
            return WHREvent.Fail;
        }

        private WHREvent HV_UnLock_Clean()
        {
            var a = eFAM_Data.HV_LockCommand(HV_LockFuncType.Clean_UnLock).Result;
            if (a.Result)
            {
                return WHREvent.HV_UnLock_Clean_Complete;
            }
            return WHREvent.Fail;
        }

        private WHREvent WHR_Get_HV()
        {
            var a = eFAM_Data
                .WTRFuncCommand(WHRActionType.Get, WHRStationID.HV, WTRArmType.CleanArm)
                .Result;
            if (a.Result)
            {
                //eFAM_Data.WHR_Data.StationInfo =
                //    JsonConvert.DeserializeObject<StationInfo>(
                //        JsonConvert.SerializeObject(eFAM_Data.HV_Data.StationInfo)
                //    )!;
                //eFAM_Data.WHR_Data.StationInfo.CopyStationInfo(eFAM_Data.HV_Data.StationInfo);
                //eFAM_Data.HV_Data.StationInfo.IsWafer = false;
                //eFAM_Data.UpdataStationData();
                //_hVState.CurrentState = HVState.Idle;
                return WHREvent.WHR_Get_HV1_Complete;
            }
            return WHREvent.Fail;
        }

        private WHREvent WHR_Get_HV2()
        {
            eFAM_Data.WHR_Data.StationInfo.CopyStationInfo(eFAM_Data.HV_Data.StationInfo);//跳过执行此步骤 只执行数据流装
            eFAM_Data.HV_Data.StationInfo.IsWafer = false;
            //eFAM_Data.UpdataStationData();
            _hVState.CurrentState = HVState.Idle;
            return WHREvent.WHR_Get_HV2_Complete;
            var a = eFAM_Data
                .WTRFuncCommand(WHRActionType.Get, WHRStationID.HV, WTRArmType.CleanArmReturn)
                .Result;
            if (a.Result)
            {
                //eFAM_Data.WHR_Data.StationInfo =
                //    JsonConvert.DeserializeObject<StationInfo>(
                //        JsonConvert.SerializeObject(eFAM_Data.HV_Data.StationInfo)
                //    )!;
                eFAM_Data.WHR_Data.StationInfo.CopyStationInfo(eFAM_Data.HV_Data.StationInfo);
                eFAM_Data.HV_Data.StationInfo.IsWafer = false;
                //eFAM_Data.UpdataStationData();
                _hVState.CurrentState = HVState.Idle;
                return WHREvent.WHR_Get_HV2_Complete;
            }
            return WHREvent.Fail;
        }

        private WHREvent WHR_Put_Opener()
        {
            var a = eFAM_Data
                .WTRFuncCommand(
                    WHRActionType.Put,
                    eFAM_Data.WHR_Data.PutWHRStationID,
                    WTRArmType.CleanArm
                )
                .Result;
            if (a.Result)
            {
                if (eFAM_Data.WHR_Data.PutWHRStationID == WHRStationID.Opener_1)
                {
                    //eFAM_Data.Opener_Data[0].StationInfo =
                    //    JsonConvert.DeserializeObject<StationInfo>(
                    //        JsonConvert.SerializeObject(eFAM_Data.WHR_Data.StationInfo)
                    //    )!;
                    eFAM_Data
                        .Opener_Data[0]
                        .StationInfo.CopyStationInfo(eFAM_Data.WHR_Data.StationInfo);
                    eFAM_Data.WHR_Data.StationInfo.IsWafer = false;
                    //eFAM_Data.Opener_Data[0].StationInfo.IsNullFoup = false;
                    //eFAM_Data.UpdataStationData();
                    openerStateMachine_1.CurrentState = OpenerState.Opener_Close;
                }
                else if (eFAM_Data.WHR_Data.PutWHRStationID == WHRStationID.Opener_2)
                {
                    //eFAM_Data.Opener_Data[1].StationInfo =
                    //    JsonConvert.DeserializeObject<StationInfo>(
                    //        JsonConvert.SerializeObject(eFAM_Data.WHR_Data.StationInfo)
                    //    )!;
                    eFAM_Data
                        .Opener_Data[1]
                        .StationInfo.CopyStationInfo(eFAM_Data.WHR_Data.StationInfo);
                    eFAM_Data.WHR_Data.StationInfo.IsWafer = false;
                    //eFAM_Data.Opener_Data[1].StationInfo.IsNullFoup = false;
                    //eFAM_Data.UpdataStationData();
                    openerStateMachine_2.CurrentState = OpenerState.Opener_Close;
                }
                return WHREvent.WHR_Put_Opener_Complete;
            }
            return WHREvent.Fail;
        }
    }
}
