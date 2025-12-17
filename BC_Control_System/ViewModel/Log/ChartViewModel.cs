using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Log
{
    [AddINotifyPropertyChangedInterface]
    public class ChartViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "Chart View";

        public event Action<IDialogResult> RequestClose;
        #region 视图属性
        public Type ValueType { get; set; }
        public List<object> CurveValues { get; set; }
        #endregion

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            ValueType=parameters.GetValue<Type>("Param1");
            CurveValues=parameters.GetValue<List<object>>("Param2");
        }
    }
}
