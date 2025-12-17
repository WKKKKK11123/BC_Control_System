using BC_Control_System.Command;
using MiniExcelLibs;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.IO;
using BC_Control_Helper;
using BC_Control_Models;

namespace BC_Control_System.ViewModel.Status
{
    [AddINotifyPropertyChangedInterface]
    public class StatusMainViewModel : BindableBase, INavigationAware
    {
        #region 传递字段

        private IDialogService _dialogService;
        private string moduleName;
        private string filePath;
        private IRegionManager _regionManager;
        private List<StatusClass> TemperatureControlOffList = new List<StatusClass>();
        private List<StatusClass> ModuleStatus = new List<StatusClass>();
        private List<DataClass> dataClasses = new List<DataClass>();
        private List<DataClass> ioStatus = new List<DataClass>();
        #endregion

        #region
        public string ModuleName { get; set; }
        #endregion

        public StatusMainViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            ModuleName = "";
            filePath = "";
            _regionManager = regionManager;
            _dialogService = dialogService;
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["ModuleName"] is string)
            {
                moduleName = navigationContext.Parameters["ModuleName"] as string;
                ModuleName = $"------{moduleName}------";
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"File", moduleName, @"Status.xlsx");
                await ReadParameterFileAsync();
            }
            else
            {
                var moduleEntity = navigationContext.Parameters["ModuleName"] as TKClass;
                moduleName = moduleEntity.ModuleName;
                ModuleName = $"------Tank{moduleEntity.TankNo}#{moduleName}------";
                VariableConvertToStatusCollection(moduleEntity);
            }
            OpenModuleView();
            OpenIOView();

        }
        private void VariableConvertToStatusCollection(TKClass moduleEntity)
        {
            try
            {
                TemperatureControlOffList = moduleEntity.StatusCollections
                        .Select(src => src.ToStatusClass()).ToList();
                dataClasses = moduleEntity.DataCollections
                            .Select(src => src.ToDataClass()).ToList();
                ModuleStatus = moduleEntity.BatchStatussCollections
                            .Select(src => src.ToStatusClass()).ToList();
                ioStatus = moduleEntity.OprationCollections
                    .Select(src => src.ToDataClass()).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void OpenModuleView()
        {
            NavigationParameters keys = new NavigationParameters();
            keys.Add("TemperatureControlOffList", TemperatureControlOffList);
            keys.Add("ModuleStatus", ModuleStatus);
            keys.Add("ModuleDatas", dataClasses);
            _regionManager.Regions["ModuleContentRegion"].RequestNavigate("HorizontalStateView", keys);
        }
        private void OpenIOView()
        {
            NavigationParameters keys = new NavigationParameters();
            keys.Add("IOStatus", ioStatus);
            _regionManager.Regions["IOContentRegion"].RequestNavigate($"{moduleName}IOView", keys);
        }
        private async Task WriteParameterFileAsync()
        {
            try
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                List<StatusClass> statusClasstemp = new List<StatusClass>();
                List<DataClass> dataClassesstemp = new List<DataClass>();
                statusClasstemp.Add(new StatusClass());
                keyValuePairs["TemperatureControlOffList"] = statusClasstemp;
                keyValuePairs["ModuleStatus"] = statusClasstemp;
                keyValuePairs["ModuleData"] = dataClassesstemp;

                await Task.Run(() => Task.Run(() => MiniExcel.SaveAs(filePath, keyValuePairs, excelType: ExcelType.XLSX)));
            }
            catch (Exception ee)
            {

            }
        }
        private async Task ReadParameterFileAsync()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    await WriteParameterFileAsync();
                }
                var sheets = MiniExcel.GetSheetInformations(filePath);
                if (sheets.Any(para => para.Name == "TemperatureControlOffList"))
                {
                    TemperatureControlOffList = await Task.Run(() => MiniExcel.Query<StatusClass>(filePath, sheetName: "TemperatureControlOffList").Where(prop => !string.IsNullOrEmpty(prop.ParameterName)).ToList());
                }
                else
                {
                    TemperatureControlOffList = new List<StatusClass> { };
                }
                if (sheets.Any(para => para.Name == "ModuleStatus"))
                {
                    ModuleStatus = await Task.Run(() => MiniExcel.Query<StatusClass>(filePath, sheetName: "ModuleStatus").Where(prop => !string.IsNullOrEmpty(prop.ParameterName)).ToList());
                }
                else
                {
                    ModuleStatus = new List<StatusClass> { };
                }
                if (sheets.Any(para => para.Name == "ModuleData"))
                {
                    dataClasses = await Task.Run(() => MiniExcel.Query<DataClass>(filePath, sheetName: "ModuleData").Where(prop => !string.IsNullOrEmpty(prop.ParameterName)).ToList());
                }
                else
                {
                    dataClasses = new List<DataClass>();
                }
                if (sheets.Any(para => para.Name == "IOStatus"))
                {
                    ioStatus = await Task.Run(() => MiniExcel.Query<DataClass>(filePath, sheetName: "IOStatus").Where(prop => !string.IsNullOrEmpty(prop.ParameterName)).ToList());
                }
                else
                {
                    ioStatus = new List<DataClass>() { };
                }
                TemperatureControlOffList.AnalysisStatusAttribute();
                ModuleStatus.AnalysisStatusAttribute();
                foreach (IPLCValue item in TemperatureControlOffList)
                {
                    CommonStaticMethods.SetVariableType(item.ValueAddress, item);
                }
                foreach (IPLCValue item in ModuleStatus)
                {
                    CommonStaticMethods.SetVariableType(item.ValueAddress, item);
                }
                foreach (IPLCValue item in dataClasses)
                {
                    CommonStaticMethods.SetVariableType(item.ValueAddress, item);
                }
                foreach (IPLCValue item in ioStatus)
                {
                    CommonStaticMethods.SetVariableType(item.ValueAddress, item);
                }
            }
            catch (Exception ee)
            {


            }

        }
    }
}
