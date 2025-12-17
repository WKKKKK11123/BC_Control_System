using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public interface ILIftSpeedInterface
    {
         short LiftSpeedPre { get; set; }
         short LiftSpeedDown1 { get; set; }
         short LiftSpeedDown2 { get; set; }
         short LiftSpeedUp1 { get; set; }
         short LiftSpeedUp2 { get; set; }
    }
}
