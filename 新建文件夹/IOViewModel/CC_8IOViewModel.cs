
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC_Control_Models.Personal;
using BC_Control_Models;
using BC_Control_Helper;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class CC_8IOViewModel : BindableBase, INavigationAware
    {
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        public double Tank8OutsideLevel { get; set; }
        public CC_8IOClass TankStatusClass { get; set; }
        public CC_8IOViewModel() 
        {
            TankStatusClass = new CC_8IOClass();
            propertyInfos=TankStatusClass.GetType().GetProperties();
        }
        private void UpdateValue()
        {
            while (true)
            {
                try
                {
                    list.UpdateDataClasses();
                    list.UpdateBoolData();
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
                    var targetItem = list.FirstOrDefault(x => x.ParameterName == "OutsideLevel");
                    if (targetItem != null && double.TryParse(targetItem.Value, out double parsedValue))
                    {
                        Tank8OutsideLevel = parsedValue != 0 ? 5 - parsedValue : parsedValue;
                    }
                    Thread.Sleep(300);
                }
                catch (Exception ee)
                {

                }
            }
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
        }    }
}
