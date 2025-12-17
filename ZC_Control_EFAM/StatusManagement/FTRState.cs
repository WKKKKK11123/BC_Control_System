using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZC_Control_EFAM.StatusManagement
{
    public enum DeviceState
    {
        Idle,
        Read_RFID,
        LPDoor_Open_And_ToLPReady_Get,
        LPDoor_Open_And_ToLPReady_Put,
        FTR_Get_LP,
        FTR_Put_LP,
        FTR_Get_Storage_Dirty,
        FTR_Get_Storage_Clean,
        FTR_Put_Storage,
        FTR_Get_Opener,
        FTR_Put_Opener,
        FTR_Put_Opener_And_LPDoor_Close,
        LPDoor_Close,
        Error,
    }

    public enum DeviceEvent
    {
        Verdict_Complete,
        None,
        Read_RFID_Complete,
        LPDoor_Open_And_ToLPReady_Get_Complete,
        LPDoor_Open_And_ToLPReady_Put_Complete,
        FTR_Get_LP_Complete,
        FTR_Put_LP_Complete,
        FTR_Get_Storage_Dirty_Complete,
        FTR_Get_Storage_Clean_Complete,
        FTR_Put_Storage_Complete,
        FTR_Get_Opener_Complete,
        FTR_Put_Opener_Complete,
        FTR_Put_Opener_And_LPDoor_Close_Complete,
        LPDoor_Close_Complete,

        To_Read_RFID_Status,
        To_Get_Storage_Dirty_Status,
        To_Get_Storage_Clean_Status,
        To_Get_Opener_Status,

        Fail,
        Reset
    }

    public class DeviceStateMachine
    {
        private string preState;

        private readonly List<StateTransition> _transitions;

        private readonly Dictionary<DeviceState, Func<DeviceEvent?>> _stateExecutors =
            new Dictionary<DeviceState, Func<DeviceEvent?>>();
        public DeviceState CurrentState { get; set; }

        public Action<DeviceState, DeviceState> OnStateChanged { get; set; }

        public DeviceStateMachine()
        {
            CurrentState = DeviceState.Idle;

            _transitions = new List<StateTransition>
            {
                new StateTransition(
                    DeviceState.Idle,
                    DeviceEvent.To_Read_RFID_Status,
                    DeviceState.Read_RFID
                ),
                new StateTransition(
                    DeviceState.Idle,
                    DeviceEvent.To_Get_Storage_Dirty_Status,
                    DeviceState.FTR_Get_Storage_Dirty
                ),
                new StateTransition(
                    DeviceState.Idle,
                    DeviceEvent.To_Get_Storage_Clean_Status,
                    DeviceState.FTR_Get_Storage_Clean
                ),
                new StateTransition(
                    DeviceState.Idle,
                    DeviceEvent.To_Get_Opener_Status,
                    DeviceState.FTR_Get_Opener
                ),
                new StateTransition(
                    DeviceState.Read_RFID,
                    DeviceEvent.Read_RFID_Complete,
                    DeviceState.LPDoor_Open_And_ToLPReady_Get
                ),
                new StateTransition(
                    DeviceState.LPDoor_Open_And_ToLPReady_Get,
                    DeviceEvent.LPDoor_Open_And_ToLPReady_Get_Complete,
                    DeviceState.FTR_Get_LP
                ),
                new StateTransition(
                    DeviceState.LPDoor_Open_And_ToLPReady_Put,
                    DeviceEvent.LPDoor_Open_And_ToLPReady_Put_Complete,
                    DeviceState.FTR_Put_LP
                ),
                new StateTransition(
                    DeviceState.FTR_Get_LP,
                    DeviceEvent.FTR_Get_LP_Complete,
                    DeviceState.FTR_Put_Opener_And_LPDoor_Close
                ),
                new StateTransition(
                    DeviceState.FTR_Put_LP,
                    DeviceEvent.FTR_Put_LP_Complete,
                    DeviceState.LPDoor_Close
                ),
                new StateTransition(
                    DeviceState.FTR_Get_Storage_Dirty,
                    DeviceEvent.FTR_Get_Storage_Dirty_Complete,
                    DeviceState.FTR_Put_Opener
                ),
                new StateTransition(
                    DeviceState.FTR_Get_Storage_Clean,
                    DeviceEvent.FTR_Get_Storage_Clean_Complete,
                    DeviceState.LPDoor_Open_And_ToLPReady_Put
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Storage,
                    DeviceEvent.FTR_Put_Storage_Complete,
                    DeviceState.Idle
                ),
                new StateTransition(
                    DeviceState.FTR_Get_Opener,
                    DeviceEvent.FTR_Get_Opener_Complete,
                    DeviceState.FTR_Put_Storage
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Opener,
                    DeviceEvent.FTR_Put_Opener_Complete,
                    DeviceState.Idle
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Storage,
                    DeviceEvent.FTR_Put_Storage_Complete,
                    DeviceState.Idle
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Opener_And_LPDoor_Close,
                    DeviceEvent.FTR_Put_Opener_And_LPDoor_Close_Complete,
                    DeviceState.Idle
                ),
                new StateTransition(
                    DeviceState.LPDoor_Close,
                    DeviceEvent.LPDoor_Close_Complete,
                    DeviceState.Idle
                ),
                new StateTransition(DeviceState.Error, DeviceEvent.Reset, DeviceState.Idle),
                //new StateTransition(DeviceState.Idle, DeviceEvent.None, DeviceState.Idle),
                new StateTransition(DeviceState.Read_RFID, DeviceEvent.Fail, DeviceState.Error),
                new StateTransition(
                    DeviceState.LPDoor_Open_And_ToLPReady_Get,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(
                    DeviceState.LPDoor_Open_And_ToLPReady_Put,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(DeviceState.FTR_Get_LP, DeviceEvent.Fail, DeviceState.Error),
                new StateTransition(DeviceState.FTR_Put_LP, DeviceEvent.Fail, DeviceState.Error),
                new StateTransition(
                    DeviceState.FTR_Get_Storage_Dirty,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(
                    DeviceState.FTR_Get_Storage_Clean,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Storage,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(
                    DeviceState.FTR_Get_Opener,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Opener,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(
                    DeviceState.FTR_Put_Opener_And_LPDoor_Close,
                    DeviceEvent.Fail,
                    DeviceState.Error
                ),
                new StateTransition(DeviceState.LPDoor_Close, DeviceEvent.Fail, DeviceState.Error),
            };
        }

        //
        public void RegisterExecutor(DeviceState state, Func<DeviceEvent?> executor)
        {
            _stateExecutors[state] = executor;
        }

        public void ExecuteCurrentState()
        {
            if (_restoredFromFile)
            {
                _restoredFromFile = false; // 下次执行就不跳过
                return;
            }
            if (_stateExecutors.TryGetValue(CurrentState, out var executor))
            {
                var evt = executor?.Invoke();

                if (evt == null)
                {
                    Fire(DeviceEvent.Fail);
                    return;
                }

                Fire(evt.Value);
            }
            else
            {
                Fire(DeviceEvent.Fail);
            }
        }

        public void TriggerStateAction(DeviceState state)
        {
            if (_stateExecutors.TryGetValue(state, out var executor))
            {
                var resultEvent = executor?.Invoke();

                if (resultEvent == null)
                {
                    return;
                }

                if (resultEvent == DeviceEvent.Fail)
                {
                    Fire(DeviceEvent.Fail);
                    return;
                }

                Fire(resultEvent.Value);
            }
            else { }
        }

        private void Fire(DeviceEvent trigger)
        {
            var transition = _transitions.FirstOrDefault(t =>
                t.From == CurrentState && t.Trigger == trigger
            );
            if (transition != null)
            {
                // 弹窗确认
                //var result = MessageBox.Show(
                //    $"是否切换状态：{CurrentState} → {transition.To}？",
                //    "状态切换确认",
                //    MessageBoxButton.YesNo
                //);

                //if (result != MessageBoxResult.Yes)
                //    return;

                var from = CurrentState;
                preState = CurrentState.ToString();
                CurrentState = transition.To;

                //OnStateChanged?.Invoke(from, CurrentState);
            }
            else
            {
                return;
                //Fire(DeviceEvent.Fail);
            }
        }

        public void SaveStateToJson(string path)
        {
            string json = JsonConvert.SerializeObject(CurrentState);
            File.WriteAllText(path, json);
        }

        private bool _restoredFromFile = false;

        public void LoadStateFromJson(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var state = JsonConvert.DeserializeObject<DeviceState>(json);
                if (state != null)
                {
                    CurrentState = state;
                    if (state != DeviceState.Idle)
                    {
                        _restoredFromFile = true;
                    }
                }
            }
        }
    }

    public class StateTransition
    {
        public DeviceState From { get; }
        public DeviceEvent Trigger { get; }
        public DeviceState To { get; }

        public StateTransition(DeviceState from, DeviceEvent trigger, DeviceState to)
        {
            From = from;
            Trigger = trigger;
            To = to;
        }

        public override string ToString() => $"{From} --{Trigger}→ {To}";
    }
}
