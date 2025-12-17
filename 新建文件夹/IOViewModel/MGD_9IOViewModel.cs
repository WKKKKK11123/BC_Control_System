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
    public class MGD_9IOViewModel : BindableBase, INavigationAware
    {
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        public MGD_9IOClass TankStatusClass { get; set; }
        public double Tank9OutsideLevel { get; set; }
        public double Tank9Bubbler1Level { get; set; }
        public double Tank9Bubbler2Level { get; set; }
        public double Tank9Bubbler3Level { get; set; }
        public bool IsAuto { get; set; } = true;
        public MGD_9IOViewModel()
        { 
            TankStatusClass= new MGD_9IOClass();    
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
            if (item == null || !PLC2CommonMethods.Device.IsConnected)
                return;

            string address = item.ValueAddress;
            bool current = PLC2CommonMethods.Device[address] is bool b && b;

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
            PLC2CommonMethods.CommonWrite(address, (!current).ToString().ToLower());
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
                    if (PLC2CommonMethods.Device.IsConnected)
                    {

                        Btn_1 = GetBoolValueByParameterName("CoverOpen");
                        Btn_2 = GetBoolValueByParameterName("CoverClose");
                    }
                    if (CommonMethods.Device["M1000"] is bool m1000)
                    {
                        IsAuto = !m1000; // M1000 为 true 时
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
                    var targetItem = list.FirstOrDefault(x => x.ParameterName == "Tank9OutsideLevel");
                    if (targetItem != null && double.TryParse(targetItem.Value, out double parsedValue))
                    {
                        Tank9OutsideLevel = parsedValue != 0 ? 5 - parsedValue : parsedValue;
                    }
                    var targetItem2 = list.FirstOrDefault(x => x.ParameterName == "Tank9Bubbler1Level");
                    if (targetItem2 != null && double.TryParse(targetItem2.Value, out double parsedValue2))
                    {
                        Tank9Bubbler1Level = parsedValue2 != 0 ? 5 - parsedValue2 : parsedValue2;
                    }
                    var targetItem3 = list.FirstOrDefault(x => x.ParameterName == "Tank9Bubbler2Level");
                    if (targetItem3 != null && double.TryParse(targetItem3.Value, out double parsedValue3))
                    {
                        Tank9Bubbler2Level = parsedValue3 != 0 ? 5 - parsedValue3 : parsedValue3;
                    }
                    var targetItem4 = list.FirstOrDefault(x => x.ParameterName == "Tank9Bubbler3Level");
                    if (targetItem4 != null && double.TryParse(targetItem4.Value, out double parsedValue4))
                    {
                        Tank9Bubbler3Level = parsedValue4 != 0 ? 5 - parsedValue4 : parsedValue4;
                    }
    //                TankStatusClass.AV_MI_6.Value =
    //TankStatusClass.AV_ID_1.Value == "True" ? "false" : "true";
    //                TankStatusClass.AV_MI_5.Value =
    //TankStatusClass.AV_ID_2.Value == "True" ? "false" : "true";
    //                TankStatusClass.AV_MI_4.Value =
    //TankStatusClass.AV_ID_3.Value == "True" ? "false" : "true";
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
            if (item != null && PLC2CommonMethods.Device.IsConnected)
            {
                if (PLC2CommonMethods.Device[item.ValueAddress] is bool b)
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
