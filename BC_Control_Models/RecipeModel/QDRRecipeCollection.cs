using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel
{
    public class QDRRecipeCollection
    {
        public string Step { get; set; }
        public int Time { get; set; }
        public bool SlowLeak { get; set; }
        public bool QDR { get; set; }
        public bool Shower { get; set; }
        public bool Bubble { get; set; }
        public bool Agitation { get; set; }
        public bool ResistivityCheck { get; set; }
        public bool N2Bubble { get; set; }
    }
}
