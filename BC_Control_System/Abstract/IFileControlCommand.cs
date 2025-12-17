using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.Abstract
{
    public interface IFileControlCommand
    {
        event Action SaveCommand;
        event Action NewCommand;
        event Action LoadCommand;
        event Action DeleteCommand;

        void SaveFunc();
        void NewFunc();
        void LoadFunc();
        void DeleteFunc();
    }
}
