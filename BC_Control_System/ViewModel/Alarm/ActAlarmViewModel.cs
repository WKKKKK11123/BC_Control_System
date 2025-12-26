using HandyControl.Tools.Extension;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_System.Event;
using BC_Control_System.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModel.Alarm
{

    public partial class ActAlarmViewModel : ObservableObject
    {
        private readonly IPLCHelper _plcHelper;
        [ObservableProperty]
        private BindingList<AlarmLog> alarmLogs;
        public DelegateCommand<AlarmLog> ResetAlarmSingleCommand { get; set; }
        public DelegateCommand ResetAllCommand { get; set; }
        public ActAlarmViewModel(IPLCHelper pLCHelper, IEventAggregator eventAggregator, DataBaseAddService dataBaseAddService)
        {
            try
            {
                _plcHelper = pLCHelper;
                AlarmLogs = new BindingList<AlarmLog>(dataBaseAddService.actualAlarmList);
                ResetAlarmSingleCommand = new DelegateCommand<AlarmLog>(ResetAlarmSingle);
                ResetAllCommand = new DelegateCommand(ResetAll);
                eventAggregator.GetEvent<AlarmUpdateEvent>().Subscribe(UpdateAlarmCollection);
            }
            catch (Exception EX)
            {
                throw;
            }
        }

        private void UpdateAlarmCollection(List<AlarmLog> newAlarmLogs)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                int t = 0;
                AlarmLogs = new BindingList<AlarmLog>(newAlarmLogs);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    int t = 1;
                    AlarmLogs = new BindingList<AlarmLog>(newAlarmLogs);
                });
            }
        }

        private void ResetAlarmSingle(AlarmLog alarmLog)
        {
            try
            {
                if (alarmLog == null)
                {
                    return;
                }
                PlcEnum P = (PlcEnum)Enum.Parse(typeof(PlcEnum), alarmLog.Controller);
                _plcHelper.CommonWrite(alarmLog.VarName, "False", P);
            }
            catch (Exception)
            {
                throw;
            }

        }
        private async void ResetAll()
        {
            try
            {
                _plcHelper.CommonWrite("M2151", "True", PlcEnum.PLC1);
                await Task.Delay(500);
                _plcHelper.CommonWrite("M2151", "False", PlcEnum.PLC1);

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
