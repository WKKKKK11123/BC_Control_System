using BC_Control_BLL.Services;
using BC_Control_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BC_Control_System
{
    public partial class AuthService : ObservableObject
    {
        private readonly SysAdminService _adminService;
        private readonly SysAdmin _sysAdmin;
        private readonly ILogOpration _logOpration;
        private readonly DispatcherTimer _sessionTimer;
        private DateTime _lastActiveTime;
        [ObservableProperty]
        private Rights userRights;
        [ObservableProperty]
        private string userName;
        public AuthService(SysAdmin sysAdmin, SysAdminService adminService, ILogOpration logOpration)
        {
            UserName = "Guest";
            UserRights = 0;
            _sysAdmin = sysAdmin;
            _adminService = adminService;
            _logOpration = logOpration;
            _lastActiveTime = DateTime.Now;
            SwitchToGuest();
            _sessionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            _sessionTimer.Tick += CheckSessionTimeout;
            _sessionTimer.Start();
        }
        public async Task<bool> Login(string username, string password)
        {
            // 验证逻辑（这里简化处理）
            var user = await ValidateCredentials(username, password);
            if (!string.IsNullOrEmpty(user.LoginName))
            {
                _sysAdmin.LoginName = user.LoginName;
                _sysAdmin.UserRights = user.UserRights;
                UserName = user.LoginName;
                UserRights = user.UserRights;
                _lastActiveTime = DateTime.Now;
                _sysAdmin.IsLoggedIn = true;
                return true;
            }
            return false;
        }
        private void SwitchToGuest()
        {
            UserName = "Guest";
            UserRights = 0;
            _sysAdmin.LoginName = "Guest";
            _sysAdmin.UserRights = 0;
            _sysAdmin.IsLoggedIn = false;
        }
        public void Login()
        {

        }
        public void Logout()
        {
            SwitchToGuest();
        }
        public async Task<List<SysAdmin>> LoadAdmins()
        {
            try
            {
                var list = await _adminService.Query();
                return list;
            }
            catch (Exception ee)
            {
                _logOpration.WriteError(ee);
                return new List<SysAdmin>();
            }

        }

        private async Task AddAsync(SysAdmin sysAdmin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysAdmin.LoginName)) return;

                if (string.IsNullOrWhiteSpace(sysAdmin.LoginId))
                    sysAdmin.LoginId = Guid.NewGuid().ToString("N");
                //CurrentAdmin.Id = 0;
                var newId = await _adminService.AddReturnId(sysAdmin);
                await LoadAdmins();
            }
            catch (Exception ee)
            {
                _logOpration.WriteError(ee);
            }

        }

        private async Task UpdateAsync(SysAdmin sysAdmin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysAdmin.LoginName)) return;
                await _adminService.UpdateEntity(sysAdmin);
                await LoadAdmins();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);

            }

        }

        private async Task DeleteAsync(SysAdmin sysAdmin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysAdmin.LoginName)) return;
                await _adminService.Delete(filter => filter.LoginName == sysAdmin.LoginName);
                await LoadAdmins();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);

            }

        }

        private void CheckSessionTimeout(object? sender, EventArgs e)
        {
            if (_sysAdmin?.IsLoggedIn == true && DateTime.Now - _lastActiveTime > new TimeSpan(0, 30, 0))
            {
                // 切换到UI线程执行
                Logout();
            }
        }
        public void UpdateActivity()
        {
            if (_sysAdmin != null)
            {
                _lastActiveTime = DateTime.Now;
            }
        }
        private async Task<SysAdmin> ValidateCredentials(string username, string password)
        {
            try
            {
                var result = await _adminService.Query(filter =>
                    filter.LoginName == username &&
                    filter.LoginPwd == password);
               ;
                return result.FirstOrDefault() ?? new SysAdmin();
            }
            catch (Exception ee)
            {
                _logOpration.WriteError(ee);
                return new SysAdmin();
            }


        }
    }
}
