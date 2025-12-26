using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZC_Control_EFAM.StatusManagement
{
    public enum WHRState
    {
        Idle,
        WHR_Get_Opener_And_HV_Flip_Degree0,
        HV_Unlock_Dirty,                //202510111 旋转到脏手指后完成后,解锁Dirty手指
        WHR_Put_HV1,
        HV_Lock_Dirty,
        WHR_Put_HV2,

        HV_Flip_Degree180,
        WHR_Get_HV1,
        HV_UnLock_Clean,
        WHR_Get_HV2,
        WaitOpenerOpen,
        WHR_Put_Opener,

        Error,
    }

    public enum WHREvent
    {
        None,
        WHR_Get_Opener_And_HV_Flip_Degree0_Complete,
        HV_Unlock_Dirty_Complete,             //202510111 旋转到脏手指后完成后,解锁Dirty手指
        WHR_Put_HV1_Complete,
        HV_Lock_Dirty_Complete,
        WHR_Put_HV2_Complete,

        HV_Flip_Degree180_Complete,
        WHR_Get_HV1_Complete,
        HV_UnLock_Clean_Complete,
        WHR_Get_HV2_Complete,
        WHR_Put_Opener_Complete,
        WaitOpenerOpenComplete,

        WHR_To_Get_Opener,
        TO_HV_Flip_Degree180,
        WHR_To_Put_Opener,
        Fail,
        Reset
    }

    public class WHRStateMachine
    {
        private string preState;
        public event Func<HVEvent> OnHV_FuncCompleted;
        public event Func<OpenerEvent> OnOpener_FuncCompleted;
        private readonly List<WHRStateTransition> _transitions;

        private readonly Dictionary<WHRState, Func<WHREvent?>> _stateExecutors =
            new Dictionary<WHRState, Func<WHREvent?>>();
        public WHRState CurrentState { get; set; }

        public Action<WHRState, WHRState> OnStateChanged { get; set; }

        public WHRStateMachine()
        {
            CurrentState = WHRState.Idle;

            //OnHV_FuncCompleted += () => Fire(WHREvent.HV_Func_Complete); // 绑定事件
            //OnOpener_FuncCompleted += () => Fire(WHREvent.Opener_Func_Complete); // 绑定事件

            _transitions = new List<WHRStateTransition>
            {
                new WHRStateTransition(
                    WHRState.Idle,
                    WHREvent.WHR_To_Get_Opener,
                    WHRState.WHR_Get_Opener_And_HV_Flip_Degree0
                ),
                new WHRStateTransition(
                    WHRState.WHR_Get_Opener_And_HV_Flip_Degree0,
                    WHREvent.WHR_Get_Opener_And_HV_Flip_Degree0_Complete,
                    WHRState.HV_Unlock_Dirty
                ),
                 new WHRStateTransition(
                    WHRState.HV_Unlock_Dirty,
                    WHREvent.HV_Unlock_Dirty_Complete,
                    WHRState.WHR_Put_HV1
                ),
                new WHRStateTransition(
                    WHRState.WHR_Put_HV1,
                    WHREvent.WHR_Put_HV1_Complete,
                    WHRState.HV_Lock_Dirty
                ),
                new WHRStateTransition(
                    WHRState.HV_Lock_Dirty,
                    WHREvent.HV_Lock_Dirty_Complete,
                    WHRState.WHR_Put_HV2
                ),
                new WHRStateTransition(
                    WHRState.WHR_Put_HV2,
                    WHREvent.WHR_Put_HV2_Complete,
                    WHRState.Idle
                ),
                new WHRStateTransition(
                    WHRState.Idle,
                    WHREvent.TO_HV_Flip_Degree180,
                    WHRState.HV_Flip_Degree180
                ),
                new WHRStateTransition(
                    WHRState.HV_Flip_Degree180,
                    WHREvent.HV_Flip_Degree180_Complete,
                    WHRState.WHR_Get_HV1
                ),
                new WHRStateTransition(
                    WHRState.WHR_Get_HV1,
                    WHREvent.WHR_Get_HV1_Complete,
                    WHRState.HV_UnLock_Clean
                ),
                new WHRStateTransition(
                    WHRState.HV_UnLock_Clean,
                    WHREvent.HV_UnLock_Clean_Complete,
                    WHRState.WHR_Get_HV2
                ),
                new WHRStateTransition(
                    WHRState.WHR_Get_HV2,
                    WHREvent.WHR_Get_HV2_Complete,
                    WHRState.Idle
                ),
                new WHRStateTransition(
                    WHRState.Idle,
                    WHREvent.WHR_To_Put_Opener,
                    WHRState.WHR_Put_Opener
                ),
                new WHRStateTransition(
                    WHRState.WaitOpenerOpen,
                    WHREvent.WaitOpenerOpenComplete,
                    WHRState.WHR_Put_Opener
                ),
                new WHRStateTransition(
                    WHRState.WHR_Put_Opener,
                    WHREvent.WHR_Put_Opener_Complete,
                    WHRState.Idle
                ),
                //new WHRStateTransition(WHRState.Idle, WHREvent.None, WHRState.Idle),
                new WHRStateTransition(WHRState.Error, WHREvent.Reset, WHRState.Idle),
                new WHRStateTransition(
                    WHRState.WHR_Get_Opener_And_HV_Flip_Degree0,
                    WHREvent.Fail,
                    WHRState.Error
                ),
                new WHRStateTransition(WHRState.WHR_Put_HV1, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.WHR_Put_HV2, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.HV_Lock_Dirty, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.HV_Flip_Degree180, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.HV_UnLock_Clean, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.WHR_Get_HV1, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.WHR_Get_HV2, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.WHR_Put_Opener, WHREvent.Fail, WHRState.Error),
                new WHRStateTransition(WHRState.WaitOpenerOpen, WHREvent.Fail, WHRState.Error),
                 new WHRStateTransition(
                    WHRState.HV_Unlock_Dirty,
                    WHREvent.Fail, WHRState.Error),//20251014
            };
        }

        //
        public void RegisterExecutor(WHRState state, Func<WHREvent?> executor)
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
                    Fire(WHREvent.Fail);
                    return;
                }

                Fire(evt.Value);
            }
            else
            {
                Fire(WHREvent.Fail);
            }
        }

        public void TriggerStateAction(WHRState state)
        {
            if (_stateExecutors.TryGetValue(state, out var executor))
            {
                var resultEvent = executor?.Invoke();

                if (resultEvent == null)
                {
                    return;
                }

                if (resultEvent == WHREvent.Fail)
                {
                    Fire(WHREvent.Fail);
                    return;
                }

                Fire(resultEvent.Value);
            }
            else { }
        }

        private void Fire(WHREvent trigger)
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
                //Fire(WHREvent.Fail);
                return;
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
                var state = JsonConvert.DeserializeObject<WHRState>(json);
                if (state != null)
                {
                    CurrentState = state;
                    if (state != WHRState.Idle)
                    {
                        _restoredFromFile = true;
                    }
                }
            }
        }

        public class WHRStateTransition
        {
            public WHRState From { get; }
            public WHREvent Trigger { get; }
            public WHRState To { get; }

            public WHRStateTransition(WHRState from, WHREvent trigger, WHRState to)
            {
                From = from;
                Trigger = trigger;
                To = to;
            }

            public override string ToString() => $"{From} --{Trigger}→ {To}";
        }
    }
}
