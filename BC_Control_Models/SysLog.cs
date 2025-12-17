using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class SysLog
    {
        /// <summary>
        /// 插入时间
        /// </summary>
        public string InsertTime
        {
            get;set;
        }

        /// <summary>
        /// 盒号
        /// </summary>
        public string Case_State
        {
            get; set;
        }

        /// <summary>
        /// 层号
        /// </summary>
        public string Slot_State
        {
            get; set;
        }

        /// <summary>
        /// 流程配方
        /// </summary>
        public string Tool_State
        {
            get; set;
        }

        /// <summary>
        /// 自动状态
        /// </summary>
        public string Auto_State
        {
            get; set;
        }

        /// <summary>
        /// 原点状态
        /// </summary>
        public string Init_State
        {
            get; set;
        }

        /// <summary>
        /// 报警状态
        /// </summary>
        public string Alarm_State
        {
            get; set;
        }

        /// <summary>
        /// 停止状态
        /// </summary>
        public string Stop_State
        {
            get; set;
        }

        /// <summary>
        /// 清洗状态
        /// </summary>
        public string Rinse_State
        {
            get; set;
        }

        /// <summary>
        /// 匀胶状态
        /// </summary>
        public string Coat_State
        {
            get; set;
        }

        /// <summary>
        ///热盘1状态
        /// </summary>
        public string HP1_State
        {
            get; set;
        }

        /// <summary>
        ///热盘2状态
        /// </summary>
        public string HP2_State
        {
            get; set;
        }

        /// <summary>
        ///冷盘1状态
        /// </summary>
        public string CP1_State
        {
            get; set;
        }

        /// <summary>
        ///冷盘2状态
        /// </summary>
        public string CP2_State
        {
            get; set;
        }

        /// <summary>
        ///对中状态
        /// </summary>
        public string Aligner_State
        {
            get; set;
        }

        /// <summary>
        ///上手指状态
        /// </summary>
        public string RobotUp_State
        {
            get; set;
        }

        /// <summary>
        ///下手指状态
        /// </summary>
        public string RobotDown_State
        {
            get; set;
        }

        /// <summary>
        ///12寸1状态
        /// </summary>
        public string Inch12_1State
        {
            get; set;
        }
        /// <summary>
        ///12寸2状态
        /// </summary>
        public string Inch12_2State
        {
            get; set;
        }

        /// <summary>
        ///8寸1状态
        /// </summary>
        public string Inch8_1State
        {
            get; set;
        }
        /// <summary>
        ///8寸2状态
        /// </summary>
        public string Inch8_2State
        {
            get; set;
        }


        /// <summary>
        ///12寸3状态
        /// </summary>
        public string Inch12_3State
        {
            get; set;
        }

        /// <summary>
        ///12寸4状态
        /// </summary>
        public string Inch12_4State
        {
            get; set;
        }

        /// <summary>
        ///8寸3状态
        /// </summary>
        public string Inch8_3State
        {
            get; set;
        }

        /// <summary>
        ///8寸4状态
        /// </summary>
        public string Inch8_4State
        {
            get; set;
        }

        /// <summary>
        ///HP1温度
        /// </summary>
        public string HP1_Temp
        {
            get; set;
        }

        /// <summary>
        ///HP2温度
        /// </summary>
        public string HP2_Temp
        {
            get; set;
        }

        /// <summary>
        ///Rinse配方
        /// </summary>
        public string Rinse_Rec
        {
            get; set;
        }

        /// <summary>
        ///RinseCH配方
        /// </summary>
        public string Rinse_CHRec
        {
            get; set;
        }

        /// <summary>
        ///Rinse层号
        /// </summary>
        public string Rinse_Slot
        {
            get; set;
        }

        /// <summary>
        ///Rinse盒号
        /// </summary>
        public string Rinse_No
        {
            get; set;
        }

        /// <summary>
        ///Coat配方
        /// </summary>
        public string Coat_Rec
        {
            get; set;
        }

        /// <summary>
        ///CoatCH配方
        /// </summary>
        public string Coat_CHRec
        {
            get; set;
        }

        /// <summary>
        ///Coat层号
        /// </summary>
        public string Coat_Slot
        {
            get; set;
        }

        /// <summary>
        ///Coat盒号
        /// </summary>
        public string Coat_No
        {
            get; set;
        }

        /// <summary>
        ///CP1配方
        /// </summary>
        public string CP1_Rec
        {
            get; set;
        }

        /// <summary>
        ///CP1CH配方
        /// </summary>
        public string CP1_CHRec
        {
            get; set;
        }

        /// <summary>
        ///CP1层号
        /// </summary>
        public string CP1_Slot
        {
            get; set;
        }

        /// <summary>
        ///CP1盒号
        /// </summary>
        public string CP1_No
        {
            get; set;
        }

        /// <summary>
        ///CP2配方
        /// </summary>
        public string CP2_Rec
        {
            get; set;
        }

        /// <summary>
        ///CP2CH配方
        /// </summary>
        public string CP2_CHRec
        {
            get; set;
        }

        /// <summary>
        ///CP2层号
        /// </summary>
        public string CP2_Slot
        {
            get; set;
        }

        /// <summary>
        ///CP2盒号
        /// </summary>
        public string CP2_No
        {
            get; set;
        }

        /// <summary>
        ///HP1配方
        /// </summary>
        public string HP1_Rec
        {
            get; set;
        }

        /// <summary>
        ///HP1CH配方
        /// </summary>
        public string HP1_CHRec
        {
            get; set;
        }

        /// <summary>
        ///HP1层号
        /// </summary>
        public string HP1_Slot
        {
            get; set;
        }

        /// <summary>
        ///HP1盒号
        /// </summary>
        public string HP1_No
        {
            get; set;
        }

        /// <summary>
        ///HP2配方
        /// </summary>
        public string HP2_Rec
        {
            get; set;
        }

        /// <summary>
        ///HP2CH配方
        /// </summary>
        public string HP2_CHRec
        {
            get; set;
        }

        /// <summary>
        ///HP2层号
        /// </summary>
        public string HP2_Slot
        {
            get; set;
        }

        /// <summary>
        ///HP2盒号
        /// </summary>
        public string HP2_No
        {
            get; set;
        }
    }
}
