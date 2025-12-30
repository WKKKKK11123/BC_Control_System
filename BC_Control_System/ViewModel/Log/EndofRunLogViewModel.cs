using System;
using Prism.Mvvm;
using PropertyChanged;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL;
using BC_Control_Models;
using BC_Control_Models.ClassK.SQLService;
using Prism.Commands;
using Prism.Regions;
using BC_Control_DAL;
using System.Reflection;
using Prism.Services.Dialogs;
using DryIoc;
using BC_Control_System.View.Log;
using BC_Control_BLL.Services;
using BC_Control_System.Comand;
using BC_Control_System.Command;
using MiniExcelLibs;
using BC_Control_Models.Log;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModels.LogDataModel
{
    //20251219
    public partial class EndofRunLogViewModel : ObservableObject, INavigationAware
    {
        private EndofRunLogService _sqlSugarHelper;
        [ObservableProperty]
        private DataTable _EndofRunLogTable;
        [ObservableProperty]
        private List<EndofRunData> _EndofRunLogList;
        [ObservableProperty]
        private EndofRunData _selectedEndofRunLog;
        public DelegateCommand SelectByTime { get; set; }
        IDialogService _dialogService;

        private readonly IRegionManager _regionManager;
        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand OpenDetailCommand { get; }



        public EndofRunLogViewModel(IRegionManager regionManager, EndofRunLogService sqlSugarHelper, IDialogService dialogService)
        {
            EndofRunLogTable = new DataTable();
            SelectedEndofRunLog = new EndofRunData();
            EndofRunLogList = new List<EndofRunData>();
            _dialogService = dialogService;
            _sqlSugarHelper = sqlSugarHelper;
            SelectByTime = new DelegateCommand(SelectTime);

            _regionManager = regionManager;

            OpenDetailCommand = new DelegateCommand(OpenDetail);

            ExportCommand = new DelegateCommand(ExportData);
        }

        private void OpenDetail()
        {
            if (SelectedEndofRunLog == null)
                return;

            var parameters = new NavigationParameters
    {
        { "EndofRunId", SelectedEndofRunLog.Id }
    };
            _regionManager.RequestNavigate("ContentRegion", "ProcessLogDetailedInformationView", parameters);
        }

        public async void SelectTime()
        {
            try
            {
                IDialogResult r = null;
                _dialogService.ShowDialog(nameof(SearchByTimeView), result => r = result);
                if (r.Result == ButtonResult.OK)
                {
                    var StartTime = r.Parameters.GetValue<DateTime>("Time1");
                    var EndTime = r.Parameters.GetValue<DateTime>("Time2");
                    EndofRunLogList = await _sqlSugarHelper.Query(filter => filter.StartTime > StartTime && filter.StartTime < EndTime);
                }
            }

            catch (Exception ee)
            {

            }

        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {

            }
            catch (Exception ee)
            {

            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void ExportData()
        {
            if (EndofRunLogList == null || !EndofRunLogList.Any())
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            string pageName = "ProcessLog";
            string defaultFileName = $"{pageName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                FileName = defaultFileName,
                Filter = "Excel 文件 (*.xlsx)|*.xlsx"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string path = saveFileDialog.FileName;

                try
                {
                    // 使用 MiniExcel 保存
                    MiniExcel.SaveAs(path, EndofRunLogList);

                    MessageBox.Show("导出成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败: " + ex.Message);
                }
            }
        }
    }
}
