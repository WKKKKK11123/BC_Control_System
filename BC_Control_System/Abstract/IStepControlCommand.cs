using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_System.Abstract
{
    public interface IStepControlCommand
    {
        event Action<ProcessStepEnum> AddStepCommand;
        event Action DeleteStepFuncCommand;
        event Action ChangeStepCommand;
        event Action InsertStepCommand;
        void AddStepFunc(ProcessStepEnum processStepEnum);
        void DeleteStepFunc();
        void ChangeStepFunc();
        void InsertStepFunc();
    }
}
