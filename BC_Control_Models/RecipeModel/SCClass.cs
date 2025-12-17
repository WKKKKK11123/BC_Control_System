using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel
{
    [AddINotifyPropertyChangedInterface]
    public class SCClass
    {
        public virtual string Step { get; set; }
        public virtual int Time { get; set; }
        public virtual int DSM { get; set; }
        public virtual bool PumpStop { get; set; }
        public virtual bool Agination { get; set; }
    }
}
