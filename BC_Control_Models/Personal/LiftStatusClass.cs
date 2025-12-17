using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Personal
{
    [AddINotifyPropertyChangedInterface]
    public class LiftStatusClass
    {
        [Description("Z轴使能信号")]
        public DataClass VerticalEnable { get; set; } = new DataClass();
        [Description("Z轴初始化完成")]
        public DataClass VerticalInit{ get; set; } = new DataClass();
        [Description("Z轴运行中")]
        public DataClass VerticalBusy { get; set; } = new DataClass();
        [Description("Z轴Error")]
        public DataClass VerticalError { get; set; } = new DataClass();
        [Description("Z轴Alarm")]
        public DataClass VerticalAlarm { get; set; } = new DataClass();
        [Description("Z轴正极限Sensor")]
        public DataClass VerticalAxisFLS { get; set; } = new DataClass();
        [Description("Z轴原点Sensor")]
        public DataClass VerticalAxisDOG { get; set; } = new DataClass();
        [Description("Z轴负极限Sensor")]
        public DataClass VerticalAxisRLS { get; set; } = new DataClass();
        [Description("Z轴上定位")]
        public DataClass VerticalPosUp { get; set; } = new DataClass();
        [Description("Z轴横移位")]
        public DataClass VerticalPosSide { get; set; } = new DataClass();
        [Description("Z轴等待位")]
        public DataClass VerticalPosStandby { get; set; } = new DataClass();
        [Description("Z轴下定位")]
        public DataClass VerticalPosDown { get; set; } = new DataClass();


        [Description("X轴使能信号")]
        public DataClass LateralEnable { get; set; } = new DataClass();
        [Description("X轴初始化完成")]
        public DataClass LateralInit { get; set; } = new DataClass();
        [Description("X轴运行中")]
        public DataClass LateralBusy { get; set; } = new DataClass();
        [Description("X轴Error")]
        public DataClass LateralError { get; set; } = new DataClass();
        [Description("X轴Alarm")]
        public DataClass LateralAlarm { get; set; } = new DataClass();
        [Description("X轴正极限Sensor")]
        public DataClass LateralAxisFLS { get; set; } = new DataClass();
        [Description("X轴原点Sensor")]
        public DataClass LateralAxisDOG { get; set; } = new DataClass();
        [Description("X轴负极限Sensor")]
        public DataClass LateralAxisRLS { get; set; } = new DataClass();
        [Description("X轴Station1到位")]
        public DataClass LateralPos1 { get; set; } = new DataClass();
        [Description("X轴Station2到位")]
        public DataClass LateralPos2 { get; set; } = new DataClass();
        [Description("工位有料信号")]
        public DataClass LFRIswafer { get; set; }


    }
}
