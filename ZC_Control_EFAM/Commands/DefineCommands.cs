using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_EFAM.Commands
{
    public class DefineCommands
    {
        public byte DefineCommand { get; } = 0x01;
        internal byte[] InternalToByteArray(ushort value, ByteOrder order)
        {
            var ret = BitConverter.GetBytes(value);

            if (order == ByteOrder.LittleEndian)
                Array.Reverse(ret);

            return ret;
        }
        public HEX_EN WaferSpecificationSwitchCommand(int station, int thicknessSwitch)
        {
            try
            {
                HEX_EN b;
                ushort commandID = 0x0012;
                byte[] commandTemp = InternalToByteArray(commandID, ByteOrder.LittleEndian);
                byte stationbyte = (byte)station;
                byte thicknessbyte = (byte)thicknessSwitch;
                b = new HEX_EN()
                {
                    f_5t = DefineCommand.ToString("X2"),
                    f_6t = commandTemp[0].ToString("X2"),
                    f_7t = commandTemp[1].ToString("X2"),
                    f_8t = stationbyte.ToString("X2"),
                    f_9t = thicknessbyte.ToString("X2")
                };
                b.f_full = b.f_5t + b.f_6t + b.f_7t + b.f_8t + b.f_9t + b.f_10t + b.f_11t;
                return b;
            }
            catch (Exception EX)
            {
                return null;

            }
        }
    }
}
