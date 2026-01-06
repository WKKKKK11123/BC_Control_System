using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    /// <summary>
    /// 所有窗体的枚举， 小于临界窗体的位固定窗体
    /// </summary>
    public enum FormNames
    {
        集中监控,
        临界窗体,
        参数设置,
        配方管理,
        报警追溯,
        历史趋势,
        用户管理,
    }

    public enum StoreArea
    {
        线圈,
        寄存器
    }
    public enum PlcEnum
    {
        [Description("Robot")]
        PLC1,
        [Description("Ag_1")]
        PLC2,
        [Description("Ag_2")]
        PLC3,
        [Description("Ni_1")]
        PLC4,
        [Description("Ni_2")]
        PLC5,
        [Description("Ti_1")]
        PLC6,
        [Description("LPD")]
        PLC7,
        [Description("CC")]
        PLC8,
        [Description("Ag_1/MIX")]
        PLC9,
        [Description("Ag_2/MIX")]
        PLC10,
        [Description("Ni_1/MIX")]
        PLC11,
        [Description("Ni_2/MIX")]
        PLC12,
        [Description("LPD/MIX")]
        PLC13
    }
    public enum TempratureEnum
    {
        pattern1,
        pattern2,
        pattern3,
    }
    public enum BathNameEnum
    {
        Ag_1 = 1,
        QDR_1 = 2,
        Ag_2 = 3,
        QDR_2 = 4,
        Ni_1 = 5,
        QDR_3 = 6,
        Ni_2 = 7,
        QDR_4 = 8,
        Ti_1 = 9,
        QDR_5 = 10,
        LPD_1 = 11,
    }
    public enum DIWEnum
    {
        None,
        SlowLeak,
        Low,
        High
    }
    public enum BlowPatternEnum
    {
        None,
        Both,
        Blow_1,
        Blow_2
    }
    public enum LFREnum
    {
        None,
        UP,
        Dwon
    }

    public enum StationEnum
    {
        ProcessTank,
        BufferTank,
        Mechanical
    }
    public enum ProcessStepEnum
    {
        PreStep,
        Step,
        PostStep
    }

}
