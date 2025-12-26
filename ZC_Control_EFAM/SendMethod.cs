using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_EFAM
{
    public partial class ZC_EFAM_Data
    {
        #region WTR方法

        /// <summary>
        /// WTR取放方法
        /// </summary>
        /// <param name="wHRActionType">取放选择</param>
        /// <param name="wHRStationID">目标工位</param>
        /// <param name="armType">手指选择</param>
        /// <returns></returns>
        public async Task<MegModel> WTRFuncCommand(
            WHRActionType wHRActionType,
            WHRStationID wHRStationID,
            WTRArmType armType
        )
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "WTR_Func");
            if (b != null)
            {
                b.f_8t = ((byte)wHRActionType).ToString("X2");
                b.f_9t = ((byte)wHRStationID).ToString("X2");
                b.f_11t = ((byte)armType).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// WTR 移动到MAP位置
        /// </summary>
        /// <param name="armType">手指选择</param>
        /// <returns></returns>
        public async Task<MegModel> WTRMoveToMAPCommand(ArmType armType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "WTR_Map");
            if (b != null)
            {
                b.f_11t = ((byte)armType).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region FTR方法
        /// <summary>
        /// FTR取放操作命令
        /// </summary>
        /// <param name="actionType">取放类型</param>
        /// <param name="stationID">要作业的站点，工位定义是FTR的定义</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> FTRFuncCommand(ActionType actionType, FTRStationID stationID)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "FTR");
            if (b != null)
            {
                b.f_8t = ((byte)actionType).ToString("X2");
                b.f_9t = ((byte)stationID).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region Opener操作
        /// <summary>
        /// Opener操作
        /// </summary>
        /// <param name="stationID">站点ID</param>
        /// <param name="func">要进行的动作  false:关  true:开</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> OpenerFuncCommand(StationID stationID, bool func)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            if ((int)stationID > 1)
            {
                megModel.ErrorCode = "超出了Opener工位的编号！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Opener");
            if (b != null)
            {
                b.f_6t = ((byte)stationID).ToString("X2");
                b.f_8t = func ? "04" : "05";
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region LP操作
        /// <summary>
        /// LP操作
        /// </summary>
        /// <param name="stationID">站点ID</param>
        /// <param name="lPFuncType">动作类型</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> LoadportFuncCommand(StationID stationID, LPFuncType lPFuncType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            if ((int)stationID > 0x17 || (int)stationID < 0x14)
            {
                megModel.ErrorCode = "超出了LoadPort工位的编号！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "LP");
            if (b != null)
            {
                b.f_6t = ((byte)stationID).ToString("X2");
                b.f_8t = ((byte)lPFuncType).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region 读取RFID
        /// <summary>
        /// 读取RFID
        /// </summary>
        /// <param name="lp_No">要读取的LP号</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> ReadRFIDCommand(byte lp_No)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            if (lp_No > 4 || lp_No < 1)
            {
                megModel.ErrorCode = "超出了LoadPort工位的编号,请输入1--4！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "RFID_Read");
            if (b != null)
            {
                b.f_8t = lp_No.ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region HV相关的方法
        /// <summary>
        /// HV翻转的命令
        /// </summary>
        /// <param name="hV_FlipFuncType">Flip翻转角度的参数</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> HV_FlipCommand(HV_FlipFuncType hV_FlipFuncType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "HV_Flip");
            if (b != null)
            {
                b.f_9t = ((byte)hV_FlipFuncType).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// HV抱片动作方法
        /// </summary>
        /// <param name="hV_LockFuncType">抱片结构动作类型</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> HV_LockCommand(HV_LockFuncType hV_LockFuncType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "HV_Lock");
            if (b != null)
            {
                switch (hV_LockFuncType.ToString())
                {
                    case nameof(HV_LockFuncType.Dirty_Lock):
                    case nameof(HV_LockFuncType.Clean_Lock):
                        b.f_8t = "14";
                        break;
                    case nameof(HV_LockFuncType.Dirty_UnLock):
                    case nameof(HV_LockFuncType.Clean_UnLock):
                        b.f_8t = "15";
                        break;
                }

                switch (hV_LockFuncType.ToString())
                {
                    case nameof(HV_LockFuncType.Dirty_Lock):
                    case nameof(HV_LockFuncType.Dirty_UnLock):
                        b.f_11t = "00";
                        break;
                    case nameof(HV_LockFuncType.Clean_Lock):
                    case nameof(HV_LockFuncType.Clean_UnLock):
                        b.f_11t = "01";
                        break;
                }

                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        #endregion

        #region Pusher相关方法
        /// <summary>
        /// Pusher移动到HV取放位的方法
        /// </summary>
        /// <param name="pusherMoveMode">普通运动===》轨迹运动</param>
        /// <param name="oddEven">奇偶模式</param>
        /// <param name="pusherToHV_Func">取放模式</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherToHVCommand(
            PusherMoveMode pusherMoveMode,
            OddEven oddEven,
            PusherToHV_Func pusherToHV_Func
        )
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_To_HV");
            if (b != null)
            {
                b.f_8t = ((byte)pusherMoveMode).ToString("X2");
                b.f_9t = ((byte)oddEven).ToString("X2");
                b.f_11t = ((byte)pusherToHV_Func).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher移动到Home
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherToHome()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_To_Home");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// PusherMapping
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherMapping()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_Mapping");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher移动到RB取点位
        /// </summary>
        /// <param name="armType">干净还是脏污选择</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherToRB_Getpos(ArmType armType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_To_RB_Getpos");
            if (b != null)
            {
                b.f_11t = ((byte)armType).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher移动到上定位
        /// </summary>
        /// <param name="armType">干净还是脏污选择</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherToUp(ArmType armType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_Move_Up");
            if (b != null)
            {
                b.f_11t = ((byte)armType).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher移动到下定位
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherToDown()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_Move_Down");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher移动到安全位置
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> PusherToSafe()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_Track_To_Safe");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher——Comb动作
        /// </summary>
        /// <param name="armType">干净还是脏污选择</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> Pusher_Comb(ArmType armType)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_Comb");
            if (b != null)
            {
                if (armType == ArmType.DirtyArm)
                {
                    b.f_8t = "1B";
                }
                else if (armType == ArmType.CleanArm)
                {
                    b.f_8t = "1C";
                }

                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// Pusher旋转
        /// </summary>
        /// <param name="angle">false=》0°   true=》180°</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> Pusher_Rotate(bool angle)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Pusher_Rotate");
            if (b != null)
            {
                b.f_9t = angle ? "01" : "00";

                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 直接检索命令并发送
        /// </summary>
        /// <param name="meg">从表格中取得需要执行的命令字符串</param>
        /// <returns>返回结果的MegModel实体</returns>
        public async Task<MegModel> SendCommand(string meg)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            var b = List_hex.FirstOrDefault(c => c.f_HEXName == meg);

            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                //SendQueue.Add(new MegModel() { UUID = a, Message = b.f_full });
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// 站点获取状态的方法
        /// </summary>
        /// <param name="stationID">站点ID，</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> GetStatusCommand(StationID stationID)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Update");
            if (b != null)
            {
                b.f_6t = ((byte)stationID).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// 站点初始化的方法
        /// </summary>
        /// <param name="stationID">站点ID，</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> ResetCommand(StationID stationID)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            if (((int)stationID > 0x5 && (int)stationID < 0x14) || ((int)stationID == 0x18))
            {
                megModel.ErrorCode = $"目标工位{stationID.ToString()}不存在复位的命令！";
                return megModel;
            }
            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Reset");
            if (b != null)
            {
                b.f_6t = ((byte)stationID).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// 站点清除错误的方法
        /// </summary>
        /// <param name="stationID">站点ID，</param>
        /// <returns>返回结果</returns>
        public async Task<MegModel> ClearErrorCommand(StationID stationID)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }
            if (((int)stationID > 0x5 && (int)stationID < 0x14) || ((int)stationID == 0x18))
            {
                megModel.ErrorCode = $"目标工位{stationID.ToString()}不存在清除报错的命令！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "ClearError");
            if (b != null)
            {
                b.f_6t = ((byte)stationID).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion

        #region 系统命令

        /// <summary>
        /// 系统初始化
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> Sys_All_Reset()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Sys_All_Reset");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// 获取LP检知状态
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> Sys_Get_LPStatus()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Sys_Get_LP_Status");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// 设置系统操作模式
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> Sys_Set_Mode(SysMode sysMode)
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Sys_Set_Model");
            if (b != null)
            {
                b.f_8t = ((byte)sysMode).ToString("X2");
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;

                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }

        /// <summary>
        /// 获取StorageAll的状态
        /// </summary>
        /// <returns>返回结果</returns>
        public async Task<MegModel> Sys_Get_AllStorage_Status()
        {
            MegModel megModel = new MegModel() { Result = false };

            if (!tcpHexClient.IsConnected)
            {
                megModel.ErrorCode = "网络连接失败！";
                return megModel;
            }

            HEX_EN b;

            b = List_hex.FirstOrDefault(c => c.f_HEXName == "Sys_Get_Storage_All");
            if (b != null)
            {
                long a = tcpHexClient.SendHexPayload(b.f_full);
                return await ChackReveState(a);
            }
            else
            {
                megModel.ErrorCode = "未找到需要发送的报文！";
            }

            return megModel;
        }
        #endregion
    }
}
