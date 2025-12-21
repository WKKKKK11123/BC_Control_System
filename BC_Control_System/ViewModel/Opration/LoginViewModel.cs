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

namespace BC_Control_System.ViewModel.Opration
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly AuthService _authManager;
        private readonly SysAdmin _sysAdmin;
        public string Title => "LoginView";
        public event Action<IDialogResult> RequestClose;

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        public string UsernameValue { get; set; }
        public string PasswordValue { get; set; }

        public LoginViewModel(SysAdmin sysAdmin, AuthService authManager)
        {
            _sysAdmin = sysAdmin;
            _authManager = authManager;
            CancelCommand = new DelegateCommand(() => RequestClose?.Invoke(new DialogResult(ButtonResult.No)));
            ConfirmCommand = new DelegateCommand(Confirm);
        }

        private async void Confirm()
        {
            try
            {
                var users = await _authManager.Login(UsernameValue, PasswordValue);
                if (users==false)
                {
                    MessageBox.Show("用户名或密码错误");
                    return;
                }
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登录失败: {ex.Message}");
            }
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
