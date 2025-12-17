using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.Log
{
    public class BaseEntity
    {       
        public DateTime InsertTime { get; set; }=DateTime.Now;
        public string BatchID { get; set; } = "";
        public string Temp1 { get; set; } = "";

    }
}
