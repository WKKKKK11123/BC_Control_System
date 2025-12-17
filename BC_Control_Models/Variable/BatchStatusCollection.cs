using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class BatchStatusCollection : IVariable, IPLCSelect
    {
        [ExcelColumnName("DataType")]
        public string DataType { get; set; }
        [ExcelColumnName("参数名称")]
        public string Remark { get; set; }
        [ExcelColumnName("地址")]
        public string VarName { get; set; }
        [ExcelColumnName("特性值")]
        public string StatusAttibute { get; set; }
        [ExcelColumnName("字符串长度")]
        public int OffsetOrLength { get; set; } = 0;
        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; } = PlcEnum.PLC1;        
        [ExcelIgnore]
        public float Scale { get; set; } = 1.0f;
        [ExcelIgnore]
        public float Offset { get; set; } = 1;
        [ExcelIgnore]
        public object VarValue { get; set; }
        public StatusClass ToStatusClass()
        {
            return new StatusClass()
            {
                ParameterName = Remark,
                //Unit = Unit,
                ValueAddress = VarName,
                StatusArributeString = StatusAttibute,
                PLC = PLC,
            };
        }
    }
}
