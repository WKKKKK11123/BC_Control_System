using BC_Control_BLL.Oprations;
using BC_Control_BLL.Services;
using BC_Control_Models;
using BC_Control_Models.Personal;
using BC_Control_System.BaseModel;
using BC_Control_System.View.Log;
using CommunityToolkit.Mvvm.Input;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel
{
    public partial class EventLogViewModel : InsertTimeLogViewModelBase<EventLog, EventLogService>
    {
        private readonly IDialogService _dialogService;
        public EventLogViewModel(ILogCommandService commandService, EventLogService dataBaseService,
            IExcelOperation excelOperation,
           IDialogService dialogService) : base(commandService, dataBaseService)
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
                var list = GetInsertTimeData();
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}
