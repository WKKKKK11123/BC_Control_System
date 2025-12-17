using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models.RecipeModel
{
    [AddINotifyPropertyChangedInterface]
    public class RecipeClassBase<T> : ILIftSpeedInterface, IRevisioninterface
    {
        public string Name { get; set; } = "MGD Recipe";
        public string Comment { get; set; } = "V1.0";
        public string RevisionNo { get; set; } = "V1.0";
        public string RevComment { get; set; } = "";
        public  TempratureEnum Temprature { get; set; } = TempratureEnum.pattern1;
        public short LiftSpeedPre { get; set; } = 210;
        public short LiftSpeedDown1 { get; set; } = 110;
        public  short LiftSpeedDown2 { get; set; } = 25;
        public  short LiftSpeedUp1 { get; set; } = 25;
        public  short LiftSpeedUp2 { get; set; } = 25;
        public List<T> PreSCRecipeList { get; set; } = new List<T>();
        public List<T> SCRecipeList { get; set; } = new List<T>();
        public List<T> PostSCRecipeList { get; set; } = new List<T>();
    }
}
