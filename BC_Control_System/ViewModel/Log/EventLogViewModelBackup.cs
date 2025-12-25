using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BC_Control_BLL;
using BC_Control_BLL.Services;
using BC_Control_Models;
using BC_Control_Models.Personal;
using BC_Control_System.Comand;
using BC_Control_System.Command;
using BC_Control_System.View.Log;

namespace BC_Control_System.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class EventLogViewModelBackup : BindableBase
    {
        private IDialogService _dialogService;
        public ICollectionView CollectionView { get; set; } //这个类感觉用在实时报警上会很好用

        [OnChangedMethod(nameof(FilterTestChanged))]
        public string FilterTest { get; set; }
        public DelegateCommand ExportCommand { get; set; }
        public List<EventLog> EventLogs { get; set; }
        public DelegateCommand SelectByTime { get; set; }
        
        private EventLogService _eventLogService;
        public EventLogViewModelBackup(IRegionManager regionManager, IDialogService dialogService,EventLogService eventLogService)
        {
            _eventLogService= eventLogService;
            _dialogService = dialogService;
            SelectByTime = new DelegateCommand(SelectTime);
            EventLogs = new List<EventLog>();

            CollectionView = CollectionViewSource.GetDefaultView(source: EventLogs);
            CollectionView.Filter = UpDateObserve;
            ExportCommand = new DelegateCommand(ExportData);
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

                    EventLogs = await _eventLogService.Query(filter => (Convert.ToDateTime(filter.InsertTime) >= StartTime) && (Convert.ToDateTime(filter.InsertTime) <= EndTime));
                }

                CollectionView = CollectionViewSource.GetDefaultView(source: EventLogs);
                CollectionView.Filter = UpDateObserve;
                CollectionView.Refresh();
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private bool UpDateObserve(object item)
        {
            if (string.IsNullOrEmpty(FilterTest)) return true;
            var em = item as EventLog;
            return em.VarName.Contains(FilterTest);
        }

        public void FilterTestChanged()
        {
            CollectionView.Refresh();
        }

        public void ExportData()
        {
            if (EventLogs == null || !EventLogs.Any())
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            string pageName = "EventLog";
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
                    MiniExcel.SaveAs(path, EventLogs);
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
