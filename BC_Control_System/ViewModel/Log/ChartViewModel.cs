using CommunityToolkit.Mvvm.ComponentModel;
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
   
    public partial class ChartViewModel : ObservableObject, IDialogAware
    {
        public string Title { get; set; } = "Chart View";

        public event Action<IDialogResult> RequestClose;
        #region 视图属性
        [ObservableProperty]
        private Type valueType;
        [ObservableProperty]
        private List<object> _CurveValues;
        #endregion
        public ChartViewModel()
        {
            ValueType = typeof(bool);
            CurveValues=new List<object>();
            RequestClose = new Action<IDialogResult>(item => { });
        }
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
