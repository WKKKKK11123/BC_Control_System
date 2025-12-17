using NPOI.POIFS.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System
{
    public interface ILogCommandService
    {
        event Action<DateTime,DateTime> SelectTimeCommandExecuted;
        event Func<List<object>> GetListCommandExecuted;
        event Func<Type> GetTypeCommandExecuted;

        void ExecuteSelectTime(DateTime arg1, DateTime arg2);
        List<object> ExcuteGetList();
        Type ExcuteGetType();
    }
}
