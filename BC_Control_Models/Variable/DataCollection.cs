using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class DataCollection : IVariable, IPLCSelect
    {

        [ExcelColumnName("参数名称")]
        public string Remark { get; set; }
        [ExcelColumnName("地址")]
        public string VarName { get; set; }
        [ExcelColumnName("设置值地址")]
        public string SettingValueAddress { get; set; }
        [ExcelColumnName("DataType")]
        public string DataType { get; set; }
        [ExcelColumnName("补偿值")]
        public float Offset { get; set; }
        [ExcelColumnName("字符串长度")]
        public int OffsetOrLength { get; set; }
        [ExcelColumnName("比例尺")]
        public float Scale { get; set; }
        [ExcelColumnName("单位")]
        public string Unit { get; set; }
        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; }
        [ExcelIgnore]
        public object VarValue { get; set; }
        [ExcelIgnore]
        public object SettingVarValue { get; set; }

        public DataClass ToDataClass()
        {
            return new DataClass
            {
                ParameterName = Remark,
                Unit = Unit,
                SettingValueAddress = SettingValueAddress,
                ValueAddress = VarName,
                PLC = PLC,
            };
        }
    }
}
