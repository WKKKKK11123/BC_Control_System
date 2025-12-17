using BC_Control_System.model;
using NPOI.SS.Formula.Functions;
using Prism.Mvvm;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class Ag_1IOViewModel: IOViewModelBase<Ag_1IOModel>
    {
        public Ag_1IOViewModel(ILogOpration logOpration) : base(logOpration)
        {
            
        }
    }
}
