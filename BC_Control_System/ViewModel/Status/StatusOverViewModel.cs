using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using Prism.Common;
using SqlSugar.SplitTableExtensions;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Prism.Services.Dialogs;
using BC_Control_System.View;
using DryIoc;

namespace BC_Control_System.ViewModel
{
    public partial class StatusOverViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly IBenchStationEntity _benchStationEntity;
        private IStationConfig _stationCollection;
        [ObservableProperty]
        private string? moduleName = "";
        public StatusOverViewModel(IRegionManager regionManager, IBenchStationEntity benchStationEntity,IDialogService dialogService)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;
            _benchStationEntity = benchStationEntity;
            _stationCollection = new StationCollection();

        }
        #region  视图命令
        [RelayCommand]
        private void OpenStationValueView()
        {
            try
            {
                DialogParameters parameter=new DialogParameters();
                parameter.Add("Module", _benchStationEntity.Stations.FirstOrDefault(filter => filter.StationNo == _stationCollection.StationNo && filter.StationName == _stationCollection.StationName));
                _dialogService.ShowDialog(nameof(StationValueView), parameter, r => { });
            }
            catch (Exception)
            {

            }
        }
        [RelayCommand]
        private void OpenFolder()
        {
            try
            {
                string pathTemp = Path.Combine(@"D:\BC3100\212\File\Station", _stationCollection.StationName);

                if (Directory.Exists(pathTemp))
                {
                    Process.Start("explorer.exe", $"\"{pathTemp}\"");
                }
            }
            catch (Exception)
            {
                
            }
        }
        #endregion
        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                int moduleNo = 0;
                if (!navigationContext.Parameters.TryGetValue<IStationConfig>("Module", out var tempModuleCollection))
                {
                    throw new ArgumentException("传入StatusView参数错误");
                }
                _stationCollection = tempModuleCollection;
                moduleNo = _stationCollection!.StationNo;
                if (moduleNo == 0)
                {
                    ModuleName = $"{_stationCollection.StationType}#{_stationCollection.StationName}";
                }
                else
                {
                    ModuleName = $"{_stationCollection.StationType}{_stationCollection.StationNo}#{_stationCollection.StationName}";
                }
                OpenModuleView();
                OpenIOView(); 
                OpenControlView();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
        #region 私有方法
        private void OpenModuleView()
        {
            try
            {
                NavigationParameters keys = new NavigationParameters();
                var station=_benchStationEntity.Stations.FirstOrDefault(filter=>filter.StationNo== _stationCollection.StationNo && filter.StationName==_stationCollection.StationName);
                keys.Add("Module", station);
                keys.Add("TemperatureControlOffList", station.ModuleStatus);
                keys.Add("ModuleStatus", station.BatchDataCollection);
                keys.Add("ModuleDatas", station.ModuleDataCollection);
                _regionManager.Regions["ModuleContentRegion"].RequestNavigate("HorizontalStateView", keys);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private void OpenIOView()
        {
            try
            {
                NavigationParameters keys = new NavigationParameters();
                var station = _benchStationEntity.Stations.FirstOrDefault(filter => filter.StationNo == _stationCollection.StationNo && filter.StationName == _stationCollection.StationName);
                keys.Add("IOStatus", station.IOViewDataCollection);
                _regionManager.Regions["IOContentRegion"].RequestNavigate($"{_stationCollection.StationName}IOView", keys);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private void OpenControlView()
        {
            NavigationParameters keys = new NavigationParameters();
            var station = _benchStationEntity.Stations.FirstOrDefault(filter => filter.StationNo == _stationCollection.StationNo && filter.StationName == _stationCollection.StationName);
            keys.Add("Param1", station);
            _regionManager.Regions["IOControllerContent"].RequestNavigate($"ControlPanelView", keys);
        }
        #endregion
    }
}
