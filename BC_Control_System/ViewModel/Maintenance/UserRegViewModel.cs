using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BC_Control_BLL.Services;
using BC_Control_Models;

namespace BC_Control_System.ViewModel.Maintenance
{
    [AddINotifyPropertyChangedInterface]
    public class UseRegViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "User Reg";

        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }
        private readonly SysAdminService _adminService;

        public ObservableCollection<SysAdmin> AdminList { get; set; } = new ObservableCollection<SysAdmin>();

        public SysAdmin CurrentAdmin { get; set; } = new SysAdmin();
        [OnChangedMethod(nameof(SelectChange))]
        public SysAdmin SelectAdmin { get; set; }

        public DelegateCommand AddCommand => new DelegateCommand(async () => await AddAsync());
        public DelegateCommand UpdateCommand => new DelegateCommand(async () => await UpdateAsync());
        public DelegateCommand DeleteCommand => new DelegateCommand(async () => await DeleteAsync());

        public UseRegViewModel(SysAdminService adminService)
        {
            _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
            _ = LoadAdmins();
        }
        private void SelectChange()
        {
            if (SelectAdmin != null)
            {
                CurrentAdmin = SelectAdmin;
            }
        }
        public async Task LoadAdmins()
        {
            try
            {
                //var list = await _adminService.Query();    没有隐藏Vendor的账号
                //AdminList.Clear();
                //foreach (var item in list)
                //    AdminList.Add(item);

                var list = await _adminService.Query();

                AdminList.Clear();

                foreach (var item in list)
                {
                    if (item.UserRights != Rights.Vendor) // 隐藏Vendor的账号
                        AdminList.Add(item);
                }
            }
            catch (Exception ee)
            {


            }

        }

        private async Task AddAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentAdmin.LoginName)) return;

                if (string.IsNullOrWhiteSpace(CurrentAdmin.LoginId))
                    CurrentAdmin.LoginId = Guid.NewGuid().ToString("N");
                //CurrentAdmin.Id = 0;
                var newId = await _adminService.AddReturnId(CurrentAdmin);
                await LoadAdmins();

                CurrentAdmin = new SysAdmin();
            }
            catch (Exception ee)
            {


            }

        }

        private async Task UpdateAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentAdmin.LoginName)) return;
                await _adminService.UpdateEntity(CurrentAdmin);
                await LoadAdmins();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);

            }

        }

        private async Task DeleteAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentAdmin.LoginName)) return;
                await _adminService.Delete(filter => filter.LoginName == CurrentAdmin.LoginName);
                await LoadAdmins();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);

            }

        }
    }
}
