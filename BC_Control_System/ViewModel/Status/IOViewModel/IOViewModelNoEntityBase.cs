using BC_Control_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Security;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Status.IOViewModel
{
    public partial class IOViewModelNoEntityBase : ObservableObject, INavigationAware
    {
        private readonly ILogOpration _logOpration;
        private List<DataClass> list;
        [ObservableProperty]
        private BindingList<DataClass> iODataCollection;
        public IOViewModelNoEntityBase(ILogOpration logOpration)
        {
            _logOpration = logOpration;
            list = new List<DataClass>();
            IODataCollection= new BindingList<DataClass>(); 
        }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("IOStatus", out List<DataClass> templist))
            {
                list = templist;
                IODataCollection = new BindingList<DataClass>(list);
            }
        }
       
    }
}
