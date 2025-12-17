
using PropertyChanged;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_Models
{
    public class Admins
    {
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public bool HandCtrl { get; set; }
        public bool AutoCtrl { get; set; }
        public bool SysSet { get; set; }
        public bool SysLog { get; set; }
        public bool Report { get; set; }
        public bool Trend { get; set; }
        public bool UserManage { get; set; }

    }
    [AddINotifyPropertyChangedInterface]
    public class SysAdmin:INotifyPropertyChanged
    {
        #region MyRegion

        public bool IsLoggedIn
        {
            get ;
            set;
        }
        #endregion     
        /// <summary>
        /// 用户名ID
        /// </summary>
        public string LoginId
        {
            get; set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
         private string _loginName;
        [SugarColumn(IsPrimaryKey = true)]
        public string LoginName
        {
            get => _loginName;
            set
            {
                if (_loginName == value)
                {
                    return;
                }
                _loginName = value;
                OnPropertyChanged();
            }
        }
        

            
        /// <summary>
        /// 用户名密码
        /// </summary>
        public string LoginPwd
        {
            get; set;
        }

        /// <summary>
        /// 用户名权限：Operator, Supervisor, Service, ZC
        /// </summary>
        public Rights UserRights
        {
            get; set;
        }

        public string Personal
        {
            get; set;
        }
        public string Comment
        {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public enum Rights
    {
        Operator = 1,
        ProcessEngineer = 2,
        MaintenanceEngineer = 3,
        Supervisor = 4, 
        Vendor = 5
    }
}
