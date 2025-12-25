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
using BC_Control_BLL.services.TraceLogService;
using BC_Control_Models.Log;
using BC_Control_System.BaseModel;
using Prism.Regions;
using CommunityToolkit.Mvvm.Input;

namespace BC_Control_System.ViewModels
{
    
    public partial class AlarmLogViewModel : InsertTimeLogViewModelBase<AlarmLog, AlarmLogService>
    {
        private readonly IDialogService _dialogService;
        public AlarmLogViewModel(
           ILogCommandService commandService,
           AlarmLogService dataBaseService,
           IExcelOperation excelOperation,
           IDialogService dialogService)
           : base(commandService, dataBaseService)
        {
            _dialogService = dialogService;
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
                    OnSelectTime(StartTime, EndTime);
                }
            }
            catch (Exception ee)
            {

            }

        }
        [RelayCommand]
        private void Export()
        {
            try
            {
                string path = "";
                var list=GetInsertTimeData();               
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}
