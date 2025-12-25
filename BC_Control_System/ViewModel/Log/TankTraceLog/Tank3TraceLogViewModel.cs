using BC_Control_Models.Log;
using BC_Control_Models;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_System.BaseModel;
using BC_Control_BLL.services.TraceLogService;

namespace BC_Control_System.ViewModel.Log.TankTraceLog
{
    class Tank3TraceLogViewModel : InsertTimeLogViewModelBase<TK3DataLogClass, TK3LogDataService>, INavigationAware
    {
        //private List<TK3DataLogClass> insertTimeCollections;
        //public override List<TK3DataLogClass> InsertTimeCollections
        //{
        //    get => insertTimeCollections;
        //    set => SetProperty(ref insertTimeCollections, value);
        //}
        public Tank3TraceLogViewModel(
            ILogCommandService commandService,
            TK3LogDataService dataBaseService,
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
