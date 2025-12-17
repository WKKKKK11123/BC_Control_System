using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_System
{
    public class EFAMEnum
    {
    }
    public enum StorageEnum
    {
        ModuleDisable = -1,
        Free = 0,
        Busy = 1,
    }
    public enum LoadPortEnum
    {
        ModuleDisabe = -1,
        Free = 0,
        WaitingForUp = 1,
        Busy = 2,
    }
    public enum CarrierAccessingStatus
    {
        NOTACCESSED = 0, 
        INACCESS,
        CARRIERCOMPLETE,
        CARRIERSTOPPED,
    }
}
