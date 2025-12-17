using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BC_Control_System.ViewModel.Opration
{
    [AddINotifyPropertyChangedInterface]
    public class ChangeWaferThicknessViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "Change Wafer Thickness";

        public event Action<IDialogResult> RequestClose;
        public int Location { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ChangeWaferThicknessViewModel()
        {
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }
        private void Confirm()
        {

            DialogParameters keys = new DialogParameters();
            keys.Add("Value1", Location);
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
                //CanChangeMessage = parameters.GetValue<bool>("Param1");
                
                Location = parameters.GetValue<int>("Param1");
            }
            catch (Exception ee)
            {


            }
        }
    }
}
