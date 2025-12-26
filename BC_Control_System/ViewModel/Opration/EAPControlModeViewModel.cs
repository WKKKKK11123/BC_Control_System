using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Services.Dialogs;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Opration
{
    public partial class EAPControlModeViewModel : ObservableObject, IDialogAware
    {
        public string Title { get; set; } = "EAP State Control";
        [ObservableProperty]
        private int _controlMode = 0;
        public event Action<IDialogResult> RequestClose;

        private void Confirm()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value1", ControlMode);
            //var result = PLCSelect.Instance.CommonWrite(value, NewSiteValue);         
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));

        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
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
            try
            {
                ControlMode = parameters.GetValue<int>("Param1");               
            }
            catch (Exception ee)
            {

            }
        }
    }
}
