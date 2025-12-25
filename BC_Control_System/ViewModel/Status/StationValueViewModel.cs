using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Status
{
    public partial class StationValueViewModel : ObservableObject, IDialogAware
    {
        private readonly IBenchStationEntity _benchStationEntity;
        [ObservableProperty]
        private BindingList<StatusClass> moduleStatusClasseCollection;
        [ObservableProperty]
        private BindingList<DataClass> moduleDataClasseCollection;
        [ObservableProperty]
        private BindingList<StatusClass> batchDataClasseCollection;
        [ObservableProperty]
        private BindingList<DataClass> iODataClasseCollection;
        [ObservableProperty]
        private BindingList<DataClass> _CotrolCollection;
        public string Title { get; set; } = "";

        public event Action<IDialogResult> RequestClose;

        public StationValueViewModel(IBenchStationEntity benchStationEntity)
        {
            _benchStationEntity= benchStationEntity;
            RequestClose = new Action<IDialogResult>(item => { });
            ModuleStatusClasseCollection = new BindingList<StatusClass>();
            ModuleDataClasseCollection=new BindingList<DataClass>();
            BatchDataClasseCollection=new BindingList<StatusClass> { };
            IODataClasseCollection=new BindingList<DataClass> { };
            CotrolCollection=new BindingList<DataClass> { };
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            parameters.TryGetValue("Module",out StationCollection stationCollection);
            StationCollection? stationInfo = _benchStationEntity.Stations.FirstOrDefault(filter => filter.StationName == stationCollection.StationName);
            if (stationInfo?.ModuleStatus!=null)
            {
                ModuleStatusClasseCollection = new BindingList<StatusClass>(stationInfo.ModuleStatus);
            }
            if (stationInfo?.ModuleDataCollection!= null)
            {
                ModuleDataClasseCollection = new BindingList<DataClass>(stationInfo.ModuleDataCollection);
            }
            if (stationInfo?.BatchDataCollection != null)
            {
                BatchDataClasseCollection = new BindingList<StatusClass>(stationInfo.BatchDataCollection);
            }
            if (stationInfo?.IOViewDataCollection != null)
            {
                IODataClasseCollection = new BindingList<DataClass>(stationInfo.IOViewDataCollection);
            }
            if (stationInfo?.ControlDataCollection != null)
            {
                CotrolCollection = new BindingList<DataClass>(stationInfo.ControlDataCollection);
            }  
        }
    }
}
