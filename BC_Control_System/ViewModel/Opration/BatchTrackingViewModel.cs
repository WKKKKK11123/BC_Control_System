using CommunityToolkit.Mvvm.Input;
using MiniExcelLibs;
using Prism.Mvvm;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ZC_Control_EFAM;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.Personal;

namespace BC_Control_System.ViewModel.Opration
{
    [AddINotifyPropertyChangedInterface]
    public class BatchTrackingViewModel : BindableBase
    {
        private IPLCHelper _plcOpration;
        public List<StorageStation> StorageStations { get; set; }
        private string filepath;
        private Dictionary<string,(string,string)> OprationCollection;
        private List<StatusClass> OverviewList;
        public List<StatusClass> BatchTrackClasses { get; set; }
       
        public ObservableCollection<ModuleStatus> ModuleStatusCollection { get; set; }
        public BindingList<BatchTrackingVo> BatchTrackInProcess { get; set; }
        public ICommand ProcessPauseCommand { get; set; }      
        public ICommand ProcessRestartCommand { get; set; }
        public BatchTrackingViewModel(IPLCHelper plcHelper)
        {
            _plcOpration = plcHelper;
            //StorageStations = processControl.eFAM_Data.Storage_Data;
            filepath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"File\OtherView",
                "Status.xlsx"
            );
            ProcessPauseCommand = new RelayCommand<ModuleStatus>(ProcessPause);
            ProcessRestartCommand=new RelayCommand<ModuleStatus>(ProcessRestart);
            LoadFromExcel();
        }
        private async void LoadFromExcel()
        {
            try
            {
                //string filePath = @"G:\ZC\program\华为BC3100\2025\BC_Control_System0312\BC_Control_System\bin\Debug\File\Overview\Status.xlsx";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"File\Overview\Status.xlsx");
                var sheets = MiniExcel.GetSheetInformations(filePath);
                if (sheets.Any(para => para.Name == "TemperatureControlOffList"))
                {
                    OverviewList = await Task.Run(() => MiniExcel.Query<StatusClass>(filePath, sheetName: "TemperatureControlOffList").Where(prop => !string.IsNullOrEmpty(prop.ParameterName)).ToList());
                }
                else
                {
                    OverviewList = new List<StatusClass>();
                }

                AddModuleMessage();
                await Task.Run(() =>
                {
                    try
                    {
                        while (true)
                        {
                            OverviewList.UpdateStatus();

                            Thread.Sleep(500);
                        }
                    }
                    catch (System.Exception e)
                    {

                    }
                });
            }
            catch (System.Exception ee)
            {

            }

        }

        private void AddModuleMessage()
        {
            List<ModuleStatus> temp = new List<ModuleStatus>();
            ModuleStatusCollection = new ObservableCollection<ModuleStatus>();
            OprationCollection = new Dictionary<string, (string, string)>();
            ModuleStatusCollection.Add(new ModuleStatus()
            {
                ModuleName = "Tank6#Dry",
                BatchID = OverviewList.Find(para => para.ParameterName == "Tank6BatchID"),
                FlowRecipeName = OverviewList.Find(para => para.ParameterName == "Tank6FlowRecipeName"),
                FlowRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank6FlowRecipeStep"),
                UnitRecipeName = OverviewList.Find(para => para.ParameterName == "Tank6UnitRecipeName"),
                UnitRecipeAllTime = OverviewList.Find(para => para.ParameterName == "Tank6UnitRecipeAllTime"),
                UnitRecipeResidueTime = OverviewList.Find(para => para.ParameterName == "Tank6UnitRecipeResidueTime"),
                UnitRecipeOverTime = OverviewList.Find(para => para.ParameterName == "Tank6UnitRecipeOverTime"),
                UnitRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank6UnitRecipeStep"),
                UnitRecipeStepTime = OverviewList.Find(para => para.ParameterName == "Tank6UnitRecipeStepTime"),
                IsWafer = OverviewList.Find(para => para.ParameterName == "Tank6IsWafer")
            });
           
            OprationCollection.Add("Tank6#Dry",("L1000","L1001"));           
            OprationCollection.Add("Tank4#QDR", ("L1000", "L1001"));
            ModuleStatusCollection.Add(new ModuleStatus()
            {
                ModuleName = "Tank4#QDR",
                BatchID = OverviewList.Find(para => para.ParameterName == "Tank4BatchID"),
                FlowRecipeName = OverviewList.Find(para => para.ParameterName == "Tank4FlowRecipeName"),
                FlowRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank4FlowRecipeStep"),
                UnitRecipeName = OverviewList.Find(para => para.ParameterName == "Tank4UnitRecipeName"),
                UnitRecipeAllTime = OverviewList.Find(para => para.ParameterName == "Tank4UnitRecipeAllTime"),
                UnitRecipeResidueTime = OverviewList.Find(para => para.ParameterName == "Tank4UnitRecipeResidueTime"),
                UnitRecipeOverTime = OverviewList.Find(para => para.ParameterName == "Tank4UnitRecipeOverTime"),
                UnitRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank4UnitRecipeStep"),
                UnitRecipeStepTime = OverviewList.Find(para => para.ParameterName == "Tank4UnitRecipeStepTime"),
                IsWafer = OverviewList.Find(para => para.ParameterName == "Tank4IsWafer")
            });
            OprationCollection.Add("Tank3#NMP", ("L1000", "L1001"));
            ModuleStatusCollection.Add(new ModuleStatus()
            {
                ModuleName = "Tank3#NMP",
                BatchID = OverviewList.Find(para => para.ParameterName == "Tank3BatchID"),
                FlowRecipeName = OverviewList.Find(para => para.ParameterName == "Tank3FlowRecipeName"),
                FlowRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank3FlowRecipeStep"),
                UnitRecipeName = OverviewList.Find(para => para.ParameterName == "Tank3UnitRecipeName"),
                UnitRecipeAllTime = OverviewList.Find(para => para.ParameterName == "Tank3UnitRecipeAllTime"),
                UnitRecipeResidueTime = OverviewList.Find(para => para.ParameterName == "Tank3UnitRecipeResidueTime"),
                UnitRecipeOverTime = OverviewList.Find(para => para.ParameterName == "Tank3UnitRecipeOverTime"),
                UnitRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank3UnitRecipeStep"),
                UnitRecipeStepTime = OverviewList.Find(para => para.ParameterName == "Tank3UnitRecipeStepTime"),
                IsWafer = OverviewList.Find(para => para.ParameterName == "Tank3IsWafer"),
            });
            OprationCollection.Add("Tank2#EKC", ("L1000", "L1001"));
            ModuleStatusCollection.Add(new ModuleStatus()
            {
                ModuleName = "Tank2#EKC",
                BatchID = OverviewList.Find(para => para.ParameterName == "Tank2BatchID"),
                FlowRecipeName = OverviewList.Find(para => para.ParameterName == "Tank2FlowRecipeName"),
                FlowRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank2FlowRecipeStep"),
                UnitRecipeName = OverviewList.Find(para => para.ParameterName == "Tank2UnitRecipeName"),
                UnitRecipeAllTime = OverviewList.Find(para => para.ParameterName == "Tank2UnitRecipeAllTime"),
                UnitRecipeResidueTime = OverviewList.Find(para => para.ParameterName == "Tank2UnitRecipeResidueTime"),
                UnitRecipeOverTime = OverviewList.Find(para => para.ParameterName == "Tank2UnitRecipeOverTime"),
                UnitRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank2UnitRecipeStep"),
                UnitRecipeStepTime = OverviewList.Find(para => para.ParameterName == "Tank2UnitRecipeStepTime"),
                IsWafer = OverviewList.Find(para => para.ParameterName == "Tank2IsWafer"),
            });
            OprationCollection.Add("Tank1#EKC", ("L1000", "L1001"));
            ModuleStatusCollection.Add(new ModuleStatus()
            {
                ModuleName = "Tank1#EKC",
                BatchID = OverviewList.Find(para => para.ParameterName == "Tank1BatchID"),
                FlowRecipeName = OverviewList.Find(para => para.ParameterName == "Tank1FlowRecipeName"),
                FlowRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank1FlowRecipeStep"),
                UnitRecipeName = OverviewList.Find(para => para.ParameterName == "Tank1UnitRecipeName"),
                UnitRecipeAllTime = OverviewList.Find(para => para.ParameterName == "Tank1UnitRecipeAllTime"),
                UnitRecipeResidueTime = OverviewList.Find(para => para.ParameterName == "Tank1UnitRecipeResidueTime"),
                UnitRecipeOverTime = OverviewList.Find(para => para.ParameterName == "Tank1UnitRecipeOverTime"),
                UnitRecipeStep = OverviewList.Find(para => para.ParameterName == "Tank1UnitRecipeStep"),
                UnitRecipeStepTime = OverviewList.Find(para => para.ParameterName == "Tank1UnitRecipeStepTime"),
                IsWafer = OverviewList.Find(para => para.ParameterName == "Tank1IsWafer"),
            });
            



        }
        private void ProcessPause(ModuleStatus moduleStatus)
        {
            try
            {
                if (moduleStatus == null)
                {
                    MessageBox.Show("运单选择异常");
                    return;
                }
                if (!_plcOpration.ConnectState())
                {
                    return;
                }
                MessageBoxResult t=MessageBox.Show("是否暂停制程", "Process Pause", MessageBoxButton.OKCancel);
                if (t== MessageBoxResult.OK)
                {
                    _plcOpration.CommonWrite(OprationCollection[moduleStatus.ModuleName].Item1,"True");
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private void ProcessRestart(ModuleStatus moduleStatus)
        {
            try
            {
                if (moduleStatus == null)
                {
                    MessageBox.Show("运单选择异常");
                    return;
                }
                if (!_plcOpration.ConnectState())
                {
                    return;
                }
                MessageBoxResult t = MessageBox.Show("是否暂停制程", "Process Pause", MessageBoxButton.OKCancel);
                if (t == MessageBoxResult.OK)
                {
                    _plcOpration.CommonWrite(OprationCollection[moduleStatus.ModuleName].Item1, "False");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
