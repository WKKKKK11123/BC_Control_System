using BC_Control_BLL.services;
using BC_Control_Models.Log;
using BC_Control_Models;
using BC_Control_System.BaseModel;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.services.TraceLogService;

namespace BC_Control_System.ViewModel.Log.TankTraceLog
{
    public class Tank11TraceLogViewModel : InsertTimeLogViewModelBase<TK11DataLogClass, TK11LogDataService>, INavigationAware
    {
        public Tank11TraceLogViewModel(
            ILogCommandService commandService,
            TK11LogDataService dataBaseService,
            IExcelOperation excelOperation,
            IDialogService dialogService)
            : base(commandService, dataBaseService)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Dispose();
        }
    }
}
