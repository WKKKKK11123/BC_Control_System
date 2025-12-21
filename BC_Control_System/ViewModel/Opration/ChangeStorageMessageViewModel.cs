using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_System.ViewModel.Opration
{
    public class ChangeStorageMessageViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; }="Storage Message";

        public event Action<IDialogResult> RequestClose;
        public bool CanChangeMessage { get; set; }
        public string CarrierID { get; set; }
        public string Location { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        public ChangeStorageMessageViewModel()
        {
            CancelCommand=new DelegateCommand(Cancel);
            ConfirmCommand=new DelegateCommand(Confirm);
        }
        private void Confirm()
        {

            DialogParameters keys = new DialogParameters();
            keys.Add("Value1", CarrierID);
            keys.Add("Value2", Location);
            //var result = _plcHelper.CommonWrite(value, NewSiteValue);         
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
                CanChangeMessage = parameters.GetValue<bool>("Param1");
                CarrierID = parameters.GetValue<string>("Param2");
                Location= parameters.GetValue<string>("Param3");
            }
            catch (Exception ee)
            {


            }
        }
    }
}
