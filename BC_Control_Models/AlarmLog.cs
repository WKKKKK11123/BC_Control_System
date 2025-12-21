using PropertyChanged;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    [AddINotifyPropertyChangedInterface]
    [SugarTable("AlarmLog")]
    public class AlarmLog:IInsertTime
    {   
        /// <summary>
        /// 报错代码
        /// </summary>
        public string Code
        {
            get; set;
        }

        /// <summary>
        /// 报错模块
        /// </summary>
        public string Controller
        {
            get; set;
        }

        /// <summary>
        /// 报错触发时间
        /// </summary>
        public DateTime InsertTime
        {
            get; set;
        }

        /// <summary>
        /// 报错清除时间
        /// </summary>
        public DateTime ClearedTime
        {
            get; set;
        }

        /// <summary>
        /// 报错确认时间
        /// </summary>
        public string AcknowledgeTime
        {
            get; set;
        }

        /// <summary>
        /// 注释信息
        /// </summary>
        public string Comment
        {
            get; set;
        }

        /// <summary>
        /// 报警类型，触发/消除
        /// </summary>
        public string AlarmType
        {
            get; set;
        }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator
        {
            get; set;
        }

        /// <summary>
        /// 报警组名 (级别名)
        /// </summary>
        public string GroupName
        {
            get; set;
        }

        /// <summary>
        /// 变量名称
        /// </summary>
        public string VarName
        {
            get; set;
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority
        {
            get; set;
        }
    }
}
