using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace ZC_Control_System.EFAMAction
{
    public class EFAMMessage
    {
        public EFAMMessage()
        {
            dictionary = new List<LoadPortState>();
            StorageCollectionList=new List<StorageCollection>();
            dictionary.Add(new LoadPortState() { Station = 1 });
            dictionary.Add(new LoadPortState() { Station = 2 });
            dictionary.Add(new LoadPortState() { Station = 3 });
            dictionary.Add(new LoadPortState() { Station = 4 });

        }
        public int BTRState { get; set; }
        public List<LoadPortState> dictionary { get; set; }
        public List<StorageCollection> StorageCollectionList { get; set; }
    }
}
