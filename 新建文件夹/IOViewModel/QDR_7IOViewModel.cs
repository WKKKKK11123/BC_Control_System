using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PropertyChanged;
using System.Reflection;
using System.Windows;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Personal;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class QDR_7IOViewModel : BindableBase, INavigationAware
    {
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        public double Tank7OutsideLevel { get; set; }
        public QDR_7IOClass TankStatusClass { get; set; }
        public bool IsAuto { get; set; } = true;
        public QDR_7IOViewModel()
        { 
            TankStatusClass = new QDR_7IOClass();
            propertyInfos=TankStatusClass.GetType().GetProperties();
            WritePLCCommand = new DelegateCommand<string>(WritePLC);
        }

        private readonly Dictionary<string, string> _buttonTips = new Dictionary<string, string>
{
    { "CoverOpen", "确认开盖！" },
    { "CoverClose", "确认关盖！" }
};

        private void WritePLC(string parameterName)
        {
            var item = list.FirstOrDefault(x => x.ParameterName == parameterName);
            if (item == null || !PLC3CommonMethods.Device.IsConnected)
                return;

            string address = item.ValueAddress;
            bool current = PLC3CommonMethods.Device[address] is bool b && b;

            string action = current ? "关闭" : "打开";
            string message;

            if (_buttonTips.ContainsKey(parameterName))
            {
                message = _buttonTips[parameterName].Replace("打开", action);
            }
            else
            {
                message = $"确认{action}当前状态！";
            }

            if (MessageBox.Show(message, "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            // 写入相反值
            PLC3CommonMethods.CommonWrite(address, (!current).ToString().ToLower());
        }

        public DelegateCommand<string> WritePLCCommand { get; set; }

        public bool Btn_1 { get; set; }
        public bool Btn_2 { get; set; }
        private void UpdateValue()
        {
            while (true)
            {
                try
                {
                    if (PLC3CommonMethods.Device.IsConnected)
                    {

                        Btn_1 = GetBoolValueByParameterName("CoverOpen");
                        Btn_2 = GetBoolValueByParameterName("CoverClose");
                    }
                    if (CommonMethods.Device["M1000"] is bool m1000)
                    {
                        IsAuto = !m1000; // M1000 为 true 时，禁用按钮（IsPlcReady 为 false）
                    }
                    list.UpdateBoolData();
                    list.UpdateDataClasses();
                    
                    foreach (var item in list)
                    {
                        var propertyInfo = propertyInfos.FirstOrDefault(para =>
                            para.Name == item.ParameterName
                        );
                        if (propertyInfo == null)
                        {
                            continue;
                        }
                        propertyInfo.SetValue(TankStatusClass, item);
                    }
                    var targetItem = list.FirstOrDefault(x => x.ParameterName == "Tank7OutsideLevel");
                    if (targetItem != null && double.TryParse(targetItem.Value, out double parsedValue))
                    {
                        Tank7OutsideLevel = parsedValue != 0 ? 5 - parsedValue : parsedValue;
                    }
                    //if (TankStatusClass.AV_DR_2.Value == "true")
                    //{
                    //    TankStatusClass.AV_DR_3.Value = "false";
                    //}
                    //else if(TankStatusClass.AV_DR_2.Value == "false")
                    //{
                    //    TankStatusClass.AV_DR_3.Value = "true";
                    //}
                    TankStatusClass.AV_DR_3.Value =
    TankStatusClass.AV_DR_2.Value == "true" ? "false" : "true";
                    Thread.Sleep(300);
                }
                catch (Exception ee)
                {

                }
            }
        }
        private bool GetBoolValueByParameterName(string parameterName)
        {
            var item = list.FirstOrDefault(x => x.ParameterName == parameterName);
            if (item != null && PLC3CommonMethods.Device.IsConnected)
            {
                if (PLC3CommonMethods.Device[item.ValueAddress] is bool b)
                    return b;
            }
            return false;
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            if (list != null)
            {
                return;
            }
            list = navigationContext.Parameters["IOStatus"] as List<DataClass>;
            Task.Run(() =>
            {
                UpdateValue();
            });
        }
    }
}
