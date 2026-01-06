using BC_Control_BLL.services;
using BC_Control_BLL.Services;
using BC_Control_Models;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_DAL;
using Prism.Regions;
using BC_Control_Models.Log;
using BC_Control_System.Event;
using Prism.Events;
using Newtonsoft.Json;
using BC_Control_Helper;
using BC_Control_Models.ClassK.SQLService;
using System.Windows.Navigation;
using BC_Control_BLL.services.TraceLogService;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BC_Control_System.Service
{
    public class DataBaseAddService
    {
        #region
        private readonly IContainerProvider _containerProvider;
        private readonly TK1LogDataService tk1LogService;
        private readonly TK2LogDataService tk2LogService;
        private readonly TK3LogDataService tk3LogService;
        private readonly TK4LogDataService tk4LogService;
        private readonly TK5LogDataService tk5LogService;
        private readonly TK6LogDataService tk6LogService;
        private readonly TK7LogDataService tk7LogService;
        private readonly TK8LogDataService tk8LogService;
        private readonly TK9LogDataService tk9LogService;
        private readonly TK10LogDataService tk10LogService;
        private readonly TK11LogDataService tk11LogService;
        private readonly AlarmLogService alarmLogManage;
        private readonly OperatorLogService operatorLogManage;
        private readonly EndofRunLogService endofRunLogService;
        private readonly EventLogService eventLogManage;
        private readonly IExcelOperation _excelOperation;
        private readonly ILogOpration _logOpration;
        private readonly TankProcessService tankProcessService;
        private readonly IEventAggregator _eventAggregator;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List<AlarmLog> actualAlarmList { get; private set; }
        #endregion

        public DataBaseAddService(IContainerProvider containerProvider, IExcelOperation excelOperation, ILogOpration logOpration, IEventAggregator eventAggregator)
        {
            actualAlarmList = new List<AlarmLog>();
            _containerProvider = containerProvider;
            tk10LogService = containerProvider.Resolve<TK10LogDataService>();
            tk9LogService = containerProvider.Resolve<TK9LogDataService>();
            tk8LogService = containerProvider.Resolve<TK8LogDataService>();
            tk7LogService = containerProvider.Resolve<TK7LogDataService>();
            tk1LogService = containerProvider.Resolve<TK1LogDataService>();
            tk2LogService = containerProvider.Resolve<TK2LogDataService>();
            tk3LogService = containerProvider.Resolve<TK3LogDataService>();
            tk4LogService = containerProvider.Resolve<TK4LogDataService>();
            tk5LogService = containerProvider.Resolve<TK5LogDataService>();
            tk6LogService = containerProvider.Resolve<TK6LogDataService>();
            tk10LogService = containerProvider.Resolve<TK10LogDataService>();
            tk11LogService = containerProvider.Resolve<TK11LogDataService>();
            tankProcessService = containerProvider.Resolve<TankProcessService>();
            alarmLogManage = containerProvider.Resolve<AlarmLogService>();
            eventLogManage = containerProvider.Resolve<EventLogService>();
            operatorLogManage = containerProvider.Resolve<OperatorLogService>();
            endofRunLogService = containerProvider.Resolve<EndofRunLogService>();
            _excelOperation = excelOperation;
            _logOpration = logOpration;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ProcessTankLogUpdateEvent>().Subscribe(UpdateTankProcessLog);
        }
        /// <summary>
        /// TraceLog
        /// </summary>
        /// <param name="cts"></param>
        public void AddActualLogAsync(CancellationTokenSource cts)
        {
            var tocken = cts;
            Task.Run(async () =>
            {
                try
                {
                    string temppath = Path.Combine(
                    @"D:\BC3100\212\File\OterFile",
                    "LogAddress.xlsx");
                    List<Task> tasks = new List<Task>();
                    Dictionary<string, List<StatusClass>> tempDataLogKeyValue = new Dictionary<string, List<StatusClass>>();
                    tempDataLogKeyValue.Add("tk1", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk1").ToList());
                    tempDataLogKeyValue.Add("tk2", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk2").ToList());
                    tempDataLogKeyValue.Add("tk3", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk3").ToList());
                    tempDataLogKeyValue.Add("tk4", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk4").ToList());
                    tempDataLogKeyValue.Add("tk5", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk5").ToList());
                    tempDataLogKeyValue.Add("tk6", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk6").ToList());
                    tempDataLogKeyValue.Add("tk7", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk7").ToList());
                    tempDataLogKeyValue.Add("tk8", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk8").ToList());
                    tempDataLogKeyValue.Add("tk9", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk9").ToList());
                    tempDataLogKeyValue.Add("tk10", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk10").ToList());
                    tempDataLogKeyValue.Add("tk11", _excelOperation.ReadExcelToObjects<StatusClass>(temppath, "tk11").ToList());
                    await Task.Delay(5000);
                    while (!tocken.IsCancellationRequested)
                    {
                        tasks.Clear();
                        tasks.Add(tk1LogService.SaveActualDataLog(tempDataLogKeyValue["tk1"]));
                        tasks.Add(tk2LogService.SaveActualDataLog(tempDataLogKeyValue["tk2"]));
                        tasks.Add(tk3LogService.SaveActualDataLog(tempDataLogKeyValue["tk3"]));
                        tasks.Add(tk4LogService.SaveActualDataLog(tempDataLogKeyValue["tk4"]));
                        tasks.Add(tk5LogService.SaveActualDataLog(tempDataLogKeyValue["tk5"]));
                        tasks.Add(tk6LogService.SaveActualDataLog(tempDataLogKeyValue["tk6"]));
                        tasks.Add(tk7LogService.SaveActualDataLog(tempDataLogKeyValue["tk7"]));
                        tasks.Add(tk8LogService.SaveActualDataLog(tempDataLogKeyValue["tk8"]));
                        tasks.Add(tk9LogService.SaveActualDataLog(tempDataLogKeyValue["tk9"]));
                        tasks.Add(tk10LogService.SaveActualDataLog(tempDataLogKeyValue["tk10"]));
                        tasks.Add(tk11LogService.SaveActualDataLog(tempDataLogKeyValue["tk11"]));
                        await Task.WhenAll(tasks);
                        await Task.Delay(2000);
                    }
                }
                catch (Exception ee)
                {
                    _logOpration.WriteError($"AddActualLogAsync Error {ee.Message}");
                }
            }, tocken.Token);
        }
        /// <summary>
        /// 订阅实体TankProcess更新的方法
        /// </summary>
        /// <param name="moduleStatus"></param>
        public async void UpdateTankProcessLog(ModuleStatus moduleStatus)
        {
            if (moduleStatus.IsWafer.Value == "1")
            {
                long addResult = await tankProcessService.Add(new TankProcess
                {
                    DataId = int.Parse(moduleStatus.DataID.Value),
                    ModuleRecipeName = moduleStatus.UnitRecipeName.Value,
                    StartTime = DateTime.Now,
                    StationName = moduleStatus.StationName,
                    StationNo = moduleStatus.StationNo
                });

                if (addResult != 1)
                    _logOpration.WriteError($"制程记录添加失败 {moduleStatus.ModuleName}");
            }
            else
            {

                bool updated = await tankProcessService.UpdateEntity(
                 x => new TankProcess
             {
                 EndTime = DateTime.Now,
                 ModuleRecipeName = moduleStatus.UnitRecipeName.Value,
                 StationName = moduleStatus.ModuleName,
                 StationNo = moduleStatus.StationNo
             },
             x => x.DataId == int.Parse(moduleStatus.DataID.Value) && x.StationNo== moduleStatus.StationNo
);


                if (!updated)
                    _logOpration.WriteError($"制程记录变更失败 {moduleStatus.ModuleName}");
            }

        }
        /// <summary>
        /// 设备事件触发记录
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="variable"></param>
        /// <param name="plcEnum"></param>
        public async void Device_TrigEvent(bool arg1, Variable variable, PlcEnum plcEnum)
        {
            try
            {
                if (arg1) // 报警发生时
                {
                    try
                    {
                        var eventLog = new BC_Control_Models.Personal.EventLog()
                        {
                            InsertTime = DateTime.Now,
                            Comment = variable.Remark,
                            VarName = variable.VarName,
                            GroupName = variable.GroupName,
                            Priority = variable.Priority.ToString(),
                            Controller = plcEnum.ToString(),
                        };

                        // 存储
                        await eventLogManage.Add(eventLog);
                    }
                    catch (Exception ee)
                    {
                        _logOpration.WriteError($"Event Error {ee.Message}");
                    }
                }

            }
            catch (Exception ee)
            {
                _logOpration.WriteError($"Event Error {ee.Message}");
            }
        }
        /// <summary>
        /// 发生报警事件处理的方法
        /// </summary>
        /// <param name="ackType"></param>
        /// <param name="variable"></param>
        public async void Device_AlarmTrigEvent(bool ackType, Variable variable, PlcEnum plcEnum)
        {
            try
            {
                if (ackType) // 报警发生时
                {
                    try
                    {
                        var alarmLog = new AlarmLog()
                        {
                            Code = variable.VarName,
                            //Controller = "",
                            InsertTime = variable.PosAlarmTime,
                            //ClearedTime = "",
                            //AcknowledgeTime = "",
                            Controller = plcEnum.ToString(),
                            AlarmType = "触发",
                            Comment = variable.Remark,
                            Operator = CommonMethods.CurrentAdmin?.LoginName,
                            GroupName = variable.GroupName,
                            VarName = variable.VarName,
                            Priority = variable.Priority.ToString(),
                        };

                        var alarmLogTemp = actualAlarmList?
                            .ToList()
                            .Find(c => c.VarName == alarmLog.VarName && c.Controller == alarmLog.Controller);

                        if (alarmLogTemp == null)
                        {
                            actualAlarmList.Add(alarmLog);
                            _eventAggregator.GetEvent<AlarmUpdateEvent>().Publish(actualAlarmList);


                        }

                        // 报警存储
                        await alarmLogManage.Add(alarmLog);
                    }
                    catch (Exception ee)
                    {
                        _logOpration.WriteError($"Alarm Error {ee.Message}");
                    }
                }
                else
                {
                    try
                    {
                        var alarmLog = new AlarmLog()
                        {
                            //Code = "",
                            Controller = plcEnum.ToString(),
                            InsertTime = variable.PosAlarmTime,
                            ClearedTime = variable.NegAlarmTime,
                            //AcknowledgeTime = “ ”,
                            AlarmType = "消失",
                            Comment = variable.Remark,
                            Operator = CommonMethods.CurrentAdmin?.LoginName,
                            GroupName = variable.GroupName,
                            VarName = variable.VarName,
                            Priority = variable.Priority.ToString(),
                        };

                        var alarmLogTemp = actualAlarmList
                            .ToList()
                            .Find(c => c.VarName == alarmLog.VarName && c.Controller == alarmLog.Controller);

                        if (alarmLogTemp != null)
                        {
                            actualAlarmList.Remove(alarmLogTemp);
                            _eventAggregator.GetEvent<AlarmUpdateEvent>().Publish(actualAlarmList);

                        }

                        // 报警存储
                        await alarmLogManage.UpdateEntity(setExpression: a => new AlarmLog() { ClearedTime = variable.NegAlarmTime, }
                            , whereExpression: p => (
                            p.InsertTime == alarmLogTemp.InsertTime
                            && alarmLogTemp.VarName == p.VarName
                            && alarmLogTemp.Controller == p.Controller));


                    }
                    catch (Exception ee)
                    {
                        _logOpration.WriteError($"Alarm Error {ee.Message}");
                    }
                }
            }
            catch (Exception ee)
            {
                _logOpration.WriteError($"Alarm Error {ee.Message}");
            }
        }

        /// <summary>
        /// 发生操作事件处理的方法
        /// </summary>
        /// <param name="variable"></param>
        public async void Device_OperatorTrigEvent(Variable variable)
        {
            try
            {
                var operatorLog = new OperatorLog()
                {
                    InsertTime = DateTime.Now,
                    OldValue = variable.PreVarValue.ToString(),
                    NewValue = variable.VarValue.ToString(),
                    Comment = variable.Remark,
                    Operator = CommonMethods.CurrentAdmin?.LoginName,
                    GroupName = variable.GroupName,
                    VarName = variable.VarName,
                };

                // 事件记录存储
                await operatorLogManage.Add(operatorLog);
            }
            catch (Exception ee)
            {
                _logOpration.WriteError($"Opration Error {ee.Message}");
            }
        }
        /// <summary>
        /// 设备批次开始记录的方法
        /// </summary>
        /// <param name="endofRunLog"></param>
        /// <returns></returns>
        public async Task<int> AddEndofRunLog(EndofRunData endofRunLog)
        {
            try
            {
                int id = await endofRunLogService.AddReturnId(endofRunLog);
                return id;
            }
            catch (Exception ee)
            {
                _logOpration.WriteError(ee);
                throw;
            }
        }
        /// <summary>
        /// 设备批次结束记录的方法
        /// </summary>
        /// <param name="storageID"></param>
        /// <returns></returns>
        public async Task UpdateEndofRunLog(int storageID)
        {
            try
            {
                var data = await endofRunLogService.Query(filter => (storageID == filter.MessageID && filter.Status == "Run"));
                if (data.Count() > 0)
                {
                    var updatedata = data.Last();
                    updatedata.EndTime = DateTime.Now;
                    updatedata.Status = "Finish";
                    bool b = await endofRunLogService.UpdateEntity(updatedata);
                    _containerProvider.Resolve<EAPService>().CJStopAction = new Func<string>(() => { return updatedata.BatchID.Replace("\0", ""); });
                    _containerProvider.Resolve<EAPService>().CJEndState = true;
                }
            }
            catch (Exception ee)
            {
                _logOpration.WriteError(ee);
            }
        }

    }
}
