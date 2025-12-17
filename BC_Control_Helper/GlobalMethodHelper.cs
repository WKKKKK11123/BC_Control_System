using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Helper
{
    public class GlobalMethodHelper
    {
        
        private static GlobalMethodHelper _instance;

        public static GlobalMethodHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GlobalMethodHelper();
                return _instance;
            }
        }
        public short[] stringTointArray(string s, int specifiedLength)
        {
            try
            {
                string str = s;
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                int lengthToProcess = Math.Min(byteArray.Length, specifiedLength * 2);
                byte[] truncatedArray = new byte[lengthToProcess];
                Array.Copy(byteArray, truncatedArray, lengthToProcess);
                short[] shortArray = new short[specifiedLength];
                for (int i = 0; i < lengthToProcess; i += 2)
                {
                    // 组合 2 个字节为一个 short
                    short value = 0;
                    for (int j = 0; j < 2 && (i + j) < byteArray.Length; j++)
                    {
                        value |= (short)(byteArray[i + j] << (8 * j)); // 按字节合并成一个 short
                    }
                    shortArray[i / 2] = value;
                }
                return shortArray;
            }
            catch (Exception ee)
            {

                return new short[specifiedLength];
            }
        }
        public short[] stringTointArray(float f)
        {
            try
            {
                float floatNumber = f;  // 示例浮点数

                // 1. 将浮点数转换为字节数组（4字节）
                byte[] byteArray = BitConverter.GetBytes(floatNumber);

                // 2. 将字节数组转换为short数组（每2个字节转换为一个short）
                short[] shortArray = new short[2];  // 保证足够的空间来存储short

                for (int i = 0; i < byteArray.Length; i += 2)
                {
                    short value = 0;
                    for (int j = 0; j < 2 && (i + j) < byteArray.Length; j++)
                    {
                        value |= (short)(byteArray[i + j] << (8 * j));  // 按字节合并成一个short
                    }
                    shortArray[i / 2] = value;
                }
                return shortArray;
            }
            catch (Exception)
            {

                throw;
            }



        }

        // 模拟发送数据到 PLC 的方法


    }
}
