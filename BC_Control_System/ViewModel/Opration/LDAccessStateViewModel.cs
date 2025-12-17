using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Opration
{
    [AddINotifyPropertyChangedInterface]
    public class LDAccessStateViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "Loadport Access";

        public event Action<IDialogResult> RequestClose;
        public bool LD1AccessState { get; set; }
        public bool LD2AccessState { get; set; }
        public bool LD3AccessState { get; set; }
        public bool LD4AccessState { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        public LDAccessStateViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            ConfirmCommand = new DelegateCommand(Confirm);
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
            var accessstate= parameters.GetValue<bool[]>("AccessState");
            LD1AccessState = accessstate[0];
            LD2AccessState= accessstate[1];
            LD3AccessState= accessstate[2];
            LD4AccessState= accessstate[3];
        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Confirm()
        {          
            DialogParameters keys = new DialogParameters();
            var result = new List<bool>() { LD1AccessState, LD2AccessState, LD3AccessState, LD4AccessState };
            keys.Add("Result1", result);     
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }
    }
}
