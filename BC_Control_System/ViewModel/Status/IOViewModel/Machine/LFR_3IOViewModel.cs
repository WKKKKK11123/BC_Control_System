using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Personal;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class LFR_3IOViewModel : IOViewModelBase<LiftStatusClass>
    {
        public LFR_3IOViewModel(ILogOpration logOpration) : base(logOpration)
        {

        }
    }
}
