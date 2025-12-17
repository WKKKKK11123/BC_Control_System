using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    /// <summary>
    /// 用于标记 PLC 与槽体 Tank 的对应关系
    /// </summary>
    public static class TankMapping
    {
        private static readonly Dictionary<PlcEnum, List<string>> _mapping = new Dictionary<PlcEnum, List<string>>
    {
        { PlcEnum.PLC6, new List<string> { "Tank1", "Tank2" } },
        { PlcEnum.PLC5, new List<string> { "Tank3", "Tank4" } },
        { PlcEnum.PLC4, new List<string> { "Tank5", "Tank6" } },
    };

        public static List<string> GetTankNames(PlcEnum plcEnum)
        {
            return _mapping.ContainsKey(plcEnum) ? _mapping[plcEnum] : new List<string>();
        }
    }
}
