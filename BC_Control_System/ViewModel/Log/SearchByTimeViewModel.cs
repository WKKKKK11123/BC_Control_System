using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BC_Control_BLL.Services;
using BC_Control_DAL;
using BC_Control_Models;

namespace BC_Control_System.ViewModel.Log
{
    [AddINotifyPropertyChangedInterface]
    public class SearchByTimeViewModel : BindableBase, IDialogAware
    {
        public SearchByTimeViewModel() 
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
           
        }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        public DelegateCommand<string> OpenDialogViewCommand { get; set; }
        public DateTime StartTime { get; set; }= DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public string Title => "Search";

        public event Action<IDialogResult> RequestClose;

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Confirm()
        {
            if (StartTime>=EndTime)
            {
                MessageBox.Show("开始时间不能大于结束时间，间隔至少一天");
                return;
            }
            DialogParameters keys = new DialogParameters();
            keys.Add("Time1", StartTime);
            keys.Add("Time2", EndTime);

            //var result = _plcHelper.CommonWrite(value, NewSiteValue);         
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));

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
