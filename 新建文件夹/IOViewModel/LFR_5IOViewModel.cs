using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Personal;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class LFR_5IOViewModel : BindableBase, INavigationAware
    {
        private string path;
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        public LiftStatusClass LiftStatusClass { get; set; }

        public LFR_5IOViewModel(IDialogService dialogService)
        {
            LiftStatusClass = new LiftStatusClass();
            propertyInfos = LiftStatusClass.GetType().GetProperties();

        }

        private void UpdateValue()
        {
            while (true)
            {
                try
                {
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
                        propertyInfo.SetValue(LiftStatusClass, item);
                    }
                    Thread.Sleep(300);
                }
                catch (Exception ee)
                {

                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
