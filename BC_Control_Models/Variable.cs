using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    /// <summary>
    /// 变量的实体类
    /// </summary>
    public class Variable : IVariable
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string VarName
        {
            get; set;
        }

        /// <summary>
        /// 起始索引
        /// </summary>
        public ushort Start
        {
            get; set;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get; set;
        }

        /// <summary>
        /// 位偏移或长度
        /// </summary>
        public int OffsetOrLength
        {
            get; set;
        }

        /// <summary>
        /// 所属组名称
        /// </summary>
        public string GroupName
        {
            get; set;
        }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark
        {
            get; set;
        }

        /// <summary>
        /// 是否上升沿报警
        /// </summary>
        public bool PosAlarm
        {
            get; set;
        }

        /// <summary>
        /// 是否下降沿报警
        /// </summary>
        public bool NegAlarm
        {
            get; set;
        }

        /// <summary>
        /// 转换系数
        /// </summary>
        public float Scale
        {
            get; set;
        } = 1.0f;

        /// <summary>
        /// 值偏移
        /// </summary>
        public float Offset
        {
            get; set;
        } = 0.0f;

        /// <summary>
        /// 优先级 0: Error 1：Warning 2：Info
        /// </summary>
        public int Priority
        {
            get; set;
        }

        /// <summary>
        /// 是否操作记录
        /// </summary>
        public bool OperatorLog
        {
            get; set;
        }

        /// <summary>
        /// 是否事件记录
        /// </summary>
        public bool Event
        {
            get; set;
        }


        /// <summary>
        /// 变量值
        /// </summary>
        [ExcelIgnore]
        public object VarValue
        {
            get; set;
        }

        /// <summary>
        /// 更新前的值
        /// </summary>
        [ExcelIgnore]
        public object PreVarValue
        {
            get; set;
        }


        /// <summary>
        /// 日期
        /// </summary>
        [ExcelIgnore]
        public string Date
        {
            get; set;
        }

        /// <summary>
        /// 触发上升沿时间
        /// </summary>
        [ExcelIgnore]
        public DateTime PosAlarmTime
        {
            get; set;
        }
        /// <summary>
        /// 触发下降沿时间
        /// </summary>
        [ExcelIgnore]
        public DateTime NegAlarmTime
        {
            get; set;
        }
    }
}
