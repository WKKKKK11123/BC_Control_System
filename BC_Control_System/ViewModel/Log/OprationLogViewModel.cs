using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BC_Control_BLL.Services;
using BC_Control_DAL;
using BC_Control_Models;
using BC_Control_System.Comand;
using BC_Control_System.Command;
using BC_Control_System.View.Log;

namespace BC_Control_System.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class OprationLogViewModel : BindableBase
    {
        public OprationLogViewModel(OperatorLogService oprationLogService,IDialogService dialogService)
        {
           _operatorLogService = oprationLogService;
            _dialogService = dialogService;
            SelectByTime = new DelegateCommand(SelectTime);
            ExportCommand = new DelegateCommand(ExportData);
        }
        private IDialogService _dialogService;
        private OperatorLogService _operatorLogService;
        public DelegateCommand ExportCommand { get; set; }
        public DateTime FromTime { get; set; } = DateTime.Now;

        public DateTime ToTime { get; set; } = DateTime.Now;

        public DelegateCommand SelectByTime { get; set; }

        public List<OperatorLog> Logs { get; set; }
       
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
                    //alarmLogs = await _sqlSugarHelper.Query(filter => (Convert.ToDateTime(filter.Date) >= StartTime) && (Convert.ToDateTime(filter.Date) <= EndTime));
                    Logs = await _operatorLogService.Query(filter => (Convert.ToDateTime(filter.InsertTime) >= StartTime) && (Convert.ToDateTime(filter.InsertTime) <= EndTime));
                }
            }
            catch (Exception EE)
            {

                
            }
            
        }

        public void ExportData()
        {
            if (Logs == null || !Logs.Any())
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            string pageName = "OperatorLog";
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
                    MiniExcel.SaveAs(path, Logs);

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
