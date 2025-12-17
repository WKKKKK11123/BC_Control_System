using Prism.Commands;
using Prism.Mvvm;
using PropertyChanged;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZC_Control_EFAM;
using ZC_Control_EFAM.ProcessControl;

namespace BC_Control_System.ViewModel.Opration
{
    [AddINotifyPropertyChangedInterface]
    public class LoadPortTrackingViewModel : BindableBase
    {
        public List<StationStateBase> loadPortStates { get; set; }
        public LoadPortTrackingViewModel( ProcessControl processControl)
        {   
            loadPortStates = processControl.eFAM_Data.Loadport_Data.Select(src => (StationStateBase)src).ToList();
        }
    }
}
