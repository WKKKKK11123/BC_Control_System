using MiniExcelLibs.Attributes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.DataConvertLib;

namespace BC_Control_Models
{
    [AddINotifyPropertyChangedInterface]
    public class DataClass:IPLCValue
    {
        [ExcelColumnName("状态名称")]
        public string ParameterName { get; set; } = "";
        [ExcelColumnName("当前值地址")]
        public string ValueAddress { get; set; } = "";
        [ExcelColumnName("设定值地址（可缺省）")]
        public string SettingValueAddress { get; set; } = "";
        [ExcelColumnName("单位")]
        public string Unit { get; set; } = "";
        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; } = PlcEnum.PLC1;
        public string SettingValue { get; set; }
        public string Value { get; set; }
        [ExcelColumnName("Type")]
        public DataType DataType { get; set; }
        [ExcelColumnName("Scale")]
        public float Scale { get; set; }
        [ExcelColumnName("OffsetOrLength")]
        public int OffsetOrLength { get; set; }
        public bool Visible { get; set; }
    }
}
