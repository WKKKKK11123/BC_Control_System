using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Dialogs
{
    [AddINotifyPropertyChangedInterface]
    public class NotificationDialogViewModel : BindableBase, IDialogAware
    {
       public string Title { get; private set; }
        public string Message { get; private set; }
        public string ConfirmButtonText { get; private set; } = "确定";
        public string CancelButtonText { get; private set; } = "取消";
        
        public event Action<IDialogResult> RequestClose;
        
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public NotificationDialogViewModel()
        {
            ConfirmCommand = new DelegateCommand(() => 
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK)));
            
            CancelCommand = new DelegateCommand(() => 
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
            Message = parameters.GetValue<string>("Message");

            // 修改这里：使用TryGetValue提供默认值
            ConfirmButtonText = parameters.TryGetValue("ConfirmButtonText", out string confirmText)
                ? confirmText : "确定";

            CancelButtonText = parameters.TryGetValue("CancelButtonText", out string cancelText)
                ? cancelText : "取消";
        }
    }
}
