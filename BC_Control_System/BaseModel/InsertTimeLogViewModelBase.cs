using BC_Control_System.View.Log;
using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_BLL.Oprations;
using BC_Control_BLL.Services;
using BC_Control_Models;
using BC_Control_Models.Log;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace BC_Control_System.BaseModel
{
    public partial class InsertTimeLogViewModelBase<T, TService> : ObservableObject, IDisposable
        where T : class, IInsertTime, new()
        where TService : BaseService<T>
    {
        private readonly ILogCommandService _commandService;
        private readonly BaseService<T> _dataBaseService;
        [ObservableProperty]
        private List<T> insertTimeCollections;
        public InsertTimeLogViewModelBase(
            ILogCommandService commandService,
            BaseService<T> dataBaseService
            )
        {
            _commandService = commandService;
            _dataBaseService = dataBaseService;
            InsertTimeCollections = new List<T>();
            _commandService.SelectTimeCommandExecuted += OnSelectTime;
            _commandService.GetListCommandExecuted += GetInsertTimeData;
            _commandService.GetTypeCommandExecuted += GetTypeFunc;


        }
        public virtual Type GetTypeFunc()
        {
            return typeof(T);
        }
        public virtual async void OnSelectTime(DateTime startTime, DateTime endTime)
        {
            try
            {
                InsertTimeCollections = await _dataBaseService.Query(filter => Convert.ToDateTime(filter.InsertTime) >= startTime && Convert.ToDateTime(filter.InsertTime) <= endTime);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public virtual List<object> GetInsertTimeData()
        {
            try
            {
                return InsertTimeCollections.Select(src => (object)src).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public void Dispose()
        {
            _commandService.SelectTimeCommandExecuted -= OnSelectTime;
            _commandService.GetListCommandExecuted -= GetInsertTimeData;
            _commandService.GetTypeCommandExecuted -= GetTypeFunc;

        }
    }
}
