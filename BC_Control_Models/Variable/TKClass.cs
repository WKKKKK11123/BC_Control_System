using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.Personal;

namespace BC_Control_Models
{
    public class TKClass
    {
        public TKClass() 
        { 
        
        }
        public string ModuleName { get; set; }
        public int TankNo { get; set; }
        public List<BatchStatusCollection> BatchStatussCollections { get; set; }
        public List<DataCollection> DataCollections { get; set; }
        public List<StatusCollection> StatusCollections { get; set; }
        public List<OprationCollection> OprationCollections { get; set; }
        public List<ParameterCollection> ParameterCollections { get; set; }

    }
}
