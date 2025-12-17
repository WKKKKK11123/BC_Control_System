using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using thinger.DataConvertLib;

namespace BC_Control_Models
{
    [Serializable]
    public class ParameterModel : IPLCValue
    {
        [ExcelColumnName("No")]
        public int No { get; set; } = 0;

        [ExcelColumnName("Group")]
        public string Group { get; set; } = "";

        [ExcelColumnName("Type")]
        public DataType DataType { get; set; }

        [ExcelColumnName("Code")]
        public string Code { get; set; } = "";

        [ExcelColumnName("参数名称")]
        public string ParameterName { get; set; } = "";

        [ExcelColumnName("Scale")]
        public float Scale { get; set; }

        [ExcelColumnName("Editable")]
        public string Editable { get; set; } = "";

        [ExcelColumnName("生效方式")]
        public string Apply { get; set; } = "";

        [ExcelColumnName("单位")]
        public string Unit { get; set; } = "";

        [ExcelColumnName("地址")]
        public string ValueAddress { get; set; } = "";

        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; } = PlcEnum.PLC1;
        [ExcelColumnName("UpLimit")]
        public float UpLimit{ get; set; }
        [ExcelColumnName("LowLimit")]
        public float LowLimit { get; set; }

        [ExcelIgnore]
        public string Value { get; set; } = "";

        [ExcelIgnore]
        public bool IsChange { get; set; }
        [ExcelColumnName("长度或补偿值")]
        public int OffsetOrLength { get; set; }
        
    }
}
