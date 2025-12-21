using BC_Control_System.Command;
using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using BC_Control_Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModel.Parameter
{
    public partial class ParameterViewModel : ObservableObject, INavigationAware
    {
        #region 传递字段

        private IDialogService _dialogService;
        private string moduleName="";
        private string filePath = "";
        private List<ParameterModel> historyValues = new List<ParameterModel>();
        private List<ParameterModel> parameterModels = new List<ParameterModel>();
        private IPLCHelper _plcHelper;
        #endregion

        #region
        [ObservableProperty]
        private string _ModuleName="";
        
        [ObservableProperty]
        private ParameterModel _SelelctGroup=new ParameterModel();
        [ObservableProperty]
        private BindingList<ParameterModel> _Parameters=new BindingList<ParameterModel>();
        [ObservableProperty]
        private BindingList<ParameterModel> _ParameterGroup=new BindingList<ParameterModel>();
        public DelegateCommand<object> ChangeValueCommand { get; set; }
        public DelegateCommand WriteParameterWithChangeCommand { get; set; }
        public DelegateCommand LoadParameterWithCommand { get; set; }
        #endregion
        public ParameterViewModel(IDialogService dialogService,IPLCHelper pLCHelper)
        {
            _plcHelper= pLCHelper;
            parameterModels = new List<ParameterModel>();
            _dialogService = dialogService;
            ChangeValueCommand = new DelegateCommand<object>(ChangeValue);
            WriteParameterWithChangeCommand = new DelegateCommand(WriteParameterWithChange);
            LoadParameterWithCommand = new DelegateCommand(UploadParameters);
        }
        partial void OnSelelctGroupChanged(ParameterModel value)
        {
            OnGroupChangeValue();
        }
        private void OnGroupChangeValue()
        {
            if (SelelctGroup == null)
            {
                SelelctGroup = new ParameterModel();
                SelelctGroup.Group = "All";
            }
            if (SelelctGroup.Group == "All")
            {
                Parameters = new BindingList<ParameterModel>(parameterModels);
            }
            else
            {
                Parameters = new BindingList<ParameterModel>(
                    parameterModels.Where(P => P.Group == SelelctGroup.Group).ToList()
                );
            }
        }

        private void ChangeValue(object t)
        {
            if (!(t is ParameterModel))
                return;
            ParameterModel model = (ParameterModel)t;
            DialogParameters keyValuePairs = new DialogParameters();
            keyValuePairs.Add("PLCValue", model);
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
                            .FirstOrDefault(p => p.ParameterName == paramName)
                            .Value;
                        parameterModels.FirstOrDefault(p => p.ParameterName == paramName).Value = value;
                        parameterModels.FirstOrDefault(p => p.ParameterName == paramName).IsChange =
                            oldvalue1 != value;
                        OnGroupChangeValue();
                    }
                }
            );
        }

        public void WriteParameterWithChange()
        {
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
            OnGroupChangeValue();
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
        public async void UploadParameters()
        {
            await UpdateParameter();
            OnGroupChangeValue();
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

        private async Task ReadParameterFileAsync()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    await WriteParameterFileAsync();
                }
                parameterModels = await Task.Run(
                    () =>
                        Task.Run(
                            () =>
                                MiniExcel
                                    .Query<ParameterModel>(filePath)
                                    .Where(prop => !string.IsNullOrEmpty(prop.ParameterName))
                                    .ToList()
                        )
                );
            }
            catch (Exception ee) { }
        }

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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            moduleName = navigationContext.Parameters["ModuleName"] as string;
            ModuleName = $"Parameter({moduleName})";
            filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"File",
                moduleName,
                @"Parameter.xlsx"
            );

            await ReadParameterFileAsync();


            //historyValues = parameterModels;
            Parameters = new BindingList<ParameterModel>(parameterModels);

            foreach (ParameterModel item in Parameters)
            {
                CommonStaticMethods.SetVariableType(item.ValueAddress, item.DataType, item.PLC);
            }


            Thread.Sleep(300);
            await UpdateParameter();
            List<string> tempgroup = new List<string>();
            tempgroup.Add("All");
            tempgroup.AddRange(parameterModels.Select(group => group.Group).Distinct());
            ParameterGroup = new BindingList<ParameterModel>(
                tempgroup
                    .Select(
                        (param, index) => new ParameterModel() { No = index + 1, Group = param }
                    )
                    .ToList()
            );
        }
    }
}
