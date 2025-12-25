using BC_Control_BLL.Services;
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
    public class Tank9TraceLogViewModel : InsertTimeLogViewModelBase<TK9DataLogClass, TK9LogDataService>, INavigationAware
    {
        //private List<TK9DataLogClass> insertTimeCollections;
        //public override List<TK9DataLogClass> InsertTimeCollections
        //{
        //    get => insertTimeCollections;
        //    set => SetProperty(ref insertTimeCollections, value);
        //}
        public Tank9TraceLogViewModel(
            ILogCommandService commandService,
            TK9LogDataService dataBaseService,
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
