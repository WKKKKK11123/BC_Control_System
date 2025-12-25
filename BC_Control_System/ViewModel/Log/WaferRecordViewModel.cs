using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using BC_Control_DAL;
using BC_Control_Models.ClassK.SQLService;
using BC_Control_Models.Log;
using BC_Control_System.View.Log;

namespace BC_Control_System.ViewModel.Log
{
    public class WaferRecordViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;
        private readonly SqlSugarHelper<EndofRunData> _sqlSugarHelper = new SqlSugarHelper<EndofRunData>();
        public ObservableCollection<WaferSummary> WaferSummaries { get; set; } = new ObservableCollection<WaferSummary>();
        public DelegateCommand SelectByTime { get; set; }
        public WaferRecordViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            SelectByTime = new DelegateCommand(SelectTime);
        }

        private async void SelectTime()
        {
            IDialogResult r = null;
            _dialogService.ShowDialog(nameof(SearchByTimeView), result => r = result);
            if (r?.Result == ButtonResult.OK)
            {
                var start = r.Parameters.GetValue<DateTime>("Time1");
                var end = r.Parameters.GetValue<DateTime>("Time2");

                var list = await _sqlSugarHelper.Query(x => x.StartTime >= start && x.StartTime <= end);

                WaferSummaries.Clear();

                if (list.Count > 0)
                {
                    var summary = new WaferSummary
                    {
                        StartTime = list.Min(x => x.StartTime),
                        EndTime = list.Max(x => x.EndTime ?? DateTime.Now),
                        BatchCount = list.Count(),
                        TotalWaferCount = list.Sum(x =>
                        {
                            int.TryParse(x.ReturnWaferCount, out int count);
                            return count;
                        })
                    };

                    WaferSummaries.Add(summary);
                }
            }
        }
    }
}
