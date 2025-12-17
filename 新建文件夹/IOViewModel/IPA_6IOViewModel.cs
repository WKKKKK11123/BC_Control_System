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
    public class IPA_6IOViewModel : BindableBase, INavigationAware
    {
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        public IPA_6IOClass TankStatusClass { get; set; }
        public double Tank6OutsideLevel { get; set; }
        public double Tank6InsideLevel { get; set; }
        public bool IsAuto { get; set; } = true;
        public IPA_6IOViewModel()
        { 
            TankStatusClass = new IPA_6IOClass();
            propertyInfos= TankStatusClass.GetType().GetProperties();
            WritePLCCommand = new DelegateCommand<string>(WritePLC);
        }
        private readonly Dictionary<string, string> _buttonTips = new Dictionary<string, string>
{
    { "CoverOpen", "确认开盖！" },
    { "CoverClose", "确认关盖！" },
    { "Cleaning", "确认打开清洗！" },
    { "DrainChem", "确认打开排液！" },
    { "DrainWater", "确认打开排水！" },
    { "Exchange", "确认打开换液！" },
    { "Cycle", "确认打开循环！" },
    { "Agitation", "确认打开晃动！" },
    { "Ultrasonic", "确认打开超声！" },
    { "Heating", "确认打开加热！" }
};

        private void WritePLC(string parameterName)
        {
            var item = list.FirstOrDefault(x => x.ParameterName == parameterName);
            if (item == null || !PLC4CommonMethods.Device.IsConnected)
                return;

            string address = item.ValueAddress;
            bool current = PLC4CommonMethods.Device[address] is bool b && b;

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
            CommonMethods.CommonWrite(address, (!current).ToString().ToLower());
        }

        public DelegateCommand<string> WritePLCCommand { get; set; }

        public bool Btn_1 { get; set; }
        public bool Btn_2 { get; set; }
        public bool Btn_3 { get; set; }
        public bool Btn_4 { get; set; }
        public bool Btn_5 { get; set; }
        public bool Btn_6 { get; set; }
        public bool Btn_7 { get; set; }
        public bool Btn_8 { get; set; }
        public bool Btn_9 { get; set; }
        public bool Btn_10 { get; set; }

        private void UpdateValue()
        {
            while (true)
            {
                try
                {
                    if (PLC4CommonMethods.Device.IsConnected)
                    {
                        Btn_1 = GetBoolValueByParameterName("CoverOpen");
                        Btn_2 = GetBoolValueByParameterName("CoverClose");
                        Btn_3 = GetBoolValueByParameterName("Cleaning");
                        Btn_4 = GetBoolValueByParameterName("DrainChem");
                        Btn_5 = GetBoolValueByParameterName("DrainWater");
                        Btn_6 = GetBoolValueByParameterName("Exchange");
                        Btn_7 = GetBoolValueByParameterName("Cycle");
                        Btn_8 = GetBoolValueByParameterName("Agitation");
                        Btn_9 = GetBoolValueByParameterName("Ultrasonic");
                        Btn_10 = GetBoolValueByParameterName("Heating");
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
                    var targetItem = list.FirstOrDefault(x => x.ParameterName == "Tank6OutsideLevel");
                    if (targetItem != null && double.TryParse(targetItem.Value, out double parsedValue))
                    {
                        Tank6OutsideLevel = parsedValue != 0 ? 5 - parsedValue : parsedValue;
                    }

                    var targetItem1 = list.FirstOrDefault(x => x.ParameterName == "Tank6InsideLevel");
                    if (targetItem1 != null && double.TryParse(targetItem1.Value, out double parsedValue1))
                    {
                        if (parsedValue1 != 0)
                        {
                            Tank6InsideLevel = 6;
                        }
                        else
                        {
                            Tank6InsideLevel = parsedValue1;
                        }


                    }
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
            if (item != null && PLC4CommonMethods.Device.IsConnected)
            {
                if (PLC4CommonMethods.Device[item.ValueAddress] is bool b)
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
