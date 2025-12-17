using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_EFAM.StatusManagement
{
    public enum HVState
    {
        Idle,
        Busy,
        Error,
    }

    public enum HVEvent
    {
        None,
        Fail,
        Reset
    }

    public class HVStateMachine
    {
        public HVState CurrentState { get; set; }

        public Action<HVState, HVState> OnStateChanged { get; set; }

        public HVStateMachine()
        {
            CurrentState = HVState.Idle;
        }
    }
}
