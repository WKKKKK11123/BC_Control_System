using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.Service
{
    public class LogCommandService : ILogCommandService
    {
        public event Action<DateTime, DateTime> SelectTimeCommandExecuted;
        public event Func<List<object>> GetListCommandExecuted;
        public event Func<Type> GetTypeCommandExecuted;

        public List<object> ExcuteGetList()
        {
            return GetListCommandExecuted?.Invoke();
        }

        public Type ExcuteGetType()
        {
            return GetTypeCommandExecuted?.Invoke();
        }

        public void ExecuteSelectTime(DateTime arg1, DateTime arg2) => SelectTimeCommandExecuted?.Invoke(arg1, arg2);
       

       
    }
}
