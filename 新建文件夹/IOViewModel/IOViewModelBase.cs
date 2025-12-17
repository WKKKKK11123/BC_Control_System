using Prism.Mvvm;
using Prism.Regions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models.Personal;
using BC_Control_Models;
using BC_Control_Helper;
using System.Windows.Documents;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class IOViewModelBase<T> : BindableBase, INavigationAware where T : class, new()
    {
        private readonly ILogOpration _logOpration;
        private List<DataClass> list;
        private PropertyInfo[] propertyInfos;
        private CancellationTokenSource _cts;
        public T TankStatusClass { get; set; }
        public IOViewModelBase(ILogOpration logOpration)
        {
            _logOpration = logOpration;
        }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _cts.Cancel();
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            list= list = navigationContext.Parameters["IOStatus"] as List<DataClass>;
            _cts = new CancellationTokenSource();
            UpdateValue();

        }
        public virtual void UpdateValue()
        {
            Task.Run(() =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {

                        list.UpdateDataClasses();
                        TankStatusClass = list.ToEntity<T>();
                        Thread.Sleep(300);
                    }
                }
                catch (Exception ee)
                {
                    _logOpration.WriteError($"{ee.StackTrace},{ee.Message}");
                }
            }, _cts.Token);
        }
    }
}
