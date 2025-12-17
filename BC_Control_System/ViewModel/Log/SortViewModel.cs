using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Log
{
    public class SortViewModel : BindableBase, IDialogAware
    {
        public string Title{ get; set; }="Sort";
        public bool AscendingOrder { get; set; }
        public bool DisAscendingOrder { get; set; }

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand ConfirmCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public SortViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public void Confirm()
        {
            DialogParameters pairs= new DialogParameters();
            pairs.Add("sort",AscendingOrder==true);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, pairs));
        }
        public void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
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
            
        }
    }
}
