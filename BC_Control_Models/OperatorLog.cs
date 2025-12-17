using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    [SugarTable("OperatorLog")]
    public class OperatorLog:IInsertTime
    {
        ///// <summary>
        ///// 日期
        ///// </summary>
        //public string Date
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// 时间
        ///// </summary>
        //public string Time
        //{
        //    get; set;
        //}
        public DateTime InsertTime { get; set ; }
        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue
        {
            get; set;
        }

        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue
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
        /// 操作者
        /// </summary>
        public string Operator
        {
            get; set;
        }

        /// <summary>
        /// 事件类型
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
        
    }
}
