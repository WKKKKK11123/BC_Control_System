using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
     public interface IRevisioninterface
    {
         
         string Name { get; set; }
         string Comment { get; set; }
         string RevisionNo { get; set; }
         string RevComment { get; set; }
    }
}
