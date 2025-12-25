using BC_Control_System.Service;
using BC_Control_System.view.Log.TraceLogViews;
using BC_Control_System.View.Log;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;

namespace BC_Control_System.ViewModel.Log
{
    public partial class InsertTimeMainLogViewModel : ObservableObject, INavigationAware
    {
        #region 私有字段
        private readonly ILogCommandService _commandService;
        private readonly IDialogService _dialogService;
        private readonly ViewTransitionNavigator _viewTransitionService;
        #endregion
        #region 视图属性
        [ObservableProperty]
        private string moduleName;
        #endregion
        public InsertTimeMainLogViewModel(ILogCommandService commandService, IDialogService dialogService, ViewTransitionNavigator viewTransitionService)
        {
            moduleName = "";
            _commandService = commandService;
            _dialogService = dialogService;
            _viewTransitionService = viewTransitionService;

        }

        #region 视图命令
        [RelayCommand]
        private void SelectTime()
        {
            try
            {
                IDialogResult r = null;
                _dialogService.ShowDialog(nameof(SearchByTimeView), result => r = result);
                if (r.Result == ButtonResult.OK)
                {
                    var StartTime = r.Parameters.GetValue<DateTime>("Time1");
                    var EndTime = r.Parameters.GetValue<DateTime>("Time2");
                    _commandService.ExecuteSelectTime(StartTime, EndTime);
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
        private void OpenModuleView(StationCollection stationCollection)
        {
            try
            {
                NavigationParameters keys = new NavigationParameters();
                string tempViewName = $"Tank_{stationCollection.StationNo}TraceLog";
                ModuleName = tempViewName;
                _viewTransitionService.TraceLogViewNavigation(tempViewName);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        #endregion
        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["ModuleName"] is StationCollection)
            {
                var stationtemp = navigationContext.Parameters["ModuleName"] as StationCollection;
                if (stationtemp==null)
                {
                    return;
                }
                OpenModuleView(stationtemp);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
    }
}
