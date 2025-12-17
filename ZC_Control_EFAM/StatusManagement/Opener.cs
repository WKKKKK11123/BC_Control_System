using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZC_Control_EFAM.StatusManagement
{
    public enum OpenerState
    {
        Idle,
        Opener_Open,

        Opener_Close,

        Opener_Open_Get,
        Wait_FuncComplete,
        Error,
    }

    public enum OpenerEvent
    {
        None,
        Opener_Open_Complete,
        Opener_Close_Complete,

        To_Open_Status,
        Fail,
        Reset
    }

    public class OpenerStateMachine
    {
        private string preState;
        public event Action OnFunctionCompleted;
        private readonly List<OpenerStateTransition> _transitions;

        private readonly Dictionary<OpenerState, Func<OpenerEvent?>> _stateExecutors =
            new Dictionary<OpenerState, Func<OpenerEvent?>>();
        public OpenerState CurrentState { get; set; }

        public Action<OpenerState, OpenerState> OnStateChanged { get; set; }

        public OpenerStateMachine()
        {
            CurrentState = OpenerState.Idle;
            //OnFunctionCompleted += () => Fire(OpenerEvent.WHR_Func_Complete); // 绑定事件
            _transitions = new List<OpenerStateTransition>
            {
                new OpenerStateTransition(
                    OpenerState.Idle,
                    OpenerEvent.To_Open_Status,
                    OpenerState.Opener_Open
                ),
                new OpenerStateTransition(
                    OpenerState.Opener_Open,
                    OpenerEvent.Opener_Open_Complete,
                    OpenerState.Opener_Close
                ),
                new OpenerStateTransition(
                    OpenerState.Opener_Open_Get,
                    OpenerEvent.Opener_Open_Complete,
                    OpenerState.Wait_FuncComplete
                ),
                new OpenerStateTransition(
                    OpenerState.Opener_Close,
                    OpenerEvent.Opener_Close_Complete,
                    OpenerState.Idle
                ),
                //new OpenerStateTransition(OpenerState.Idle, OpenerEvent.None, OpenerState.Idle),
                new OpenerStateTransition(OpenerState.Error, OpenerEvent.Reset, OpenerState.Idle),
                new OpenerStateTransition(
                    OpenerState.Opener_Open,
                    OpenerEvent.Fail,
                    OpenerState.Error
                ),
                new OpenerStateTransition(
                    OpenerState.Opener_Close,
                    OpenerEvent.Fail,
                    OpenerState.Error
                ),
                new OpenerStateTransition(
                    OpenerState.Opener_Open_Get,
                    OpenerEvent.Fail,
                    OpenerState.Error
                ),
            };
        }

        //
        public void RegisterExecutor(OpenerState state, Func<OpenerEvent?> executor)
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
                    Fire(OpenerEvent.Fail);
                    return;
                }

                Fire(evt.Value);
            }
            else
            {
                Fire(OpenerEvent.Fail);
            }
        }

        public void TriggerStateAction(OpenerState state)
        {
            if (_stateExecutors.TryGetValue(state, out var executor))
            {
                var resultEvent = executor?.Invoke();

                if (resultEvent == null)
                {
                    return;
                }

                if (resultEvent == OpenerEvent.Fail)
                {
                    Fire(OpenerEvent.Fail);
                    return;
                }

                Fire(resultEvent.Value);
            }
            else { }
        }

        private void Fire(OpenerEvent trigger)
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
                //Fire(OpenerEvent.Fail);
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
                var state = JsonConvert.DeserializeObject<OpenerState>(json);
                if (state != null)
                {
                    CurrentState = state;
                    if (state != OpenerState.Idle)
                    {
                        _restoredFromFile = true;
                    }
                }
            }
        }

        public class OpenerStateTransition
        {
            public OpenerState From { get; }
            public OpenerEvent Trigger { get; }
            public OpenerState To { get; }

            public OpenerStateTransition(OpenerState from, OpenerEvent trigger, OpenerState to)
            {
                From = from;
                Trigger = trigger;
                To = to;
            }

            public override string ToString() => $"{From} --{Trigger}→ {To}";
        }
    }
}
