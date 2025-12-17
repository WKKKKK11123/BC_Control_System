using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC_Control_Models.eap;

namespace BC_Control_Models.EAP
{
    public class XHuaiEAPClass
    {
        private string pjPath;
        private string cjPath;
        private CancellationTokenSource cts;
        
        public int EAPMode { get; set; }
        public RootObject CJS { get; set; }
        public ProcessJobClass ProcessJobEntity { get; set; }
        public List<string> CJCollection { get; set; }
        public List<EAPStatusClass> EAPStatusCollection { get; set; }
        public XHuaiEAPClass()
        {
            EAPStatusCollection=new List<EAPStatusClass>();
            for (int i = 0; i < 20; i++)
            {
                EAPStatusCollection.Add(new EAPStatusClass());
            }
        }



    }
}
