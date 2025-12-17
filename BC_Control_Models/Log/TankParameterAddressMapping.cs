using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log 
{
    public static class TankParameterAddressMapping
    {
        public static readonly Dictionary<(PlcEnum plc, string tankName), List<(string ParamName, string Address)>> ParameterAddresses
            = new Dictionary<(PlcEnum, string), List<(string, string)>>
            {
                [(PlcEnum.PLC6, "Tank1")] = new List<(string, string)>
            {
            ("CoolingTemp", "D1000"),
            ("ExhaustPressure1_1", "D1001"),
            ("ExhaustPressure1_2", "D1002"),
            ("FlowRate1", "D1003"),
            ("FFUDiffPressure1", "D1004"),
            ("ProcessTemp", "D1005"),
            ("U_sonic", "D1006"),
            ("WaterTankTemp", "D1007")
            },
                [(PlcEnum.PLC6, "Tank2")] = new List<(string, string)>
            {
            ("CoolingTemp", "D1100"),
            ("ExhaustPressure1_1", "D1101"),
            ("ExhaustPressure1_2", "D1102"),
            ("FlowRate1", "D1103"),
            ("FFUDiffPressure1", "D1104"),
            ("ProcessTemp", "D1105"),
            ("U_sonic", "D1106"),
            ("WaterTankTemp", "D1107")
            },
                [(PlcEnum.PLC5, "Tank3")] = new List<(string, string)>
            {
            ("CoolingTemp", "D1000"),
            ("ExhaustPressure1_1", "D1001"),
            ("ExhaustPressure1_2", "D1002"),
            ("FlowRate1", "D1003"),
            ("FFUDiffPressure1", "D1004"),
            ("ProcessTemp", "D1005"),
            ("U_sonic", "D1006"),
            ("WaterTankTemp", "D1007")
            },
                [(PlcEnum.PLC5, "Tank4")] = new List<(string, string)>
            {
            ("CoolingTemp", "D1100"),
            ("ExhaustPressure1_1", "D1101"),
            ("ExhaustPressure1_2", "D1102"),
            ("FlowRate1", "D1103"),
            ("FFUDiffPressure1", "D1104"),
            ("ProcessTemp", "D1105"),
            ("U_sonic", "D1106"),
            ("WaterTankTemp", "D1107")
            },

                [(PlcEnum.PLC4, "Tank5")] = new List<(string, string)>
            {
            ("CoolingTemp", "D1000"),
            ("ExhaustPressure1_1", "D1001"),
            ("ExhaustPressure1_2", "D1002"),
            ("FlowRate1", "D1003"),
            ("FFUDiffPressure1", "D1004"),
            ("ProcessTemp", "D1005"),
            ("U_sonic", "D1006"),
            ("WaterTankTemp", "D1007")
            },
                [(PlcEnum.PLC4, "Tank6")] = new List<(string, string)>
            {
            ("CoolingTemp", "D1100"),
            ("ExhaustPressure1_1", "D1101"),
            ("ExhaustPressure1_2", "D1102"),
            ("FlowRate1", "D1103"),
            ("FFUDiffPressure1", "D1104"),
            ("ProcessTemp", "D1105"),
            ("U_sonic", "D1106"),
            ("WaterTankTemp", "D1107")
            },
            };
    }
}
