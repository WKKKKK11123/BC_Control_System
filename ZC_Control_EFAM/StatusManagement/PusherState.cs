using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZC_Control_EFAM.StatusManagement
{
    public enum PusherState
    {
        Idle,
        Move_To_Home,
        Pusher_Rotate_And_HV_90,
        Pusher_To_HV_Get_Position,
        HV_Unlock_Dirty,
        Pusher_To_Map,
        Pusher_To_Home,
        Pusher_Rotate_Degree0,
        Pusher_To_WTR_Get_Position,
        WaitWTRGet,

        Pusher_To_WTR_Put_Position,
        WaitWTRPUT,
        Pusher_To_Map_And_HV_To_270,
        HV_To_270,
        HV_Unlock_Clean,   //20251014 UnlockClean
        Pusher_To_HV_Put_Position,
        HV_lock_Clean,
        Error,
    }

    public enum PusherEvent
    {
        None,
        Pusher_Rotate_And_HV_90_Complete,
        Pusher_To_HV_Get_Position_Complete,
        HV_Unlock_Dirty_Complete,
        Pusher_To_Map_Complete,
        Pusher_To_Home_Complete,
        Pusher_Rotate_Degree0_Complete,
        Pusher_Wait,
        Pusher_NoWait,
        Pusher_To_WTR_Get_Position_Complete,

        Pusher_To_WTR_Put_Position_Complete,
        Pusher_To_Map_And_HV_To_270_Complete,
        HV_To_270_Complete,
        HV_Unlock_Clean_Complete, //20251014 UnlockClean
        Pusher_To_HV_Put_Position_Complete,
        HV_lock_Clean_Complete,
        HV_Unlock_Dirty_Complete_ToHome,

        To_Pusher_Rotate_And_HV_90_Status,
        To_Pusher_To_WTR_Put_Position_Status,
        To_Pusher_To_Map_And_HV_To_270_Status,
        To_HV_To_270_Status,
        Fail,
        Reset
    }

    public class PusherStateMachine
    {
        private string preState;
        private readonly List<PusherStateTransition> _transitions;

        private readonly Dictionary<PusherState, Func<PusherEvent?>> _stateExecutors =
            new Dictionary<PusherState, Func<PusherEvent?>>();
        public PusherState CurrentState { get; set; }

        //public Action<PusherState, PusherState> OnStateChanged { get; set; }

        public PusherStateMachine()
        {
            CurrentState = PusherState.Idle;

            _transitions = new List<PusherStateTransition>
            {
                //new PusherStateTransition(
                //    PusherState.Idle,
                //    PusherEvent.To_Pusher_Rotate_And_HV_90_Status,
                //    PusherState.Pusher_Rotate_And_HV_90
                //),
                new PusherStateTransition(
                    PusherState.Idle,
                    PusherEvent.To_Pusher_Rotate_And_HV_90_Status,
                    PusherState.Move_To_Home
                ),
                new PusherStateTransition(
                    PusherState.Move_To_Home,
                    PusherEvent.Pusher_To_Home_Complete,
                    PusherState.Pusher_Rotate_And_HV_90
                ),
                new PusherStateTransition(
                    PusherState.Pusher_Rotate_And_HV_90,
                    PusherEvent.Pusher_Rotate_And_HV_90_Complete,
                    PusherState.Pusher_To_HV_Get_Position
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_HV_Get_Position,
                    PusherEvent.Pusher_To_HV_Get_Position_Complete,
                    PusherState.HV_Unlock_Dirty
                ),
                new PusherStateTransition(
                    PusherState.HV_Unlock_Dirty,
                    PusherEvent.HV_Unlock_Dirty_Complete,
                    PusherState.Pusher_To_Map
                ),
                new PusherStateTransition(
                    PusherState.HV_Unlock_Dirty,
                    PusherEvent.HV_Unlock_Dirty_Complete_ToHome,
                    PusherState.Pusher_To_Home
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_Map,
                    PusherEvent.Pusher_To_Map_Complete,
                    PusherState.Pusher_Rotate_Degree0
                ),
                new PusherStateTransition(
                    PusherState.Pusher_Rotate_Degree0,
                    PusherEvent.Pusher_Wait,
                    PusherState.Idle
                ),
                new PusherStateTransition(
                    PusherState.Pusher_Rotate_Degree0,
                    PusherEvent.Pusher_NoWait,
                    PusherState.Pusher_To_WTR_Get_Position
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_WTR_Get_Position,
                    PusherEvent.Pusher_To_WTR_Get_Position_Complete,
                    PusherState.WaitWTRGet
                ),
                new PusherStateTransition(
                    PusherState.Idle,
                    PusherEvent.To_Pusher_To_WTR_Put_Position_Status,
                    PusherState.Pusher_To_WTR_Put_Position
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_WTR_Put_Position,
                    PusherEvent.Pusher_To_WTR_Put_Position_Complete,
                    PusherState.WaitWTRPUT
                ),
                /////////////////
                new PusherStateTransition(
                    PusherState.Pusher_To_Map_And_HV_To_270,
                    PusherEvent.Pusher_To_Map_And_HV_To_270_Complete,
                    PusherState.HV_Unlock_Clean
                ),
               new PusherStateTransition(        //Unlock HV Clean
                    PusherState.HV_Unlock_Clean,
                    PusherEvent.HV_Unlock_Clean_Complete,
                    PusherState.Pusher_To_HV_Put_Position
                ),
                new PusherStateTransition(
                    PusherState.Idle,
                    PusherEvent.To_Pusher_To_Map_And_HV_To_270_Status,
                    PusherState.Pusher_To_Map_And_HV_To_270
                ),

                new PusherStateTransition(
                    PusherState.HV_To_270,
                    PusherEvent.HV_To_270_Complete,
                    PusherState.Pusher_To_HV_Put_Position
                ),
                new PusherStateTransition(
                    PusherState.Idle,
                    PusherEvent.To_HV_To_270_Status,
                    PusherState.HV_To_270
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_HV_Put_Position,
                    PusherEvent.Pusher_To_HV_Put_Position_Complete,
                    PusherState.HV_lock_Clean
                ),
                new PusherStateTransition(
                    PusherState.HV_lock_Clean,
                    PusherEvent.HV_lock_Clean_Complete,
                    PusherState.Pusher_To_Home
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_Home,
                    PusherEvent.Pusher_To_Home_Complete,
                    PusherState.Idle
                ),
                //new PusherStateTransition(PusherState.Idle, PusherEvent.None, PusherState.Idle),
                new PusherStateTransition(
                    PusherState.Pusher_Rotate_And_HV_90,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_HV_Get_Position,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                 new PusherStateTransition(        //Unlock HV Clean 20251014
                    PusherState.HV_Unlock_Clean,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.HV_Unlock_Dirty,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_Map,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_Rotate_Degree0,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_WTR_Get_Position,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_WTR_Put_Position,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_Map_And_HV_To_270,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.HV_To_270,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_HV_Put_Position,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.HV_lock_Clean,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Pusher_To_Home,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
                new PusherStateTransition(
                    PusherState.Move_To_Home,
                    PusherEvent.Fail,
                    PusherState.Error
                ),
            };
        }

        //
        public void RegisterExecutor(PusherState state, Func<PusherEvent?> executor)
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
                    Fire(PusherEvent.Fail);
                    return;
                }

                Fire(evt.Value);
            }
            else
            {
                Fire(PusherEvent.Fail);
            }
        }

        public void TriggerStateAction(PusherState state)
        {
            if (_stateExecutors.TryGetValue(state, out var executor))
            {
                var resultEvent = executor?.Invoke();

                if (resultEvent == null)
                {
                    return;
                }

                if (resultEvent == PusherEvent.Fail)
                {
                    Fire(PusherEvent.Fail);
                    return;
                }

                Fire(resultEvent.Value);
            }
            else { }
        }

        private void Fire(PusherEvent trigger)
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
                //Fire(PusherEvent.Fail);
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
                var state = JsonConvert.DeserializeObject<PusherState>(json);
                if (state != null)
                {
                    CurrentState = state;
                    if (state != PusherState.Idle)
                    {
                        _restoredFromFile = true;
                    }
                }
            }
        }

        public class PusherStateTransition
        {
            public PusherState From { get; }
            public PusherEvent Trigger { get; }
            public PusherState To { get; }

            public PusherStateTransition(PusherState from, PusherEvent trigger, PusherState to)
            {
                From = from;
                Trigger = trigger;
                To = to;
            }

            public override string ToString() => $"{From} --{Trigger}→ {To}";
        }
    }
}
