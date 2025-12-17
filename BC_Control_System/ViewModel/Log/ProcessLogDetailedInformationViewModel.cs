using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using BC_Control_BLL.Services;
using BC_Control_Models;
using BC_Control_Models.Log;
using BC_Control_System.Comand;
using BC_Control_System.Command;
using BC_Control_System.View.Log;
using static NPOI.HSSF.Util.HSSFColor;
using BC_Control_System.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BC_Control_Models.BenchConfig;

namespace BC_Control_System.ViewModel.Log
{
    public partial class ProcessLogDetailedInformationViewModel : ObservableObject, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IDialogService _dialogService;
        private readonly ViewTransitionNavigator _viewTransitionService;
        private readonly ILogCommandService _commandService;
        private readonly TankProcessService _tankProcessService;
        [ObservableProperty]
        private ObservableCollection<TankProcess> tankList;
        [ObservableProperty]
        private TankProcess selectedTank;

        private int _endofRunId;

        public ProcessLogDetailedInformationViewModel(ViewTransitionNavigator viewTransitionService, ILogCommandService commandService, TankProcessService tankService, IRegionManager regionManager, IDialogService dialogService)
        {
            _viewTransitionService = viewTransitionService;
            _commandService = commandService;
            _dialogService = dialogService;
            _tankProcessService = tankService;
            _regionManager = regionManager;
            SelectedTank = new TankProcess();
            TankList = new ObservableCollection<TankProcess>();
        }
        #region 视图命令
        [RelayCommand]
        private void Back()
        {
            _regionManager.RequestNavigate("ContentRegion", "EndofRunLogView");
        }
        [RelayCommand]
        private void SelectTime()
        {
            try
            {
                var StartTime = SelectedTank.StartTime;
                if (SelectedTank.EndTime != null)
                {
                    var EndTime = SelectedTank.EndTime;
                    _commandService.ExecuteSelectTime(StartTime, (DateTime)EndTime);
                }
                else
                {
                    MessageBox.Show("无结束时间");
                }             
            }
            catch (Exception ee)
            {

            }

        }
        [RelayCommand]
        private void ExportExcel()
        {
            try
            {
                string path = "";
                List<object> list = _commandService.ExcuteGetList();
                Type type = _commandService.ExcuteGetType();
            }
            catch (Exception)
            {

                throw;
            }

        }
        [RelayCommand]
        private void OpenChartView()
        {
            try
            {
                List<object> list = _commandService.ExcuteGetList();
                Type type = _commandService.ExcuteGetType();
                IDialogResult r = null;
                DialogParameters keyValuePairs = new DialogParameters();
                keyValuePairs.Add("Param1", type);
                keyValuePairs.Add("Param2", list);
                _dialogService.ShowDialog(nameof(ChartView), keyValuePairs, result => r = result);

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region 私有方法 
        partial void OnSelectedTankChanged(TankProcess value)
        {
            try
            {
                NavigationParameters keys = new NavigationParameters();
                if (value.StationNo==0)
                {
                    return;
                }
                string tempViewName = $"Tank_{value.StationNo}TraceLog";
                _viewTransitionService.NavigationAware(tempViewName, "ProcessTraceLogContentRegion");
                SelectTime();
            }
            catch (Exception)
            {

                throw;
            }
        }       
        #endregion

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("EndofRunId"))
            {
                _endofRunId = navigationContext.Parameters.GetValue<int>("EndofRunId");

                var tankList = await _tankProcessService.Query(x => x.DataId == _endofRunId);
                TankList.Clear();
                TankList.AddRange(tankList);

            }
        }



    }
}
