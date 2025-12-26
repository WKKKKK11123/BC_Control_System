using BC_Control_System.View.Opration;
using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.Collections;
using System.Windows;
using ZC_Control_EFAM;
using ZC_Control_EFAM.ProcessControl;
using BC_Control_Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BC_Control_System.ViewModel.Opration
{
    public partial class CarrierTrackViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<StorageStation> _StorageStations;
        private IDialogService _dialogService;

        private ProcessControl _processControl;
        public bool ControlMode { get; set; }       
        public DelegateCommand<IList> ProcessStartCommand { get; set; }
        public DelegateCommand<StorageStation> CarrierOutCommand { get; set; }
       
        public AsyncRelayCommand ChangeWaferThickness { get; set; }

        private void OnCarrierCountChang()
        {
            //StorageCollections = new BindingList<StorageCollection>(_eFAMMessage.StorageCollectionList);
        }

        public CarrierTrackViewModel(
            ProcessControl processControl,
            IDialogService dialogService
        )
        {
            _processControl = processControl;
            StorageStations = processControl.eFAM_Data.Storage_Data;    
            _dialogService = dialogService;
            ControlMode = processControl.Auto;

            ProcessStartCommand = new DelegateCommand<IList>(ProcessStart);
            CarrierOutCommand = new DelegateCommand<StorageStation>(CarrierOutFunc);
            //ChangeDataCommand = new DelegateCommand<StorageCollection>(ChangeData);
            //RemoveCarrierCommand = new DelegateCommand<StorageCollection>(RemoveCarrier);
        }


        private void ProcessStart(IList carrierTrackClasses1)
        {

            try
            {
                List<StorageStation> carrierTrackClasses = carrierTrackClasses1
                    .Cast<StorageStation>()
                    .ToList();
                if (carrierTrackClasses.Count > 2 || carrierTrackClasses.Count == 0)
                {
                    MessageBox.Show("最多只能选择两个运单进行跟踪！");
                    return;
                }
                if (
                    carrierTrackClasses
                        .Where(para => (para.StationInfo.ProcessState != ProcessState.UnProcess))
                        .Count() != 0
                )
                {
                    MessageBox.Show("选择的运单异常！");
                    return;
                }

                if (carrierTrackClasses.Count(c => c.StationInfo.MAPError) > 0)
                {
                    MessageBox.Show("选择的运单有MAP异常，请检查选择的运单！");
                    return;
                }
                DialogParameters key = new DialogParameters();
                IDialogResult r = new DialogResult();
                key.Add("CarrierTrackClasses", carrierTrackClasses);

                _dialogService.ShowDialog(nameof(ProcessStartView), key, result => r = result);
                if (r.Result != ButtonResult.OK)
                {
                    return;
                }
                var RecipeName = r.Parameters.GetValue<string>("Value2");
                var batchid = DateTime.Now.ToString("yyyyMMddHHmmss_") + r.Parameters.GetValue<string>("Value1");


                _processControl.CarrierProcessQueue.BatchidCollection.Add(
                    new CarrierProcessQueueModel()
                    {
                        Batchid = batchid,
                        BatchState = BatchState.Process,
                        Priority = 0,
                        RecipeName = RecipeName
                    }
                );
                _processControl.CarrierProcessQueue.OnPropertyChanged(string.Empty);
                foreach (var item in carrierTrackClasses)
                {
                    if (carrierTrackClasses.Count > 1)
                    {
                        item.StationInfo.DoubleProcess = true;
                        carrierTrackClasses[0].StationInfo.OddEven = OddEven.Odd;
                        carrierTrackClasses[1].StationInfo.OddEven = OddEven.Even;
                    }
                    else
                    {
                        item.StationInfo.DoubleProcess = false;
                        carrierTrackClasses[0].StationInfo.OddEven = OddEven.Odd;
                    }

                    item.StationInfo.RecipeName = RecipeName;
                    item.StationInfo.Batchid.Batchid = batchid;
                    item.StationInfo.Batchid.BatchState = BatchState.Process;
                    item.StationInfo.Batchid.Priority = 0;
                    item.StationInfo.Batchid.RecipeName = RecipeName;

                    item.StationInfo.ProcessState = ProcessState.WaitProcess;
                }

                //await _ctcSystemService.UploadFromStorage(
                //    carrierTrackClasses.ToList(),
                //    RecipeName,
                //    batchid
                //);
            }
            catch (Exception ee)
            {

            }
        }

        private void CarrierOutFunc(StorageStation storageCollection)
        {
            try
            {
                if (!(storageCollection.StationInfo.ProcessState == ProcessState.UnProcess || storageCollection.StationInfo.ProcessState == ProcessState.Processed))
                {
                    MessageBox.Show("选择的运单异常！");
                    return;
                }
                DialogParameters keyValuePairs = new DialogParameters();
                IDialogResult dialogResult = new DialogResult();
                keyValuePairs.Add("Param1", storageCollection.StationInfo.RFID);
                keyValuePairs.Add("Param2", storageCollection.StationInfo.Form_LP);
                _dialogService.ShowDialog(nameof(CarrierOut), keyValuePairs, new Action<IDialogResult>(src => dialogResult = src));
                if (dialogResult.Result == ButtonResult.OK)
                {
                    string outlpCarrierID = dialogResult.Parameters.GetValue<string>("Value1");
                    int outlpStationID = dialogResult.Parameters.GetValue<int>("Value2");
                    _processControl.SetStorageOutLP(outlpCarrierID, outlpStationID);
                }
            }
            catch (Exception EX)
            {


            }
        }
        private async Task ChangeWaferThicklessFunc()
        {
            try
            {
                DialogParameters key = new DialogParameters();
                IDialogResult r = new DialogResult();
                key.Add("Param1", _processControl.eFAM_Data.WaferSpecification);
                _dialogService.ShowDialog(
                        nameof(ChangeWaferThicknessView),
                        key,
                        result => r = result
                    );
                if (r.Result != ButtonResult.OK)
                {
                    return;
                }
                int tempThickness = r.Parameters.GetValue<int>("Value1");
                await Task.Run(() =>
                {
                    _processControl.eFAM_Data.WaferSpecification = tempThickness;
                });
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }


        }
        private void ChangeData(StorageCollection storageCollection)
        {
            try
            {
                DialogParameters key = new DialogParameters();
                IDialogResult r = new DialogResult();
                key.Add("Param1", false);
                key.Add("Param2", storageCollection.CarrierID);
                key.Add("Param3", storageCollection.Location);
                _dialogService.ShowDialog(
                    nameof(ChangeStorageMessageView),
                    key,
                    result => r = result
                );
                if (r.Result != ButtonResult.OK)
                {
                    return;
                }
                //TODO: 打开修改界面
             
            }
            catch (Exception EE) { }
        }

        private void RemoveCarrier(StorageCollection storageCollection)
        {
            MessageBoxResult r = MessageBox.Show(
                $"是否确定删除 位置{storageCollection.Location} 运单？",
                "提示",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question
            );
            if (r != MessageBoxResult.OK)
            {
                return;
            }
            //TODO: 移除运单
            
        }
    }
}
