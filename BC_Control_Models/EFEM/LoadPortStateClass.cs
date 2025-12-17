using PropertyChanged;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZC_Control_System
{
    [AddINotifyPropertyChangedInterface]
    [SugarTable("ActualData_BufferInfo")]
    public class LoadPortState
    {
        public int Station { get; set; } //当前工位
        public string CarrierID {  get; set; } 
        public int ActState { get; set; }//当前工位状态
        public Guid FoupGuidD { get; set; }
        public bool ReverseState { get; set; } //Out of service in service  
        public bool AccessMode { get; set; } //Manual Auto
        public bool CarrierState { get; set; }
    }
}
