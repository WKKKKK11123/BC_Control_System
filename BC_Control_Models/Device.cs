using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using BC_Control_Models.Log;

namespace BC_Control_Models
{

    
    /// <summary>
    /// t通信设备的实体类
    /// </summary>
    public class Device
    {

        private Dictionary<int, CancellationTokenSource> _paramCollectionDict = new Dictionary<int, CancellationTokenSource>();
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress
        {
            get; set;
        }
        /// <summary>
        /// 站名
        /// </summary>
        public string StationName
        {
            get; set;
        }
      

        public int Port
        {
            get; set;
        }

        /// <summary>
        /// 组集合
        /// </summary>
        public List<Group> GroupList
        {
            get; set;
        }

        /// <summary>
        /// 通信状态的标志位
        /// </summary>
        public bool IsConnected
        {
            get; set; 
        }

        /// <summary>
        /// 重连时间
        /// </summary>
        public int ReConnectTime
        {
            get; set;
        } = 500;

        /// <summary>
        /// 重连标志位
        /// </summary>
        public bool ReConnectSign
        {
            get; set;
        }=false;

        /// <summary>
        /// 所有变量键值对，Key：变量名称 Value：变量值
        /// </summary>
        public Dictionary<string, object> CurrentValue = new Dictionary<string, object>();

        /// <summary>
        /// 触发的事件
        /// </summary>
        public event Action<bool, Variable,PlcEnum> AlarmTrigEvent;


        /// <summary>
        /// 上报事件触发的事件
        /// </summary>
        public event Action<bool, Variable,PlcEnum> CarrierTrigEvent;

        /// <summary>
        /// 操作触发的事件
        /// </summary>
        public event Action<Variable> OperatorTrigEvent;

        /// <summary>
        /// 变量词典更新变量值
        /// </summary>
        /// <param name="variable"></param>
        public void UpdateVariable(Variable variable,PlcEnum plcEnum)
        {
            if (CurrentValue.ContainsKey(variable.VarName))
            {
                CurrentValue[variable.VarName] = variable.VarValue;
            }
            else
            {
                CurrentValue.Add(variable.VarName, variable.VarValue);
            }

            // 报警检测
            CheckAlarm(variable,plcEnum);
            // 报警检测
            CheckEvent(variable, plcEnum);          

            // 值更新
            variable.PreVarValue = variable.VarValue;
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="variable"></param>
        public void WriteOperatorLog(Variable variable)
        {
            if (variable.OperatorLog)
            {
                if (variable.PreVarValue != null && variable.VarValue != null)
                    if (variable.PreVarValue.ToString() != variable.VarValue.ToString())
                    {
                        OperatorTrigEvent?.Invoke(variable);
                    }
            }
        }

        /// <summary>
        /// 报警记录
        /// </summary>
        /// <param name="variable"></param>
        private void CheckAlarm(Variable variable,PlcEnum plcEnum)
        {
            bool currentValue = variable.VarValue?.ToString() == "True";
            bool preValue = variable.PreVarValue?.ToString() == "True";

            if (!preValue && currentValue)
            {
                variable.PosAlarmTime = DateTime.Now;
            }

            if (preValue && !currentValue)
            {
                variable.NegAlarmTime = DateTime.Now;
            }

            // 上升沿报警检测
            if (variable.PosAlarm)
            {
                // 报警发生
                if (!preValue && currentValue )
                {
                    AlarmTrigEvent?.Invoke(true, variable,plcEnum);
                }

                // 报警消除
                if (preValue && !currentValue)
                {
                    AlarmTrigEvent?.Invoke(false, variable, plcEnum);
                }

            }

            // 下降沿报警检测
            if (variable.NegAlarm)
            {
                // 报警发生
                if (preValue && !currentValue)
                {
                    AlarmTrigEvent?.Invoke(true, variable, plcEnum);
                }

                // 报警消除
                if (!preValue && currentValue)
                {
                    AlarmTrigEvent?.Invoke(false, variable, plcEnum);
                }

            }

        }

        /// <summary>
        /// 事件记录
        /// </summary>
        /// <param name="variable"></param>
        private void CheckEvent(Variable variable, PlcEnum plcEnum)
        {
            bool currentValue = variable.VarValue?.ToString() == "True";
            bool preValue = variable.PreVarValue?.ToString() == "True";

            if (!preValue && currentValue)
            {
                variable.PosAlarmTime = DateTime.Now;
            }
             
            if (preValue && !currentValue)
            {
                variable.NegAlarmTime = DateTime.Now;
            }
            if (variable.Event)
            {
                if (!preValue && currentValue)
                {
                    CarrierTrigEvent?.Invoke(true, variable, plcEnum);
                }

                if (preValue && !currentValue)
                {
                    CarrierTrigEvent?.Invoke(false, variable, plcEnum);
                }

            }

        }
       

        



        /// <summary>
        /// 索引器获取变量值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (CurrentValue.ContainsKey(key))
                {
                    return CurrentValue[key];
                }
                else
                {
                    return null; 
                }
            }
        }
    }

}
