
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Helper;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    public class LPD_IOViewModel : BindableBase, INavigationAware
    {
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        //public LPD_IOModel TankStatusClass { get; set; }
        public bool IsAuto { get; set; } = true;
        public LPD_IOViewModel()
        {
            //TankStatusClass = new LPDBufferIOModel();
            //propertyInfos = TankStatusClass.GetType().GetProperties();
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (list != null)
            {
                return;
            }
            list = navigationContext.Parameters["IOStatus"] as List<DataClass>;
            await Task.Run(() =>
            {
                UpdateValue();
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        private void UpdateValue()
        {
            try
            {
                while (true)
                {
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
                        object convertedValue = Convert.ChangeType(item.Value, propertyInfo.PropertyType);
                        //propertyInfo.SetValue(TankStatusClass, convertedValue);
                    }
                }
               
            }
            catch (Exception EX)
            {

               
            }
           

           
        }
    }
}
