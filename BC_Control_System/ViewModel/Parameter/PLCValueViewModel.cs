using HandyControl.Tools.Extension;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Helper;
using BC_Control_Models;
using PropertyChanged;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModel.Parameter
{

    public partial class PLCValueViewModel : ObservableObject, IDialogAware
    {
        public PLCValueViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
            value = new PLCValue();
        }
        private IPLCValue value;
        #region 命令属性
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        #endregion

        #region 视图属性
        [ObservableProperty]
        private string _SiteName = "";
        [ObservableProperty]
        private string _SiteValue = "";
        [ObservableProperty]
        private float _NewSiteValue = 0.0f;
        public float UpLimit { get; set; } = 0.0f;
        public float LowLimit { get; set; } = 0.0f;

        #endregion

        #region 弹窗会话接口实现
        public string Title { get; set; } = "";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        private void WriteMethod()
        {

        }
        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            value = parameters.GetValue<IPLCValue>("PLCValue");
            if (parameters.TryGetValue<float>("UpLimit", out float tempUpLimit))
            {
                UpLimit = tempUpLimit;
            }
            ;
            if (parameters.TryGetValue<float>("LowLimit", out float tempLowLimit))
            {
                LowLimit = tempLowLimit;
            }
            if (UpLimit == LowLimit)
            {
                UpLimit = 32767;
                LowLimit = 0;
            }
            Title = "Value Window";
            SiteName = value.ParameterName;
            SiteValue = value.Value;
        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Confirm()
        {
            ValidActValue();
            DialogParameters keys = new DialogParameters();
            keys.Add("ParamViewValue1", SiteName);
            keys.Add("ParamViewValue2", NewSiteValue);
            //var result = _plcHelper.CommonWrite(value, NewSiteValue);         
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));

        }

        private void ValidActValue()
        {
            if (NewSiteValue > UpLimit)
            {
                NewSiteValue = UpLimit;
            }
            if (NewSiteValue < LowLimit)
            {
                NewSiteValue = LowLimit;
            }
        }


        #endregion
    }
}
