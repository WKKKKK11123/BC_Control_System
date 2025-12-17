using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BC_Control_CustomControl.Common
{
    public class CheckBase
    {
        public string Description {  get; set; }
        public bool IsChecked {  get; set; }
        public SolidColorBrush BackColor { get; set; }
    }
}
