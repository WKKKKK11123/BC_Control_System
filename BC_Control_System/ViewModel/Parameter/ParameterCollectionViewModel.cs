using BC_Control_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using MiniExcelLibs;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Prism.Commands;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using BC_Control_System.Command;
using BC_Control_Models.BenchConfig;
using System.Windows;

namespace BC_Control_System.ViewModel.Parameter
{
    public partial class ParameterCollectionViewModel : ObservableObject, INavigationAware
    {
        #region 传递字段
        private readonly ILogOpration _logOpration;
        private IDialogService _dialogService;       
        private string filePath="";
        private List<ParameterModel> historyValues = new List<ParameterModel>();
        private List<ParameterModel> parameterModels = new List<ParameterModel>();
        private IPLCHelper _plcHelper;
        #endregion

        #region 视图属性
        [ObservableProperty]
        private string moduleName="";
        [ObservableProperty]
        private BindingList<ParameterModel> parameterCollections = new BindingList<ParameterModel>();
        [ObservableProperty]
        private BindingList<ParameterModel> parameterGroup = new BindingList<ParameterModel>();
        [ObservableProperty]
        private ParameterModel selelctGroup = new ParameterModel();
        #endregion
        public ParameterCollectionViewModel(IDialogService dialogService, IPLCHelper pLCHelper,ILogOpration logOpration)
        {
            _logOpration = logOpration;
            _plcHelper = pLCHelper;
            parameterModels = new List<ParameterModel>();
            _dialogService = dialogService;
        }

        #region 视图方法
        partial void OnSelelctGroupChanged(ParameterModel value)
        {
            if (string.IsNullOrEmpty(SelelctGroup.Group))
            {             
                SelelctGroup.Group = "All";
            }
            if (SelelctGroup.Group == "All")
            {
                ParameterCollections = new BindingList<ParameterModel>(parameterModels);
            }
            else
            {
                ParameterCollections = new BindingList<ParameterModel>(
                    parameterModels.Where(P => P.Group == SelelctGroup.Group).ToList()
                );
            }
        }
        [RelayCommand]
        private void WriteParameterWithChange()
        {
            if (!_plcHelper.ConnectAll())
            {
                MessageBox.Show("设备未连接");
                return;
            }
            foreach (var parameterModel in parameterModels.Where(filter => filter.IsChange))
            {
                _plcHelper.CommonWrite(
                    parameterModel.ValueAddress,
                    parameterModel.Value,
                    parameterModel.PLC
                );
                parameterModel.IsChange = false;
            }
            historyValues = DeepClone(parameterModels);
            OnSelelctGroupChanged(SelelctGroup);
        }
        [RelayCommand]
        private void ChangeValue(object t)
        {
            if (!(t is ParameterModel))
                return;
            ParameterModel model = (ParameterModel)t;
            DialogParameters keyValuePairs = new DialogParameters();
            keyValuePairs.Add("PLCValue", model);
            keyValuePairs.Add("UpLimit", model.UpLimit);
            keyValuePairs.Add("LowLimit", model.LowLimit);
            _dialogService.ShowDialog(
                "PLCValueView",
                keyValuePairs,
                result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        string paramName = result.Parameters.GetValue<string>("ParamViewValue1");
                        string value = result.Parameters.GetValue<string>("ParamViewValue2");
                        //string S = OldParameterModels.FirstOrDefault(p => p.ParameterName == paramName).Value
                        string oldvalue1 = historyValues
                            .FirstOrDefault(p => p.ParameterName == paramName)!
                            .Value;
                        parameterModels.FirstOrDefault(p => p.ParameterName == paramName)!.Value = value;
                        parameterModels.FirstOrDefault(p => p.ParameterName == paramName)!.IsChange =
                            oldvalue1 != value;
                        OnSelelctGroupChanged(SelelctGroup);
                    }
                }
            );
        }
        [RelayCommand]
        private async Task UploadParameters()
        {
            await UpdateParameter();
            OnSelelctGroupChanged(SelelctGroup);
        }
        #endregion
        #region 私有方法      
        private async Task UpdateParameter()
        {
            await Task.Run(() =>
            {
                try
                {
                    foreach (var parameter in parameterModels)
                    {
                        parameter.Value = _plcHelper.GetValue(parameter);
                        parameter.IsChange = false;
                    }
                    historyValues = DeepClone(parameterModels);
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }
        public static List<T> DeepClone<T>(List<T> list)
        {
            var options = new JsonSerializerOptions
            {
                // 允许循环引用
                ReferenceHandler = ReferenceHandler.Preserve,
                // 包含字段（如果需要）
                IncludeFields = true,
                // 使用宽松的属性名称匹配
                PropertyNameCaseInsensitive = true,
                // 写入缩进的JSON（调试时有用，生产环境可设为false）
                WriteIndented = false
            };

            string jsonString = JsonSerializer.Serialize(list, options);
            return JsonSerializer.Deserialize<List<T>>(jsonString, options);
        }
        private async Task WriteParameterFileAsync()
        {
            try
            {
                List<ParameterModel> parametertemp = new List<ParameterModel>();
                parametertemp.Add(new ParameterModel());
                await Task.Run(
                    () =>
                        Task.Run(
                            () =>
                                MiniExcel.SaveAs(filePath, parametertemp, excelType: ExcelType.XLSX)
                        )
                );
            }
            catch (Exception ee) { }
        }
        #endregion
        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {           
            var tempmodule = navigationContext.Parameters["ModuleName"] as StationCollection ?? new StationCollection();           
            ModuleName = $"Parameter({tempmodule.StationName})";    
            parameterModels = tempmodule.ParameterCollections;
            ParameterCollections = new BindingList<ParameterModel>(parameterModels);
            await UpdateParameter();
            List<string> tempgroup = new List<string>();
            tempgroup.Add("All");
            tempgroup.AddRange(tempmodule.ParameterCollections.Select(group => group.Group).Distinct());
            ParameterGroup = new BindingList<ParameterModel>(
                tempgroup
                    .Select(
                        (param, index) => new ParameterModel() { No = index + 1, Group = param }
                    )
                    .ToList()
            );
           
        }
        #endregion
    }
}
