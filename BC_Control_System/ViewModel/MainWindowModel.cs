using BC_Control_System.Command;
using BC_Control_System.Event;
using BC_Control_System.Service;
using BC_Control_System.Theme;
using BC_Control_System.view.Log.TraceLogViews;
using BC_Control_System.View.Opration;
using BC_Control_System.View.Recipe.ModelRecipe;
using BC_Control_System.Views.IO;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using BC_Control_BLL.Services;
using BC_Control_DAL;
using ZC_Control_EFAM;
using ZC_Control_EFAM.ProcessControl;
using BC_Control_Helper;
using BC_Control_Helper.RecipeDownLoad;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using BC_Control_Models.ClassK.SQLService;
using BC_Control_Models.Log;
using ZC_Control_System.EFAMAction;

//using static System.Net.WebRequestMethods;
using Path = System.IO.Path;
using System.Collections.ObjectModel;
using BC_Control_BLL.services;
using BC_Control_BLL.recipedownload;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot.Interactivity;
using System.Windows.Shapes;

namespace BC_Control_System.ViewModel
{
    public partial class MainWindowModel : ObservableObject, IConfigureService
    {
        #region MyRegion
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly AuthService _authManager;
        private readonly ViewTransitionNavigator _viewTransitionNavigator;
        private readonly IBenchStationEntity _benchStationEntity;
        private readonly BenchStationService _benchStationManager;
        private readonly IPLCHelper _plcHelper;
        private readonly DataBaseAddService _logAddService;
        private readonly ThemeManager _themeManager;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly ILogOpration logOpration;
        private readonly IContainerProvider _containerProvider;
        private readonly SysAdmin _userAdmin;
        string filepath = "C:\\212Recipe\\Tool";
        private ProcessControl _processControl;
        #endregion

        private Thread BackThread;
        public string LoginName { get; set; } = "";
        [ObservableProperty]
        private AuthService _AuthManager;
        [ObservableProperty]
        private string _MachineState = "";
        [ObservableProperty]
        private string _ControlState = "";
        [ObservableProperty]
        private bool _Btn_1;
        [ObservableProperty]
        private bool _Btn_2;
        [ObservableProperty]
        private bool _Btn_3;
        [ObservableProperty]
        private bool _Btn_4;
        [ObservableProperty]
        private bool _Btn_5;
        [ObservableProperty]
        private bool _Btn_6;
        [ObservableProperty]
        private bool _Btn_7;
        [ObservableProperty]
        private bool _Btn_8;
        [ObservableProperty]
        public bool _Btn_9;
        [ObservableProperty]
        private DateTime _DateTime;
        [ObservableProperty]
        private ObservableCollection<StationCollection> _ProcessTankCollections;
        [ObservableProperty]
        private ObservableCollection<StationCollection> _BufferTankCollections;
        [ObservableProperty]
        private ObservableCollection<StationCollection> _MachineCollections;
        #region 视图命令
        public ICommand LogoutCommand { get; }
        public ICommand OpenViewCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand OpenParameterCommand { get; set; }
        public ICommand OpenModuleRecipeViewCommand { get; set; }
        public ICommand OpenStatusViewCommand { get; set; }
        public ICommand OpenDialogViewCommand { get; set; }
        public ICommand OpenIOViewCommand { get; set; }
        public ICommand EFAMConnectCommand { get; set; }
        public ICommand OpenTraceLogViewCommand { get; set; }
        public ICommand WritePLCCommand { get; set; }
        public ICommand WriteToolRecipeCommand { get; set; }
        public ICommand CloseCommand =>
            new RelayCommand(() =>
            {
                var result = MessageBox.Show("Do you want to close the program？", "Shutdown",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _plcHelper.CloseAll();
                    Application.Current.Shutdown();
                }
            });
        #endregion
        public MainWindowModel(
            ProcessControl processControl,
            IRegionManager regionManager,
            IDialogService dialogService,
            IContainerProvider containerProvider,
            GlobalVariable globalVariable,
            AuthService authService,
            SysAdmin sysAdmin,
            IEventAggregator eventAggregator,
            ViewTransitionNavigator viewTransitionNavigator
        )
        {

            try
            {

                ProcessTankCollections = new ObservableCollection<StationCollection>();
                BufferTankCollections = new ObservableCollection<StationCollection>();
                MachineCollections = new ObservableCollection<StationCollection>();
                _viewTransitionNavigator = viewTransitionNavigator;
                this._eventAggregator = eventAggregator;
                this._regionManager = regionManager;
                _containerProvider = containerProvider;
                logOpration = containerProvider.Resolve<ILogOpration>();
                _logAddService = containerProvider.Resolve<DataBaseAddService>();
                #region PLCInfo
                _plcHelper = containerProvider.Resolve<IPLCHelper>();
                _plcHelper.Device_AlarmTrigEvent += _logAddService.Device_AlarmTrigEvent;
                _plcHelper.Device_OperatorTrigEvent += _logAddService.Device_OperatorTrigEvent;
                _plcHelper.Device_TrigEvent += _logAddService.Device_TrigEvent;
                _plcHelper.LoadInfo();
                #endregion


                cts = new CancellationTokenSource();
                _regionManager =
                    regionManager ?? throw new ArgumentNullException(nameof(regionManager));

                _dialogService =
                    dialogService ?? throw new ArgumentNullException(nameof(dialogService));
                _processControl = processControl;
                _processControl.WTRGetCompleteEvent += _processControl_WTRGetCompleteEvent;
                _processControl.WTRPutCompleteEvent += _processControl_WTRPutCompleteEvent;
                _processControl.WTSDownRecipe += _processControl_WTSDownRecipe;
                #region Command
                LogoutCommand = new DelegateCommand(ExecuteLogout);
                OpenFolderCommand = new DelegateCommand<object>(OpenFolderDialogView);
                OpenParameterCommand = new DelegateCommand<object>(OpenParameter);
                OpenDialogViewCommand = new DelegateCommand<string>(OpenDialogView);
                OpenViewCommand = new DelegateCommand<string>(src => _viewTransitionNavigator.MainVeiwNavigation(src));
                OpenTraceLogViewCommand = new DelegateCommand<object>
                    (
                    src => _viewTransitionNavigator.MainVeiwNavigation(nameof(InsertTimeLogMainView), new NavigationParameters { { "ModuleName", src } })
                    );
                WriteToolRecipeCommand = new DelegateCommand(WriteToolRecipe);
                OpenStatusViewCommand = new DelegateCommand<object>(OpenStatusView);
                OpenIOViewCommand = new DelegateCommand<string>(OpenIOView);
                EFAMConnectCommand = new DelegateCommand<string>(EFAMConnect);
                WritePLCCommand = new DelegateCommand<string>(WritePLC);
                #endregion
                DateTime = DateTime.Now;

                OpenModuleRecipeViewCommand = new DelegateCommand<object>(OpenRecipe);
                _benchStationManager = containerProvider.Resolve<BenchStationService>();
                _benchStationEntity = containerProvider.Resolve<IBenchStationEntity>();
                _themeManager = new ThemeManager();
                _themeManager.RegisterTheme("Light", "BC_Control_System", "Theme/LightTheme.xaml");
                _themeManager.RegisterTheme("Dark", "BC_Control_System", "Theme/DarkTheme.xaml");
                DataClassHelper.Initialize(_plcHelper);
                StatusClassHelper.Initialize(_plcHelper);

                BackThread = new Thread(UpdateUIStatus);
                BackThread.IsBackground = true;
                BackThread.Start();
                LoadMainViewInfo();
                _logAddService.AddActualLogAsync(cts);
                CommunicateMessageWithPLC(cts);
                BeatHeart(cts, "M4120", PlcEnum.PLC1);
                PusherCommunicateWithPLC();
                _authManager = authService;
                AuthManager = authService;
                LoginName = AuthManager.UserName;


            }
            catch (Exception ex)
            {
                logOpration.WriteError(ex.ToString());
                MessageBox.Show($"软件配置加载异常 {ex.ToString()}");
                throw;
            }

        }
        private async void ExecuteLogout()
        {
            // 显示确认弹窗
            var result = await ShowLogoutConfirmationDialogAsync();

            if (result == ButtonResult.OK)
            {
                // 清除登录状态
                _authManager.Logout();
            }
        }
        private Task<ButtonResult> ShowLogoutConfirmationDialogAsync()
        {
            var tcs = new TaskCompletionSource<ButtonResult>();

            _dialogService.ShowDialog(
                "NotificationDialog",
                new DialogParameters
                {
            { "Message", "Are you sure you want to logout?" },
            { "ConfirmButtonText", "Yes" },
            { "CancelButtonText", "No" }
                },
                result => tcs.SetResult(result.Result)
            );

            return tcs.Task;
        }
        private void PusherCommunicateWithPLC()
        {
            Task.Run(() =>
            {
                bool WTRAllowPut, PusherRequestGet, PusherRequestPut, BufferPlaceSenser, PusherPlaceSenser;
                while (true)
                {
                    try
                    {
                        DateTime = DateTime.Now;
                        if (_plcHelper.ConnectAll())
                        {
                            WTRAllowPut = _processControl.WTRAllowPut;
                            PusherRequestGet = _processControl.PusherRequestGet;
                            PusherRequestPut = _processControl.PusherRequestPut;
                            BufferPlaceSenser = _processControl.BufferPlaceSenser;
                            PusherPlaceSenser = _processControl.PusherPlaceSenser;

                            var a =
                                (bool)CommonMethods.Device["M4130"] != WTRAllowPut
                                    ? CommonMethods.CommonWrite("M4130", WTRAllowPut.ToString())
                                    : false;
                            a =
                                (bool)CommonMethods.Device["M4131"] != PusherRequestGet
                                    ? CommonMethods.CommonWrite(
                                        "M4131",
                                        PusherRequestGet.ToString()
                                    )
                                    : false;
                            a =
                                (bool)CommonMethods.Device["M4132"] != PusherRequestPut
                                    ? CommonMethods.CommonWrite(
                                        "M4132",
                                        PusherRequestPut.ToString()
                                    )
                                    : false;
                            a =
                                (bool)CommonMethods.Device["M4133"] != BufferPlaceSenser
                                    ? CommonMethods.CommonWrite(
                                        "M4133",
                                        BufferPlaceSenser.ToString()
                                    )
                                    : false;
                            a =
                                (bool)CommonMethods.Device["M4134"] != PusherPlaceSenser
                                    ? CommonMethods.CommonWrite(
                                        "M4134",
                                        PusherPlaceSenser.ToString()
                                    )
                                    : false;

                            _processControl.WTRPutRequest = (bool)CommonMethods.Device["M4110"];
                            _processControl.WillPut = (bool)CommonMethods.Device["M4111"];
                            _processControl.WTRPutComplete = (bool)CommonMethods.Device["M4112"];
                            _processControl.WTRGetComplete = (bool)CommonMethods.Device["M4113"];
                            _processControl.PusherActionIsenable = (bool)CommonMethods.Device["M4114"];
                        }
                    }
                    catch (Exception EE)
                    {
                        logOpration.WriteError(EE);
                        Thread.Sleep(50);
                    }

                    Thread.Sleep(500);
                }
            });

        }
        private void LoadMainViewInfo()
        {

            ProcessTankCollections.AddRange(_benchStationEntity.Stations.Where(filter => filter.StationType == StationEnum.ProcessTank));
            BufferTankCollections.AddRange(_benchStationEntity.Stations.Where(filter => filter.StationType == StationEnum.BufferTank));
            MachineCollections.AddRange(_benchStationEntity.Stations.Where(filter => filter.StationType == StationEnum.Mechanical));
        }
        /// <summary>
        /// StarogageMessage=>PLC
        /// </summary>
        /// <param name="ctsSource"></param>
        public void CommunicateMessageWithPLC(CancellationTokenSource ctsSource)
        {
            CancellationTokenSource cancellationTokenSource = ctsSource;
            _containerProvider.Resolve<EAPService>().CJStartAction = new Action<string, string, string, string>(ActionCJ);
            Task.Run(() =>
            {
                try
                {
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        _benchStationManager.UpdateStationEntity();
                        Thread.Sleep(900);
                    }
                }
                catch (Exception ex)
                {

                    logOpration.WriteError(ex);

                }


            }, cancellationTokenSource.Token);
            Task.Run(async () =>
            {
                try
                {
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {

                        if (!CommonMethods.Device.IsConnected)
                        {
                            await Task.Delay(500);
                            continue;
                        }
                        Task[] tasks = new Task[2];
                        tasks[0] = WriteStorageLogToPLC();
                        tasks[1] = _containerProvider.Resolve<EAPService>().RunUpdateEapStatus(cancellationTokenSource);
                        await Task.WhenAll(tasks);
                        await Task.Delay(200);
                    }
                }
                catch (Exception ex)
                {
                    logOpration.WriteError(ex);
                }

            }, cancellationTokenSource.Token);
        }
        private void ActionCJ(string CJID, string RecipeName, string RFID1, string RFID2 = null)
        {
            try
            {
                string cjID = CJID;
                string recipeName = RecipeName;
                string rfid1 = RFID1;
                string rfid2 = RFID2;
                if (!_processControl.eFAM_Data.IsConnected)
                {
                    return;
                }
                _processControl.CarrierProcessQueue.BatchidCollection.Add(
                    new CarrierProcessQueueModel()
                    {
                        Batchid = CJID,
                        BatchState = BatchState.Process,
                        Priority = 0,
                        RecipeName = RecipeName
                    });
                var temp1 = _processControl.eFAM_Data.Storage_Data.FirstOrDefault(filter => filter.StationInfo.RFID.Replace("\0", "") == rfid1 && filter.PlaceSenser)!;

                if (!string.IsNullOrEmpty(RFID2))
                {
                    var temp2 = _processControl.eFAM_Data.Storage_Data.Where(filter => filter.StationInfo.RFID.Replace("\0", "") == rfid2 && filter.PlaceSenser).FirstOrDefault()!;
                    temp1.StationInfo.DoubleProcess = true;
                    temp2.StationInfo.DoubleProcess = true;
                    temp1.StationInfo.OddEven = OddEven.Odd;
                    temp2.StationInfo.OddEven = OddEven.Even;
                    temp2.StationInfo.RecipeName = RecipeName;
                    temp2.StationInfo.Batchid.Batchid = CJID;
                    temp2.StationInfo.Batchid.BatchState = BatchState.Process;
                    temp2.StationInfo.Batchid.Priority = 0;
                    temp2.StationInfo.Batchid.RecipeName = RecipeName;
                    temp2.StationInfo.ProcessState = ProcessState.WaitProcess;
                }
                else
                {
                    temp1.StationInfo.DoubleProcess = false;
                    temp1.StationInfo.OddEven = OddEven.Odd;

                }
                temp1.StationInfo.RecipeName = RecipeName;
                temp1.StationInfo.Batchid.Batchid = CJID;
                temp1.StationInfo.Batchid.BatchState = BatchState.Process;
                temp1.StationInfo.Batchid.Priority = 0;
                temp1.StationInfo.Batchid.RecipeName = RecipeName;
                temp1.StationInfo.ProcessState = ProcessState.WaitProcess;

                //StorageStation temp1 = _processControl.eFAM_Data.Storage_Data.FirstOrDefault(filter => filter.StationInfo.RFID == RFID1);
            }
            catch (Exception ee)
            {
                logOpration.WriteError(ee);
            }
        }
        /// <summary>
        /// 写入Storage 当前信息到PLC中
        /// </summary>
        /// <returns></returns>
        private async Task WriteStorageLogToPLC()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (cts.IsCancellationRequested)
                    {
                        return;
                    }
                    short[] StorageMessageInt = new short[3000];
                    foreach (var item in _processControl.eFAM_Data.Storage_Data)
                    {
                        short t1 = 0;
                        if ((short)(item.StationID) > 22)
                        {
                            t1 = (short)(item.StationID - 18);
                        }
                        else
                        {
                            t1 = (short)(item.StationID - 6);
                        }
                        short[] stateTemp = new short[10];
                        stateTemp[0] = (short)(t1 + 1); //NO
                        stateTemp[1] = ((short)(item.StationInfo.Form_LP - 19));
                        stateTemp[2] = (short)item.StationInfo.ProcessState;
                        //stateTemp[3] = (short)item.StationInfo.WaferCount;
                        stateTemp[4] = (short)(item.PlaceSenser ? 1 : 0);
                        Array.Copy(stateTemp, 0, StorageMessageInt, t1 * 100, 10);
                        if (!string.IsNullOrEmpty(item.StationInfo.RFID))
                        {
                            short[] RFIDTemp = GlobalMethodHelper.Instance.stringTointArray(item.StationInfo.RFID, 10);
                            Array.Copy(RFIDTemp, 0, StorageMessageInt, t1 * 100 + 10, 10);
                        }
                        short[] MappingResult = new short[25];
                        for (int i = 0; i < 25; i++)
                        {
                            switch (item.StationInfo.WaferMap[i])
                            {
                                case WaferMapStation.Empty:
                                    MappingResult[i] = 1;
                                    break;
                                case WaferMapStation.Present:
                                    MappingResult[i] = 3;
                                    break;
                                case WaferMapStation.Crossed:
                                    break;
                                case WaferMapStation.Unknown:
                                    break;
                                case WaferMapStation.Double:
                                    break;
                                case WaferMapStation.Not_found_on_Pick_Up:
                                    break;
                                default:
                                    break;
                            }

                        }
                        ;

                        Array.Copy(MappingResult, 0, StorageMessageInt, t1 * 100 + 20, 25);
                    }
                    _plcHelper.SelectPLC(PlcEnum.PLC1).Write("ZR270000", StorageMessageInt);

                    int lpstateAddress = 90000;
                    foreach (var item in _processControl.eFAM_Data.Loadport_Data)
                    {
                        if (!item.PlaceSenser & item.StationInfo.ProcessState == ProcessState.UnProcess)
                        {
                            _plcHelper.SelectPLC(PlcEnum.PLC1).Write($"ZR{lpstateAddress}", (short)0);
                            //
                        }
                        else if (item.PlaceSenser & item.StationInfo.ProcessState == ProcessState.UnProcess)
                        {
                            _plcHelper.SelectPLC(PlcEnum.PLC1).Write($"ZR{lpstateAddress}", (short)1);
                        }
                        else if (item.PlaceSenser & item.StationInfo.ProcessState == ProcessState.Processed)
                        {
                            _plcHelper.SelectPLC(PlcEnum.PLC1).Write($"ZR{lpstateAddress}", (short)2);
                        }
                        else
                        {
                            _plcHelper.SelectPLC(PlcEnum.PLC1).Write($"ZR{lpstateAddress}", (short)3);
                        }
                        ;
                        lpstateAddress++;
                    }

                }
                catch (Exception ee)
                {
                    logOpration.WriteError($"WriteStorageLogToPLC Error {ee.Message}");
                }
            }, cts.Token);
        }
        private void WritePLC(string obj)
        {
            if (obj == null)
                return;

            // 所有操作前，先确认连接状态
            if (!CommonMethods.Device.IsConnected &&
                !PLC2CommonMethods.Device.IsConnected &&
                !PLC3CommonMethods.Device.IsConnected &&
                !PLC4CommonMethods.Device.IsConnected &&
                !PLC5CommonMethods.Device.IsConnected
                //&&
                //!PLC6CommonMethods.Device.IsConnected
                )
            {
                MessageBox.Show("所有PLC都未连接！");
                return;
            }

            if (MessageBox.Show("确认切换当前状态！", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }

            if (obj == "Manual")
            {
                // PLC6: M102 = true, M2102 = true
                //PLC6CommonMethods.CommonWrite("M102", "true");
                //PLC6CommonMethods.CommonWrite("M2102", "true");

                // PLC5: M102 = true, M2102 = true
                PLC5CommonMethods.CommonWrite("M102", "true");
                PLC5CommonMethods.CommonWrite("M2102", "true");

                // PLC4: M102 = true, M2102 = true
                PLC4CommonMethods.CommonWrite("M102", "true");
                //PLC4CommonMethods.CommonWrite("M2102", "true");

                // PLC3: M102 = true
                PLC3CommonMethods.CommonWrite("M102", "true");

                // PLC2: M102 = true123123
                PLC2CommonMethods.CommonWrite("M102", "true");

                // PLC1: M1000 = false
                CommonMethods.CommonWrite("M1000", "false");
            }
            else
            {
                // 原有单地址切换逻辑
                var device = CommonMethods.Device;
                if (device.IsConnected)
                {
                    bool currentValue = (bool)device[obj];
                    CommonMethods.CommonWrite(obj, (!currentValue).ToString().ToLower());
                }
            }
        }
        #region EFAM事件
        private void EFAMConnect(string obj)
        {
            if (_processControl.eFAM_Data.IsConnected)
            {
                if (
                    MessageBox.Show("确认断开与EFAM的网络连接！", "提示", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes
                )
                {
                    _processControl.eFAM_Data.Disconnect();
                    _processControl.Auto = false;
                }
            }
            else
            {
                Task.Run(() =>
                {
                    if (_processControl.eFAM_Data.Connent())
                    {
                        _processControl.CarrierProcessQueue = new CarrierProcessQueue();
                        _processControl.eFAM_Data.Storage_Data
                        .Where(item => item.StationInfo.ProcessState == ProcessState.WaitProcess)
                        .Select(item => item.StationInfo.ProcessState = ProcessState.UnProcess);

                        _processControl.Auto = false;
                        _processControl.GetStationStatus();
                    }
                });
            }
        }
        private void _processControl_WTSDownRecipe(string obj, PusherStationState obj1)
        {
            string temppath = System.IO.Path.Combine(filepath, obj);
            bool b=DownLoadRecipe("Tool", File.ReadAllText(temppath));
            PusherStationState tempObj = obj1;
            int storagetemp = 0;
            string batchIDTemp = "";
            int id = 0;
            try
            {
                Task.Run(async () =>
                {
                    storagetemp = (int)tempObj.Odd_Data.StorageID;
                    batchIDTemp = _processControl.eFAM_Data.Storage_Data[storagetemp].StationInfo.Batchid.Batchid;

                    var data = new EndofRunData
                    {
                        StartTime = DateTime.Now,
                        BatchID = batchIDTemp,
                        MessageID = (int)tempObj.Odd_Data.StorageID,
                        FlowRecipeName = tempObj.RecipeName,
                        StationID = (int)tempObj.StationID,
                        Status = "Run",
                        ReturnWaferCount = tempObj.WaferCount.ToString(),
                    };
                    // ⭐ 关键点：返回主键 ID
                    id = await _logAddService.AddEndofRunLog(data);

                });
            }
            catch (Exception ee)
            {


            }
            finally
            {

                CommonStaticMethods.WritePusherToPLC(obj1);
                CommonMethods.PLC.Write("D5000", batchIDTemp, 20);
                CommonMethods.PLC.Write("D5175", (short)id);
                CommonMethods.PLC.Write("M4135", true);
                CommonMethods.CommonWrite("M4113", "false");
            }

        }
        private void _processControl_WTRPutCompleteEvent(PusherStationState obj)
        {
            CommonStaticMethods.ReadPLCToPusher(obj);
            PusherStationState tempObj = obj;
            Task.Run(async () =>
            {
                try
                {
                    await _logAddService.UpdateEndofRunLog((int)tempObj.Odd_Data.StorageID);
                }
                catch (Exception ee)
                {

                }
                finally
                {
                    CommonMethods.CommonWrite("M4112", "false");
                }
            });

        }
        private void _processControl_WTRGetCompleteEvent(PusherStationState obj)
        {
            PusherStationState tempObj = obj;
            int storagetemp = 0;
            int id = 0;
            string batchIDTemp = "";
            try
            {
                Task.Run(async () =>
                {
                    storagetemp = (int)tempObj.Odd_Data.StorageID;
                    batchIDTemp = _processControl.eFAM_Data.Storage_Data[storagetemp].StationInfo.Batchid.Batchid;

                    //var data = new EndofRunData
                    //{
                    //    StartTime = DateTime.Now,
                    //    BatchID = batchIDTemp,
                    //    MessageID = (int)tempObj.Odd_Data.StorageID,
                    //    FlowRecipeName = tempObj.RecipeName,
                    //    StationID = (int)tempObj.StationID,
                    //    Status = "Run",
                    //    ReturnWaferCount = tempObj.WaferCount.ToString(),
                    //};


                    // ⭐ 关键点：返回主键 ID
                    // id = await processLogManage.AddReturnId(data);
                    // CommonMethods.PLC.Write("D5175", (short)id);
                });
            }
            catch (Exception ee)
            {


            }
            finally
            {

                CommonMethods.CommonWrite("M4113", "false");
            }

        }
        #endregion
        public void MainConfigure()
        {
            _viewTransitionNavigator.MainVeiwNavigation("OverView");
        }
        private void OpenParameter(object Module)
        {
            NavigationParameters keys = new NavigationParameters();
            keys.Add("ModuleName", Module);
            _viewTransitionNavigator.MainVeiwNavigation("ParameterView", keys);
        }
        private void OpenRecipe(object Module)
        {
            var tempModule = Module as StationCollection;
            NavigationParameters keys = new NavigationParameters();
            keys.Add("Module", tempModule);
            _viewTransitionNavigator.MainVeiwNavigation(nameof(ModuleRecipeMainView), keys);
        }
        private void OpenStatusView(object Module)
        {

            var tempModule = Module as StationCollection;
            NavigationParameters keys = new NavigationParameters();
            keys.Add("Module", tempModule);
            _viewTransitionNavigator.MainVeiwNavigation("StatusMainView", keys);
        }
        private void OpenDialogView(string module)
        {
            _dialogService.ShowDialog(module);
        }
        private void WriteToolRecipe()
        {
            try
            {
                DialogParameters key = new DialogParameters();
                IDialogResult r = new DialogResult();
                string path = System.IO.Path.Combine(@"C:\212Recipe", "Tool");
                key.Add("FilePath", path);
                _dialogService.ShowDialog(nameof(OpenFileView), key, result => r = result);
                if (r.Result == ButtonResult.OK)
                {
                    string path1 = r.Parameters.GetValue<string>("Result1");
                    if (DownLoadRecipe("Tool", path1))
                    {
                        MessageBox.Show("下载完成");
                    }
                    else
                    {
                        MessageBox.Show("下载失败");
                    }
                }
            }
            catch (Exception ee)
            {


            }
        }
        private void OpenFolderDialogView(object module)
        {
            var moduletemp = module as StationCollection;
            if (moduletemp == null) return;
            DialogParameters key = new DialogParameters();
            IDialogResult r = new DialogResult();
            string path = System.IO.Path.Combine(@"C:\212Recipe", moduletemp.StationName);
            key.Add("FilePath", path);
            _dialogService.ShowDialog(nameof(OpenFileView), key, result => r = result);
            if (r.Result == ButtonResult.OK)
            {
                string path1 = r.Parameters.GetValue<string>("Result1");
                if (DownLoadRecipe(moduletemp.StationName, path1))
                {
                    MessageBox.Show("下载完成");
                }
                else
                {
                    MessageBox.Show("下载失败");
                }
            }
        }
        private bool DownLoadRecipe(string module, string PATH)
        {
            try
            {
                IRecipeDownLoad db;
                switch (module)
                {
                    case "Ag_1":
                        db = _containerProvider.Resolve<ETCHRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D5000", PlcEnum.PLC2);
                    case "Ag_2":
                        db = _containerProvider.Resolve<ETCHRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D5000", PlcEnum.PLC3);
                    case "Ni_1":
                        db = _containerProvider.Resolve<ETCHRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D5000", PlcEnum.PLC4);
                    case "Ni_2":
                        db = _containerProvider.Resolve<ETCHRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D5000", PlcEnum.PLC5);
                    case "Ti_1":
                        db = _containerProvider.Resolve<ETCHRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D5000", PlcEnum.PLC6);
                    case "QDR_1":
                        db = _containerProvider.Resolve<QDRRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D6000", PlcEnum.PLC2);
                    case "QDR_2":
                        db = _containerProvider.Resolve<QDRRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D6000", PlcEnum.PLC3);
                    case "QDR_3":
                        db = _containerProvider.Resolve<QDRRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D6000", PlcEnum.PLC4);
                    case "QDR_4":
                        db = _containerProvider.Resolve<QDRRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D6000", PlcEnum.PLC5);
                    case "QDR_5":
                        db = _containerProvider.Resolve<QDRRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D6000", PlcEnum.PLC6);
                    case "LPD_1":
                        db = _containerProvider.Resolve<LPDRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("D5000", PlcEnum.PLC7);

                    case "Tool":
                        db = _containerProvider.Resolve<FlowRecipeControl>();
                        db.path = PATH;
                        return db.DownLoad("ZR1000", PlcEnum.PLC1);
                    default:
                        return false;
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        private void OpenViewByName(string obj)
        {
            _viewTransitionNavigator.MainVeiwNavigation(obj);
        }
        public void OpenIOView(string plcEnum)
        {
            IDialogResult r = null;
            DialogParameters keys = new DialogParameters();

            keys.Add("PLCEnum", plcEnum);
            // 新增：添加标题参数（PLC类型到标题的映射）
            keys.Add(
                "DisplayText",
                plcEnum == "PLC1"
                    ? "WTR"
                    : plcEnum == "PLC2"
                        ? "MGD"
                        : plcEnum == "PLC3"
                            ? "QDR/CC"
                            : plcEnum == "PLC4"
                                ? "IPA_1/IPA_2"
                                : plcEnum == "PLC5"
                                    ? "NMP_1/NMP_2"
                                    : "SYS9070_1/SYS9070_2"
            );
            _dialogService.ShowDialog(nameof(IOView), keys, result => r = result);
        }

        #region 事件
        private void UpdateUIStatus()
        {
            Thread.Sleep(100);
            while (true)
            {
                try
                {
                    if (CommonMethods.Device.IsConnected)
                    {
                        //Btn_1 = (bool)CommonMethods.Device["Y11"];
                        //Btn_2 = (bool)CommonMethods.Device["M1000"];
                        //Btn_3 = (bool)CommonMethods.Device["Y12"];
                        //Btn_4 = (bool)CommonMethods.Device["Y13"];
                        //Btn_5 = (bool)CommonMethods.Device["M3151"];
                        ////Btn_6 = (bool)CommonMethods.Device["M3600"];
                        //Btn_7 = (bool)CommonMethods.Device["Y10"];
                        //Btn_8 = (bool)CommonMethods.Device["Y10"];
                        //Btn_9 = (bool)CommonMethods.Device["Y10"];

                    }

                    var zr1000Value = Convert.ToInt32(CommonMethods.Device["ZR44000"]);

                    switch (zr1000Value)
                    {
                        case 1:
                            ControlState = "State1";
                            break;
                        case 2:
                            ControlState = "State2";
                            break;
                        case 3:
                            ControlState = "State3";
                            break;
                        default:
                            ControlState = $"Unknow";
                            break;
                    }
                    string temp = "";

                    for (int i = 1; i < 13; i++)
                    {
                        temp = string.Concat(temp, $"PLC{i} State {_plcHelper.ConnectState((PlcEnum)(i - 1))}  ");
                    }
                    MachineState = string.Concat(temp, $"EFAM State :{_processControl.eFAM_Data.IsConnected}");

                    Thread.Sleep(300);
                }
                catch (Exception EE) { }
            }
        }
        private async void BeatHeart(CancellationTokenSource cts, string Address, PlcEnum plcEnum)
        {
            var _cts = cts;
            try
            {
                await Task.Run(async () =>
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        await Task.Delay(1000);
                        Device tempDevice = _plcHelper.SelectDevice();
                        if (!tempDevice.IsConnected) continue;
                        if (!_processControl.eFAM_Data.IsConnected) continue;
                        _plcHelper.CommonWrite(Address, true.ToString(), plcEnum);
                        await Task.Delay(1000);
                        _plcHelper.CommonWrite(Address, false.ToString(), plcEnum);

                    }
                });

            }
            catch (Exception EE)
            {
                logOpration.WriteError("EFAM BeatHeart");
            }
        }

        #endregion


    }
}
