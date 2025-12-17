using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace BC_Control_System.ViewModel.Status
{  
    public partial class HorizontalStateViewModel :  ObservableObject, INavigationAware
    {        
        private readonly IBenchStationEntity _benchStationEntity;
        [ObservableProperty]
        private BindingList<DataClass> moduleDatas;
        [ObservableProperty]
        private BindingList<StatusClass> temperatureControlOffList;
        [ObservableProperty]
        private BindingList<StatusClass> moduleStatus;
        public int ShowIndex { get; set; }
        
        public HorizontalStateViewModel(IBenchStationEntity benchStationEntity)
        {
            _benchStationEntity = benchStationEntity;
            TemperatureControlOffList = new BindingList<StatusClass>();
            ModuleStatus = new BindingList<StatusClass>();
            ModuleDatas = new BindingList<DataClass>();
           
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;

        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var tempModule = (navigationContext.Parameters["Module"] as StationCollection)!;
                var stationInfo = _benchStationEntity.Stations.FirstOrDefault(filter => filter.StationName == tempModule.StationName);
                TemperatureControlOffList = new BindingList<StatusClass>(stationInfo!.ModuleStatus);
                if (TemperatureControlOffList.Count() == 0)
                {
                    ShowIndex = 2;
                }
                ModuleStatus = new BindingList<StatusClass>(stationInfo.BatchDataCollection);
                
                ModuleDatas = new BindingList<DataClass>(stationInfo.ModuleDataCollection);
                if (ModuleDatas.Count() == 0)
                {
                    ShowIndex = 3;
                }              
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        
    }
}
