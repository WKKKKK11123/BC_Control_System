using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.Reflection;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Personal;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    public class WTRIOViewModel : IOViewModelNoEntityBase
    {
        public WTRIOViewModel(ILogOpration logOpration) : base(logOpration)
        {

        }
    }
}

