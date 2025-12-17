using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.Personal;
using BC_Control_Models;
using Prism.Regions;
using Prism.Mvvm;

using BC_Control_Helper;

namespace BC_Control_System.ViewModel
{
    public class LPDBuffer_1IOViewModel : BindableBase, INavigationAware
    {
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        
        public bool IsAuto { get; set; } = true;
        public LPDBuffer_1IOViewModel()
        {
                  
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
           
        }
    }
}
