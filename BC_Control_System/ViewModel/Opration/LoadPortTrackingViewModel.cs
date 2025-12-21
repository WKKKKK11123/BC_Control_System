using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class LoadPortTrackingViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<StationStateBase> _loadPortStates;
        public LoadPortTrackingViewModel( ProcessControl processControl)
        {   
            LoadPortStates = processControl.eFAM_Data.Loadport_Data.Select(src => (StationStateBase)src).ToList();
        }
    }
}
