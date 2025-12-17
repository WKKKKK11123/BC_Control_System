using BC_Control_System.View.Log;
using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.Windows;
using BC_Control_BLL.Services;
using BC_Control_Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModels
{
    
    public partial class AlarmLogViewModel : ObservableObject
    {
        private AlarmLogService _sqlSugarHelper;
        private IDialogService _dialogService;
        [ObservableProperty]
        private List<AlarmLog> alarmLogs;
        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand SelectByTime { get; set; }
        public DelegateCommand SortViewCommand { get; set; }
        public DelegateCommand SaveFileCommand { get; set; }
        public AlarmLogViewModel(AlarmLogService sqlSugarHelper, IDialogService dialogService)
        {
            AlarmLogs = new List<AlarmLog>();
            _dialogService = dialogService;
            _sqlSugarHelper = sqlSugarHelper;
            SelectByTime = new DelegateCommand(SelectTime);
            SortViewCommand = new DelegateCommand(SortViewOpen);
            SaveFileCommand = new DelegateCommand(SaveFile);
            ExportCommand = new DelegateCommand(ExportData);
        }
        public void SortViewOpen()
        {
            try
            {
                IDialogResult r = null;
                _dialogService.ShowDialog(nameof(SortView), result => r = result);
            }

            catch (Exception ee)
            {

            }

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
                    AlarmLogs = await _sqlSugarHelper.Query(filter => (Convert.ToDateTime(filter.Date) >= StartTime) && (Convert.ToDateTime(filter.Date) <= EndTime));
                }

            }

            catch (Exception ee)
            {

            }
            
        }
        public void SaveFile()
        {
            _dialogService.ShowDialog(nameof(SaveFileView));
        }

        public void ExportData()
        {
            if (AlarmLogs == null || !AlarmLogs.Any())
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            string pageName = "AlarmLog";
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
                    MiniExcel.SaveAs(path, AlarmLogs);
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
