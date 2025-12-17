using BC_Control_BLL.services.TraceLogService;
using BC_Control_Models;
using BC_Control_Models.Log;
using BC_Control_System.BaseModel;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Control_System.ViewModel.Log.TankTraceLog
{
    class Tank5TraceLogViewModel : InsertTimeLogViewModelBase<TK5DataLogClass, TK5LogDataService>, INavigationAware
    {
        //private List<TK5DataLogClass> insertTimeCollections;
        //public override List<TK5DataLogClass> InsertTimeCollections
        //{
        //    get => insertTimeCollections;
        //    set => SetProperty(ref insertTimeCollections, value);
        //}
        public Tank5TraceLogViewModel(
            ILogCommandService commandService,
            TK5LogDataService dataBaseService,
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
