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
        private void PusherStatusFunc()
        {
            #region Pusher
            //Idle
            _pusherState.RegisterExecutor(
                PusherState.Idle,
                () =>
                {
                    return PusherIdleFunc();
                }
            );

            //Pusher_Rotate_And_HV_90
            _pusherState.RegisterExecutor(
                PusherState.Pusher_Rotate_And_HV_90,
                () =>
                {
                    return Pusher_Rotate_And_HV_90();
                }
            );

            //Pusher_To_HV_Get_Position
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_HV_Get_Position,
                () =>
                {
                    return Pusher_To_HV_Get_Position();
                }
            );

            //HV_Unlock_Dirty
            _pusherState.RegisterExecutor(
                PusherState.HV_Unlock_Dirty,
                () =>
                {
                    return HV_Unlock_Dirty();
                }
            );

            //Pusher_To_Map
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_Map,
                () =>
                {
                    return Pusher_To_Map();
                }
            );
            //Pusher_Rotate_Degree0
            _pusherState.RegisterExecutor(
                PusherState.Pusher_Rotate_Degree0,
                () =>
                {
                    return Pusher_Rotate_Degree0();
                }
            );

            //Pusher_To_WTR_Get_Position
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_WTR_Get_Position,
                () =>
                {
                    return Pusher_To_WTR_Get_Position();
                }
            );

            //Pusher_To_WTR_Put_Position
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_WTR_Put_Position,
                () =>
                {
                    return Pusher_To_WTR_Put_Position();
                }
            );

            //Pusher_To_Map_And_HV_To_270
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_Map_And_HV_To_270,
                () =>
                {
                    return Pusher_To_Map_And_HV_To_270();
                }
            );
            //20251014 HVUnlockClean
            _pusherState.RegisterExecutor(    
                PusherState.HV_Unlock_Clean,
                () =>
                {
                    return PrimaryHVGetUnlockClean();   
                }
            );
            //HV_To_270
            _pusherState.RegisterExecutor(
                PusherState.HV_To_270,
                () =>
                {
                    return HV_To_270();
                }
            );

            //Pusher_To_HV_Put_Position
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_HV_Put_Position,
                () =>
                {
                    return Pusher_To_HV_Put_Position();
                }
            );

            //Pusher_To_Home
            _pusherState.RegisterExecutor(
                PusherState.Pusher_To_Home,
                () =>
                {
                    return Pusher_To_Home();
                }
            );

            //Pusher_To_Home
            _pusherState.RegisterExecutor(
                PusherState.Move_To_Home,
                () =>
                {
                    return Pusher_To_Home();
                }
            );

            //HV_lock_Clean
            _pusherState.RegisterExecutor(
                PusherState.HV_lock_Clean,
                () =>
                {
                    return HV_lock_Clean();
                }
            );

            #endregion
        }

        private PusherEvent PusherIdleFunc()
        {
            if (!eFAM_Data.ControlMode || !eFAM_Data.IsConnected)
                return PusherEvent.None;

            if (
                eFAM_Data.HV_Data.PlaceSenser
                && eFAM_Data.HV_Data.StationInfo.IsWafer
                && eFAM_Data.HV_Data.StationInfo.ProcessState == ProcessState.Processing
                && GetHVState
            )
            {
                switch (eFAM_Data.HV_Data.StationInfo.OddEven)
                {
                    case ZC_Control_EFAM.OddEven.Odd:
                        if (!eFAM_Data.Pusher_Data.Odd_Data.IsWafer)
                        {
                            return PusherEvent.To_Pusher_Rotate_And_HV_90_Status;
                        }
                        break;
                    case ZC_Control_EFAM.OddEven.Even:
                        if (!eFAM_Data.Pusher_Data.Even_Data.IsWafer)
                        {
                            return PusherEvent.To_Pusher_Rotate_And_HV_90_Status;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (WTRPutRequest && !eFAM_Data.Pusher_Data.PlaceSenser && WTRAllowPut)
            {
                //WTRPutRequest = false;
                return PusherEvent.To_Pusher_To_WTR_Put_Position_Status;
            }
            if (
                !eFAM_Data.HV_Data.PlaceSenser
                && !eFAM_Data.HV_Data.StationInfo.IsWafer
                && GetHVState
            )
            {
                if (
                    eFAM_Data.Pusher_Data.Odd_Data.IsWafer
                    && eFAM_Data.Pusher_Data.Odd_Data.ProcessState == ProcessState.Processed
                )
                {
                    eFAM_Data.Pusher_Data.PutType = ZC_Control_EFAM.OddEven.Odd;
                    if (eFAM_Data.Pusher_Data.IsMapComplete)
                    {
                        return PusherEvent.To_HV_To_270_Status;
                    }
                    return PusherEvent.To_Pusher_To_Map_And_HV_To_270_Status;
                }
                else if (
                    eFAM_Data.Pusher_Data.Even_Data.IsWafer
                    && eFAM_Data.Pusher_Data.Even_Data.ProcessState == ProcessState.Processed
                )
                {
                    eFAM_Data.Pusher_Data.PutType = ZC_Control_EFAM.OddEven.Even;
                    if (eFAM_Data.Pusher_Data.IsMapComplete)
                    {
                        return PusherEvent.To_HV_To_270_Status;
                    }
                    return PusherEvent.To_Pusher_To_Map_And_HV_To_270_Status;
                }
            }

            return PusherEvent.None;
        }

        private PusherEvent Pusher_Rotate_And_HV_90()
        {
            _hVState.CurrentState = HVState.Busy;
            // 启动两个任务
            bool a =
                eFAM_Data.HV_Data.StationInfo.OddEven == ZC_Control_EFAM.OddEven.Odd ? false : true;
            var task1 = eFAM_Data.Pusher_Rotate(false);
            var task2 = eFAM_Data.HV_FlipCommand(HV_FlipFuncType.Flip_Degree90);

            // 等待所有任务完成
            Task.WaitAll(task1, task2);

            // 获取结果（如果有返回值）
            var result1 = task1.Result;
            var result2 = task2.Result;

            // 处理结果...
            // 例如记录日志或检查返回值
            if (result1.Result && result2.Result)
            {
                return PusherEvent.Pusher_Rotate_And_HV_90_Complete;
            }
            else
            {
                return PusherEvent.Fail;
            }
        }

        private PusherEvent Pusher_To_HV_Get_Position()
        {
            var a = eFAM_Data
                .PusherToHVCommand(
                    ZC_Control_EFAM.PusherMoveMode.Move,
                    eFAM_Data.HV_Data.StationInfo.OddEven,
                    ZC_Control_EFAM.PusherToHV_Func.Get
                )
                .Result;
            if (a.Result)
            {
                return PusherEvent.Pusher_To_HV_Get_Position_Complete;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent HV_Unlock_Dirty()
        {
            var a = eFAM_Data.HV_LockCommand(HV_LockFuncType.Dirty_UnLock).Result;
            if (a.Result)
            {
                switch (eFAM_Data.HV_Data.StationInfo.OddEven)
                {
                    case ZC_Control_EFAM.OddEven.Odd:
                        eFAM_Data.Pusher_Data.Odd_Data.CopyStationInfo(
                            eFAM_Data.HV_Data.StationInfo
                        );

                        break;
                    case ZC_Control_EFAM.OddEven.Even:
                        eFAM_Data.Pusher_Data.Even_Data.CopyStationInfo(
                            eFAM_Data.HV_Data.StationInfo
                        );
                        break;
                    default:
                        break;
                }

                eFAM_Data.HV_Data.StationInfo.IsWafer = false;
                if (
                    (
                        eFAM_Data.HV_Data.StationInfo.DoubleProcess
                        && eFAM_Data.Pusher_Data.Odd_Data.IsWafer
                        && eFAM_Data.Pusher_Data.Even_Data.IsWafer
                    ) || !eFAM_Data.HV_Data.StationInfo.DoubleProcess
                )
                    return PusherEvent.HV_Unlock_Dirty_Complete;
                else
                    return PusherEvent.HV_Unlock_Dirty_Complete_ToHome;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_To_Map()
        {
            var a = eFAM_Data.PusherMapping().Result;
            if (a.Result)
            {
                _hVState.CurrentState = HVState.Idle;
                eFAM_Data.Pusher_Data.IsMapComplete = true;
                return PusherEvent.Pusher_To_Map_Complete;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_Rotate_Degree0()
        {
            var a = eFAM_Data.Pusher_Rotate(false).Result;
            if (a.Result)
            {
                if (
                    (
                        eFAM_Data.Pusher_Data.Odd_Data.DoubleProcess
                        && eFAM_Data.Pusher_Data.Odd_Data.IsWafer
                        && !eFAM_Data.Pusher_Data.Even_Data.IsWafer
                    )
                    || (
                        eFAM_Data.Pusher_Data.Even_Data.DoubleProcess
                        && eFAM_Data.Pusher_Data.Even_Data.IsWafer
                        && !eFAM_Data.Pusher_Data.Odd_Data.IsWafer
                    )
                )
                {
                    return PusherEvent.Pusher_Wait;
                }
                return PusherEvent.Pusher_NoWait;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_To_WTR_Get_Position()
        {
            var a = eFAM_Data.PusherToRB_Getpos(ArmType.DirtyArm).Result;
            if (a.Result)
            {
                WTSDownRecipe?.Invoke(eFAM_Data.Pusher_Data.RecipeName);
                return PusherEvent.Pusher_To_WTR_Get_Position_Complete;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_To_WTR_Put_Position()
        {
            var a = eFAM_Data.PusherToRB_Getpos(ArmType.CleanArm).Result;
            if (a.Result)
            {
                return PusherEvent.Pusher_To_WTR_Put_Position_Complete;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_To_Map_And_HV_To_270()
        {
            _hVState.CurrentState = HVState.Busy;
            var task1 = eFAM_Data.PusherMapping();
            var task2 = eFAM_Data.HV_FlipCommand(HV_FlipFuncType.Flip_Degree270);

            // 等待所有任务完成
            Task.WaitAll(task1, task2);

            // 获取结果（如果有返回值）
            var result1 = task1.Result;
            var result2 = task2.Result;

            // 处理结果...
            // 例如记录日志或检查返回值
            if (result1.Result && result2.Result)
            {
                eFAM_Data.Pusher_Data.IsMapComplete = true;
                return PusherEvent.Pusher_To_Map_And_HV_To_270_Complete;
            }
            else
            {
                return PusherEvent.Fail;
            }
        }

        private PusherEvent HV_To_270()
        {
            _hVState.CurrentState = HVState.Busy;
            var task2 = eFAM_Data.HV_FlipCommand(HV_FlipFuncType.Flip_Degree270).Result;

            if (task2.Result)
            {
                eFAM_Data.Pusher_Data.IsMapComplete = true;
                return PusherEvent.HV_To_270_Complete;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_To_HV_Put_Position()
        {
            var a = eFAM_Data
                .PusherToHVCommand(
                    ZC_Control_EFAM.PusherMoveMode.Move,
                    eFAM_Data.Pusher_Data.PutType,
                    ZC_Control_EFAM.PusherToHV_Func.Put
                )
                .Result;
            if (a.Result)
            {
                return PusherEvent.Pusher_To_HV_Put_Position_Complete;
            }
            return PusherEvent.Fail;
        }

        private PusherEvent Pusher_To_Home()
        {
            var a = eFAM_Data.PusherToHome().Result;
            if (a.Result)
            {
                _hVState.CurrentState = HVState.Idle;
                return PusherEvent.Pusher_To_Home_Complete;
            }
            return PusherEvent.Fail;
        }
        private PusherEvent PrimaryHVGetUnlockClean()
        {
            var a = eFAM_Data.HV_LockCommand(HV_LockFuncType.Clean_UnLock).Result;
            if (a.Result)
            {
                return PusherEvent.HV_Unlock_Clean_Complete;
            }
            return PusherEvent.Fail;
        }
        private PusherEvent HV_lock_Clean()
        {
            var a = eFAM_Data.HV_LockCommand(HV_LockFuncType.Clean_Lock).Result;
            if (a.Result)
            {
                switch (eFAM_Data.Pusher_Data.PutType)
                {
                    case ZC_Control_EFAM.OddEven.Odd:
                        eFAM_Data.HV_Data.StationInfo.CopyStationInfo(
                            eFAM_Data.Pusher_Data.Odd_Data
                        );
                        eFAM_Data.Pusher_Data.Odd_Data.IsWafer = false;
                        break;
                    case ZC_Control_EFAM.OddEven.Even:
                        eFAM_Data.HV_Data.StationInfo.CopyStationInfo(
                            eFAM_Data.Pusher_Data.Even_Data
                        );
                        eFAM_Data.Pusher_Data.Even_Data.IsWafer = false;
                        break;
                    default:
                        break;
                }

                return PusherEvent.HV_lock_Clean_Complete;
            }
            return PusherEvent.Fail;
        }
    }
}
