using BC_Control_Models.Personal;
using BC_Control_Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Status.IOViewModel.Machine
{
    [AddINotifyPropertyChangedInterface]
    public class LFR_10IOViewModel : IOViewModelBase<LiftStatusClass>
    {
        public LFR_10IOViewModel(ILogOpration logOpration) : base(logOpration)
        {

        }
    }
}
