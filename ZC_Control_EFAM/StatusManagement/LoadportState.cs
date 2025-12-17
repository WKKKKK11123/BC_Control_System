using MathNet.Numerics.LinearAlgebra.Factorization;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZC_Control_EFAM.StatusManagement
{
    public enum LoadportStateEnum
    {
        OutOfService,
        InService,
        TransferReady,
        ReadyToLoad,
        ReadyToUnload,
        TransferBlocked
    }
    public enum LoadportEvent
    {

        Stop,
        Transfer,
        WaitingForEntity,
        UpLoadServiceFinish,
        DownloadServiceFinish,
        ChangeServiceStatus,
        Cancel

    }

    public class LoadportState : StationStateBase
    {
        private readonly StateMachine<LoadportStateEnum, LoadportEvent> _machine;
        public AutoResetEvent TransferStart { get; set; } = new AutoResetEvent(false);
        public AutoResetEvent DownLoadFinish { get; set; } = new AutoResetEvent(false);
        public string LPName { get; set; }
        public bool FTRTransferState { get; set; }//20251015
        public void ChangeService()
        {
            if (_machine.State == LoadportStateEnum.OutOfService
                || _machine.State == LoadportStateEnum.InService
                || _machine.State == LoadportStateEnum.ReadyToLoad)
            {
                _machine.Fire(LoadportEvent.ChangeServiceStatus);
            }
        }

        public async Task WaitingForLoad()
        {
            await Task.Run(() =>
            {
                TransferStart.WaitOne();
                _machine.Fire(LoadportEvent.Transfer);
            });
        }
        public async Task WaitingForUnLoad()
        {
            await Task.Run(() =>
            {
                DownLoadFinish.WaitOne();
                _machine.Fire(LoadportEvent.DownloadServiceFinish);
            });
        }
        public LoadportState():base()
        {
            _machine = new StateMachine<LoadportStateEnum, LoadportEvent>(LoadportStateEnum.InService);
            _machine.Configure(LoadportStateEnum.InService)
                .Permit(LoadportEvent.ChangeServiceStatus, LoadportStateEnum.OutOfService)
                .Permit(LoadportEvent.WaitingForEntity, LoadportStateEnum.ReadyToLoad);
            _machine.Configure(LoadportStateEnum.OutOfService)
                .Permit(LoadportEvent.ChangeServiceStatus, LoadportStateEnum.InService);
            _machine.Configure(LoadportStateEnum.ReadyToLoad)
                .Permit(LoadportEvent.Transfer, LoadportStateEnum.TransferBlocked)
                .Permit(LoadportEvent.Cancel, LoadportStateEnum.InService)
                .Permit(LoadportEvent.ChangeServiceStatus, LoadportStateEnum.OutOfService);
            _machine.Configure(LoadportStateEnum.TransferBlocked)
                .Permit(LoadportEvent.UpLoadServiceFinish, LoadportStateEnum.ReadyToLoad)
                .Permit(LoadportEvent.DownloadServiceFinish, LoadportStateEnum.ReadyToUnload)
                .Permit(LoadportEvent.Cancel, LoadportStateEnum.InService);
            VerifyStatus = true;
            Task.Run(() =>
            {
                while (true)
                {
                    UpdateLoadportState();
                    Thread.Sleep(1000);
                }
            });
        }
        private void UpdateLoadportState()
        {
            try
            {
                switch (CurrentState)
                {
                    case LoadportStateEnum.OutOfService:

                        break;
                    case LoadportStateEnum.InService:
                        if (StationInfo.ProcessState == ProcessState.UnProcess
                            && PlaceSenser
                            && string.IsNullOrEmpty(StationInfo.RFID)
                            )
                        {
                            Fire(LoadportEvent.WaitingForEntity);
                        }
                        break;
                    case LoadportStateEnum.TransferReady:

                        break;
                    case LoadportStateEnum.ReadyToLoad:
                        if (StationInfo.ProcessState == ProcessState.UnProcess
                            && PlaceSenser
                            && !string.IsNullOrEmpty(StationInfo.RFID)
                            && VerifyStatus
                            )
                        {
                            Fire(LoadportEvent.Transfer);
                        }
                        break;
                    case LoadportStateEnum.ReadyToUnload:

                        break;
                    case LoadportStateEnum.TransferBlocked:
                        if (StationInfo.ProcessState == ProcessState.Processed
                            && PlaceSenser
                            )
                        {
                            Fire(LoadportEvent.DownloadServiceFinish);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                Fire(LoadportEvent.Cancel);
            }
        }
        //public LoadportStateEnum CurrentState => _machine.State;
        public LoadportStateEnum CurrentState
        {
            get => _machine.State;
        }

        public void Fire(LoadportEvent evt)
        {
            try
            {
                _machine.Fire(evt);
                Console.WriteLine($"[] 状态变化后：{_machine.State}");
            }
            catch (Exception ee)
            {


            }

        }
    }
}

