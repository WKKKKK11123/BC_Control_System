using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZC_Control_EFAM.StatusManagement;

namespace ZC_Control_EFAM.ProcessControl
{
    public partial class ProcessControl
    {
        private void OpenerStatusFunc()
        {
            #region Opener_1
            //Opener_Open
            openerStateMachine_1.RegisterExecutor(
                OpenerState.Opener_Open,
                () =>
                {
                    return Opener_Open(1);
                }
            );

            //Opener_Open_Get
            openerStateMachine_1.RegisterExecutor(
                OpenerState.Opener_Open_Get,
                () =>
                {
                    return Opener_Open_Get(1);
                }
            );

            //Opener_Close
            openerStateMachine_1.RegisterExecutor(
                OpenerState.Opener_Close,
                () =>
                {
                    return Opener_Close(1);
                }
            );
            #endregion

            #region Opener_2
            //Opener_Open
            openerStateMachine_2.RegisterExecutor(
                OpenerState.Opener_Open,
                () =>
                {
                    return Opener_Open(2);
                }
            );

            //Opener_Open_Get
            openerStateMachine_2.RegisterExecutor(
                OpenerState.Opener_Open_Get,
                () =>
                {
                    return Opener_Open_Get(2);
                }
            );

            //Opener_Close
            openerStateMachine_2.RegisterExecutor(
                OpenerState.Opener_Close,
                () =>
                {
                    return Opener_Close(2);
                }
            );
            #endregion
        }

        private OpenerEvent Opener_Open(int OpenerNo)
        {
            StationID station = StationID.Opener_1;
            if (OpenerNo == 1)
            {
                station = StationID.Opener_1;
            }
            else if (OpenerNo == 2)
            {
                station = StationID.Opener_2;
            }
            var aaa = eFAM_Data.OpenerFuncCommand(station, true).Result;
            if (aaa.Result)
            {
                eFAM_Data.Opener_Data[OpenerNo - 1].IsMapComplete = false;
                return OpenerEvent.Opener_Open_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{aaa.Message}", 1);
            return OpenerEvent.Fail;
        }

        private OpenerEvent Opener_Open_Get(int OpenerNo)
        {
            StationID station = StationID.Opener_1;
            if (OpenerNo == 1)
            {
                station = StationID.Opener_1;
            }
            else if (OpenerNo == 2)
            {
                station = StationID.Opener_2;
            }
            var aaa = eFAM_Data.OpenerFuncCommand(station, true).Result;
            if (aaa.Result)
            {
                return OpenerEvent.Opener_Open_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{aaa.Message}", 1);
            return OpenerEvent.Fail;
        }

        private OpenerEvent Opener_Close(int OpenerNo)
        {
            StationID station = StationID.Opener_1;
            if (OpenerNo == 1)
            {
                station = StationID.Opener_1;
            }
            else if (OpenerNo == 2)
            {
                station = StationID.Opener_2;
            }
            eFAM_Data.Opener_Data[OpenerNo - 1].IsMapComplete = false;
            var aaa = eFAM_Data.OpenerFuncCommand(station, false).Result;
            if (aaa.Result)
            {
                eFAM_Data.Opener_Data[OpenerNo - 1].IsMapComplete = true;
                return OpenerEvent.Opener_Close_Complete;
            }
            //eFAM_Data.tcpHexClient.DisplayLog($"错误报文：{aaa.Message}", 1);
            return OpenerEvent.Fail;
        }
    }
}
