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
        [Description("LPD")]
        PLC2,
        [Description("QDR/CC")]
        PLC3,
        [Description("NMP")]
        PLC4,
        [Description("EKC")]
        PLC5,
        PLC6,
        PLC7,
        PLC8,
        PLC9,
        PLC10,
        PLC11,
        PLC12,
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
        //SYS9070_1 = 1,
        //SYS9070_2 = 2,
        //NMP_3 = 3,
        //NMP_4 = 4,
        //IPA_5 = 5,
        //IPA_6 = 6,
        //QDR_7 = 7,
        //MGD_9 = 8,
        EKC_1 = 1,
        EKC_2 = 2,
        NMP_3 = 3,
        QDR_4 = 4,
        LPD = 5
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
