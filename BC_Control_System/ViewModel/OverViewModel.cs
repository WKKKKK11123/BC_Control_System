using BC_Control_System.Converters;
using BC_Control_System.Event;
using MiniExcelLibs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PropertyChanged;
using System.Collections.ObjectModel;
using BC_Control_CustomControl;
using ZC_Control_EFAM.ProcessControl;
using BC_Control_Helper;
using BC_Control_Models;
using System.ComponentModel;
using BC_Control_BLL.PLC;
using BC_Control_Models.BenchConfig;
using System.Windows.Input;
using BC_Control_System.Service;

namespace BC_Control_System.ViewModel
{
    [AddINotifyPropertyChangedInterface]

    public class OverViewModel : BindableBase
    {
        private readonly ViewTransitionNavigator _viewTransitionNavigator;
        private ProcessControl _processControl;
        private readonly IRegionManager _regionManager;
        private readonly BatchDataService _batchDataService;
        private readonly IBenchStationEntity _benchStationEntity;
        private readonly IExcelOperation _excelOpration;
        private List<StatusClass> tankLidCollections;
        private List<StatusClass> shutterCollections;
        public ObservableCollection<StatusClass> StorageStatus { get; set; }
        public ObservableCollection<StatusClass> LDStatus { get; set; }
        public ObservableCollection<StatusClass> OPStatus { get; set; }
        public ObservableCollection<StationCollection> LiftCollections { get; set; }
        public int WTRActPos { get; set; }
        public BindingList<ModuleStatus> ModuleStatus { get; set; }
        public ObservableCollection<DoubleSensor> TankLidStatus { get; set; }
        public ObservableCollection<DoubleSensor> ShutterStatus { get; set; }
        public ObservableCollection<ModuleStatus2> ModuleStatus2 { get; set; }
        public ICommand OpenStatusViewCommand { get; set; }
        public OverViewModel(DataBaseAddService dataBaseAddService, IBenchStationEntity benchStationEntity, BatchDataService batchDataService, ProcessControl processControl, IRegionManager regionManager, IExcelOperation excelOperation, ViewTransitionNavigator viewTransitionNavigator)
        {
            _viewTransitionNavigator = viewTransitionNavigator;
            _excelOpration = excelOperation;
            _benchStationEntity = benchStationEntity;
            _batchDataService = batchDataService;
            _batchDataService.UpdateModuleState += dataBaseAddService.UpdateTankProcessLog;
            _processControl = processControl;
            _regionManager = regionManager;
            tankLidCollections = new List<StatusClass>();
            shutterCollections = new List<StatusClass>();
            TankLidStatus = new ObservableCollection<DoubleSensor>();
            ShutterStatus = new ObservableCollection<DoubleSensor>();
            StorageStatus = new ObservableCollection<StatusClass>();
            ModuleStatus = new BindingList<ModuleStatus>(_batchDataService.BatchDataCollection);
            ModuleStatus2 = new ObservableCollection<ModuleStatus2>();
            LDStatus = new ObservableCollection<StatusClass>();
            OPStatus = new ObservableCollection<StatusClass>();
            OpenStatusViewCommand = new DelegateCommand<object>(OpenStatusView);
            LiftCollections = new ObservableCollection<StationCollection>();
            LoadFromExcel();
            AddStorage();
        }
        private void OpenStatusView(object Module)
        {

            var tempModule = Module as IStationConfig;
            NavigationParameters keys = new NavigationParameters();
            keys.Add("Module", tempModule);
            _viewTransitionNavigator.MainVeiwNavigation("StatusMainView", keys);
        }
        private async void LoadFromExcel()
        {
            try
            {
                string filePath = @"D:\BC3100\212\File\OterFile\MachineAddress.xlsx";
                tankLidCollections = _excelOpration.ReadExcelToObjects<StatusClass>(filePath, "TankLid").ToList();
                shutterCollections = _excelOpration.ReadExcelToObjects<StatusClass>(filePath, "Shutters").ToList();
                AddFactoryMessage();
                LoadTankLidEntity();
                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            _batchDataService.UpdateBatchData();
                            tankLidCollections.UpdateStatus();
                            shutterCollections.UpdateStatus();
                            StorageStatus[1].Value = _processControl.StoragePlaceSenser[0] ? "1" : "0";
                            StorageStatus[2].Value = _processControl.StoragePlaceSenser[1] ? "1" : "0";
                            StorageStatus[3].Value = _processControl.StoragePlaceSenser[2] ? "1" : "0";
                            StorageStatus[4].Value = _processControl.StoragePlaceSenser[3] ? "1" : "0";
                            StorageStatus[5].Value = _processControl.StoragePlaceSenser[4] ? "1" : "0";
                            StorageStatus[6].Value = _processControl.StoragePlaceSenser[5] ? "1" : "0";
                            StorageStatus[7].Value = _processControl.StoragePlaceSenser[6] ? "1" : "0";
                            StorageStatus[8].Value = _processControl.StoragePlaceSenser[7] ? "1" : "0";
                            StorageStatus[9].Value = _processControl.StoragePlaceSenser[8] ? "1" : "0";
                            StorageStatus[10].Value = _processControl.StoragePlaceSenser[9] ? "1" : "0";
                            StorageStatus[11].Value = _processControl.StoragePlaceSenser[10] ? "1" : "0";
                            StorageStatus[12].Value = _processControl.StoragePlaceSenser[11] ? "1" : "0";
                            StorageStatus[13].Value = _processControl.StoragePlaceSenser[12] ? "1" : "0";
                            StorageStatus[14].Value = _processControl.StoragePlaceSenser[13] ? "1" : "0";
                            StorageStatus[15].Value = _processControl.StoragePlaceSenser[14] ? "1" : "0";
                            StorageStatus[16].Value = _processControl.StoragePlaceSenser[15] ? "1" : "0";
                            StorageStatus[17].Value = _processControl.StoragePlaceSenser[16] ? "1" : "0";
                            StorageStatus[18].Value = _processControl.StoragePlaceSenser[17] ? "1" : "0";

                            OPStatus[1].Value = _processControl.Opener1PlaceSenser ? "1" : "0";
                            OPStatus[2].Value = _processControl.Opener2PlaceSenser ? "1" : "0";

                            LDStatus[1].Value = _processControl.Lp1PlaceSenser ? "1" : "0";
                            LDStatus[2].Value = _processControl.Lp2PlaceSenser ? "1" : "0";
                            //LDStatus[3].Value = _processControl.Lp3PlaceSenser ? "1" : "0";
                            //LDStatus[4].Value = _processControl.Lp4PlaceSenser ? "1" : "0";

                            Thread.Sleep(500);
                        }
                        catch (System.Exception e)
                        {


                        }

                    }
                });
            }
            catch (System.Exception ee)
            {

            }

        }
        private void Navigate(string moduleName)
        {
            NavigationParameters keys = new NavigationParameters();
            try
            {
                string tempmodule;
                switch (moduleName)
                {
                    case "Tank1#EKC":
                        tempmodule = "SYS9070_1";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank2#EKC":
                        tempmodule = "SYS9070_2";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank3#NMP":
                        tempmodule = "NMP_3";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank4#NMP":
                        tempmodule = "NMP_4";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank5#IPA":
                        tempmodule = "IPA_5";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank6#IPA":
                        tempmodule = "IPA_6";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank7#QDR":
                        tempmodule = "QDR_7";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank4#QDR":
                        tempmodule = "QDR_4";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank5#CC":
                        tempmodule = "CC_8";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    case "Tank8#CC":
                        tempmodule = "CC_8";
                        keys.Add("ModuleName", tempmodule);
                        break;
                    default:
                        tempmodule = moduleName;
                        keys.Add("ModuleName", tempmodule);
                        break;
                }

                _regionManager.Regions["ContentRegion"].RequestNavigate("StatusMainView", keys);
            }
            catch (Exception EX)
            {


            }

        }

        private void LoadTankLidEntity()
        {
            try
            {
                var tempLFRList = _benchStationEntity.Stations.Where(filter => filter.StationName.Contains("LFR") && filter.StationType == StationEnum.Mechanical);
                for (int i = 1; i < 13; i++)
                {
                    var item = tempLFRList.FirstOrDefault(filter => filter.StationNo == i);
                    if (item != null)
                    {
                        LiftCollections.Add(item);
                    }
                    else
                    {
                        LiftCollections.Add(new StationCollection());
                    }

                }
                for (int i = 12; i > 0; i--)
                {


                    TankLidStatus.Add(new DoubleSensor()
                    {
                        Sensor1 = tankLidCollections.FirstOrDefault(filter => filter.ParameterName == $"Tank{i}LidOpenSensor")!,
                        Sensor2 = tankLidCollections.FirstOrDefault(filter => filter.ParameterName == $"Tank{i}LidCloseSensor")!,
                    });
                }
                TankLidStatus[0].Enable = false;
                for (int i = 6; i > 0; i--)
                {
                    ShutterStatus.Add(new DoubleSensor()
                    {
                        Sensor1 = shutterCollections.FirstOrDefault(filter => filter.ParameterName == $"Shutter{i}OpenSensor")!,
                        Sensor2 = shutterCollections.FirstOrDefault(filter => filter.ParameterName == $"Shutter{i}CloseSensor")!,
                    });
                }
            }

            catch (Exception ex)
            {
                throw;
            }


        }
        private void AddFactoryMessage()
        {
            List<StatusClass> tempFactory = _benchStationEntity.Stations.FirstOrDefault(filter => filter.StationName == "Factory")!.ModuleStatus ?? new List<StatusClass>();
            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "CDA Pump 1",
                Current = tempFactory.Find(para => para.ParameterName == "CDAPump1Current") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "CDAPump1SettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "CDAPump1SettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "CDAPump1SettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "CDA Pump 2",
                Current = tempFactory.Find(para => para.ParameterName == "CDAPump2Current") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "CDAPump2SettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "CDAPump2SettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "CDAPump2SettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "CDA Valve",
                Current = tempFactory.Find(para => para.ParameterName == "CDAValveCurrent") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "CDAValveSettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "CDAValveSettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "CDAValveSettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "CDA Damper",
                Current = tempFactory.Find(para => para.ParameterName == "CDADamperCurrent") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "CDADamperSettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "CDADamperSettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "CDADamperSettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "N2 Chuck",
                Current = tempFactory.Find(para => para.ParameterName == "N2ChuckCurrent") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "N2ChuckSettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "N2ChuckSettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "N2ChuckSettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "N2 Total Liquid Level",
                Current = tempFactory.Find(para => para.ParameterName == "N2TotalLiquidLevelCurrent") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "N2TotalLiquidLevelSettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "N2TotalLiquidLevelSettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "N2TotalLiquidLevelSettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "N2 Liquid Level 1",
                Current = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel1Current") ?? new StatusClass(),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel1SettingUpLime") ?? new StatusClass(),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel1SettingDownLime") ?? new StatusClass(),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel1SettingDelayTime") ?? new StatusClass(),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "N2 Liquid Level 2",
                Current = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel2Current"),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel2SettingUpLime"),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel2SettingDownLime"),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "N2LiquidLevel2SettingDelayTime"),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "N2 Static Eliminator",
                Current = tempFactory.Find(para => para.ParameterName == "N2StaticEliminatorCurrent"),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "N2StaticEliminatorSettingUpLime"),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "N2StaticEliminatorSettingDownLime"),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "N2StaticEliminatorSettingDelayTime"),

            });

            ModuleStatus2.Add(new ModuleStatus2()
            {
                ModuleName2 = "CO2 Pressure",
                Current = tempFactory.Find(para => para.ParameterName == "CO2PressureCurrent"),
                SettingUpLime = tempFactory.Find(para => para.ParameterName == "CO2PressureSettingUpLime"),
                SettingDownLime = tempFactory.Find(para => para.ParameterName == "CO2PressureSettingDownLime"),
                SettingDelayTime = tempFactory.Find(para => para.ParameterName == "CO2PressureSettingDelayTime"),
            });


        }
        private void AddStorage()
        {
            for (int i = 0; i <= 18; i++)
            {
                StorageStatus.Add(new StatusClass()
                {
                    No = i,
                    ParameterName = $"{i}",
                    Value = "0"
                });
            }
            for (int i = 0; i <= 2; i++)
            {

                LDStatus.Add(new StatusClass()
                {
                    No = i,
                    ParameterName = $"LP{i}",
                    Value = "0"
                });
            }
            for (int i = 0; i <= 2; i++)
            {
                OPStatus.Add(new StatusClass()
                {
                    No = i,
                    ParameterName = $"OP{i}",
                    Value = "0"
                });
            }
        }

    }
}
