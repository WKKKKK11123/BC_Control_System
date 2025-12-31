using MiniExcelLibs.Attributes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.DataConvertLib;

namespace BC_Control_Models
{
    
    public class StatusClass : IPLCValue , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _value = "0";
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }


        [ExcelColumnName("状态名称")]
        public string ParameterName { get; set; } = "";
        [ExcelColumnName("单位")]
        public string Unit { get; set; } = "";
        [ExcelColumnName("地址")]
        public string ValueAddress { get; set; } = "";
        //1:Not Ready,2:Ready,3:Processing
        [ExcelColumnName("特性值")]
        public string StatusArributeString { get; set; } = "";
        [ExcelColumnName("PLC选择")]
        public PlcEnum PLC { get; set; } = PlcEnum.PLC1;
        //[ExcelIgnore]
        //public string Value { get; set; } = "0";
        [ExcelIgnore]
        public string ActualValue { get; set; } = "0";
        [ExcelColumnName("Type")]
        public DataType DataType { get; set; }
        [ExcelColumnName("Scale")]
        public float Scale { get; set; }
        [ExcelColumnName("OffsetOrLength")]
        public int OffsetOrLength { get; set; }
        public bool Visible { get; set; } = true;

        [ExcelIgnore]
        public Dictionary<int, string> StatusArribute = new Dictionary<int, string>();

    }
}
