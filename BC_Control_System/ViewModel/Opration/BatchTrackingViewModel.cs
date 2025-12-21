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
using CommunityToolkit.Mvvm.ComponentModel;
using BC_Control_BLL.PLC;

namespace BC_Control_System.ViewModel.Opration
{
    public partial class BatchTrackingViewModel :ObservableObject
    {
        private readonly BatchDataService _batchDataService;
        private readonly IPLCHelper _plcOpration;
        private Dictionary<string,(string,string)> OprationCollection;
        [ObservableProperty]
        private BindingList<ModuleStatus> _ModuleStatusCollection;
        public ICommand ProcessPauseCommand { get; set; }      
        public ICommand ProcessRestartCommand { get; set; }
        public BatchTrackingViewModel(IPLCHelper plcHelper, BatchDataService batchDataService)
        {
            _plcOpration = plcHelper;
            _batchDataService = batchDataService;
            ModuleStatusCollection = new BindingList<ModuleStatus>(_batchDataService.BatchDataCollection);
            ProcessPauseCommand = new RelayCommand<ModuleStatus>(ProcessPause);
            ProcessRestartCommand=new RelayCommand<ModuleStatus>(ProcessRestart);
            AddModuleMessage();
        }       
        private void AddModuleMessage()
        {           
            OprationCollection.Add("Tank6#Dry",("L1000","L1001"));           
            OprationCollection.Add("Tank4#QDR", ("L1000", "L1001"));
            OprationCollection.Add("Tank3#NMP", ("L1000", "L1001"));
            OprationCollection.Add("Tank2#EKC", ("L1000", "L1001"));
            OprationCollection.Add("Tank1#EKC", ("L1000", "L1001"));
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
            catch (Exception ex)
            {

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
            catch (Exception ex)
            {

            }
        }



    }
}
