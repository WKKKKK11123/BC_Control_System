using System;
using System.Collections.Generic;
using thinger.DataConvertLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public interface IPLCValue : IPLCSelect
    {
        string ParameterName { get; set; }
        string ValueAddress { get; set; }
        string Value { get; set; }
        DataType DataType { get; set; }
        float Scale
        {
            get; set;
        }
        /// <summary>
        /// 位偏移或长度
        /// </summary>
        int OffsetOrLength
        {
            get; set;
        }
    }
}
