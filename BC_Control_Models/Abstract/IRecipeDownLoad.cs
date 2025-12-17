using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public interface IRecipeDownLoad 
    {
        
        string path { get; set; }
        bool DownLoad(string startAddress,PlcEnum plcEnum=PlcEnum.PLC1);
    }
}
