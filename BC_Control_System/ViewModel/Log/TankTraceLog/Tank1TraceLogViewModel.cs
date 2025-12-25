using BC_Control_System.BaseModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NPOI.SS.Formula.Functions;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Oprations;
using BC_Control_Models;
using BC_Control_Models.Log;
using BC_Control_BLL.services.TraceLogService;

namespace BC_Control_System.ViewModel.Log.TankTraceLog
{
    public class Tank1TraceLogViewModel :InsertTimeLogViewModelBase<TK1DataLogClass, TK1LogDataService>, INavigationAware
    {
        //private List<TK1DataLogClass> insertTimeCollections;
        //public override List<TK1DataLogClass> InsertTimeCollections 
        //{
        //    get => insertTimeCollections;
        //    set => SetProperty(ref insertTimeCollections, value);
        //}
        public Tank1TraceLogViewModel(
            ILogCommandService commandService, 
            TK1LogDataService dataBaseService, 
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
