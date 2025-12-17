using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class OprationCollection : IVariable,IPLCSelect
    {
        [ExcelColumnName("参数名称")]
        public string Remark { get ; set ; }
        [ExcelColumnName("地址")]
        public string VarName { get ; set ; }
        [ExcelColumnName("设定值地址")]
        public string SettingValueAddress { get ; set ; }
        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; } = PlcEnum.PLC1;
        [ExcelIgnore]
        public int OffsetOrLength { get; set;} = 0;
        [ExcelIgnore]
        public float Scale { get; set ; } = 1.0f;
        [ExcelIgnore]
        public string DataType { get; set; } = "bool";
        [ExcelIgnore]
        public float Offset { get; set; } = 1;
        [ExcelIgnore]
        public object VarValue { get; set; }
        public DataClass ToDataClass()
        {
            return new DataClass
            {
                ParameterName = Remark,              
                SettingValueAddress = SettingValueAddress,
                ValueAddress = VarName,
                PLC = PLC,
            };
        }
    }
}
