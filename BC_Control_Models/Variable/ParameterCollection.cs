using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class ParameterCollection : IVariable,IPLCSelect
    {
        [ExcelColumnName("No")]
        public int No { get; set; } = 0;
        [ExcelColumnName("DataType")]
        public string DataType { get ; set ; }
        [ExcelColumnName("补偿值")]
        public float Offset { get ; set ; }
        [ExcelColumnName("参数名称")]
        public string Remark { get ; set ; }
        [ExcelColumnName("地址")]
        public string VarName { get ; set ; }
        
        [ExcelColumnName("比例尺")]
        public float Scale { get ; set ; }       

        [ExcelColumnName("Group")]
        public string Group { get; set; } = "";
      
        [ExcelColumnName("Code")]
        public string Code { get; set; } = ""; 

        [ExcelColumnName("Editable")]
        public string Editable { get; set; } = "";

        [ExcelColumnName("生效方式")]
        public string Apply { get; set; } = "";

        [ExcelColumnName("单位")]
        public string Unit { get; set; } = "";

        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; } = PlcEnum.PLC1;

        [ExcelIgnore]
        public bool IsChange { get; set; }
        [ExcelIgnore]
        public object VarValue { get; set; }
        [ExcelIgnore]
        public int OffsetOrLength { get; set; } = 0;
    }
}
