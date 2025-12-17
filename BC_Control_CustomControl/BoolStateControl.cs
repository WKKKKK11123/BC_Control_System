using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BC_Control_CustomControl
{
    public class BoolStateControl : Control
    {
        static BoolStateControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                        typeof(BoolStateControl),
                        new FrameworkPropertyMetadata(typeof(BoolStateControl))
                        );
        }
    }
}
