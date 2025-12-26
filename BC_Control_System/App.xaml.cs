using BC_Control_BLL.Abstract;
using BC_Control_BLL.PLC;
using BC_Control_BLL.recipedownload;
using BC_Control_BLL.services.TraceLogService;
using BC_Control_BLL.Services;
using BC_Control_DAL;
using BC_Control_Helper;
using BC_Control_Helper.FILE;
using BC_Control_Helper.interfaceHelper;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using BC_Control_System.Abstract;
using BC_Control_System.Command;
using BC_Control_System.Service;
using BC_Control_System.Theme;
using BC_Control_System.view.Log.TraceLogViews;
using BC_Control_System.view.Status.IOView;
using BC_Control_System.View;
using BC_Control_System.View.Alarm;
using BC_Control_System.View.Help;
using BC_Control_System.View.Log;
using BC_Control_System.View.Log.TraceLogViews;
using BC_Control_System.View.Maintenance;
using BC_Control_System.View.Opration;
using BC_Control_System.View.Parameter;
using BC_Control_System.View.Recipe;
using BC_Control_System.View.Recipe.ModelRecipe;
using BC_Control_System.View.Status;
using BC_Control_System.View.Status.IOView;
using BC_Control_System.View.Status.IOView.Machine;
using BC_Control_System.View.Status.IOView.MixTank;
using BC_Control_System.View.Status.IOView.Tank;
using BC_Control_System.ViewModel;
using BC_Control_System.ViewModel.Alarm;
using BC_Control_System.ViewModel.Help;
using BC_Control_System.ViewModel.Log;
using BC_Control_System.ViewModel.Log.TankTraceLog;
using BC_Control_System.ViewModel.Maintenance;
using BC_Control_System.ViewModel.Opration;
using BC_Control_System.ViewModel.Parameter;
using BC_Control_System.ViewModel.Recipe;
using BC_Control_System.ViewModel.Recipe.ModuleRecipe;
using BC_Control_System.ViewModel.Status;
using BC_Control_System.ViewModel.Status.IOViewModel;
using BC_Control_System.ViewModel.Status.IOViewModel.Machine;
using BC_Control_System.ViewModel.Status.IOViewModel.MixTank;
using BC_Control_System.ViewModel.Status.IOViewModel.Tank;
using BC_Control_System.ViewModels;
using BC_Control_System.ViewModels.LogDataModel;
using BC_Control_System.Views;
using BC_Control_System.Views.IO;
using BC_Control_System.Views.LogData;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System.IO;
using System.Windows;
using ZC_Control_System.EFAMAction;
using ZC_Control_System.ViewModel.Opration;

namespace BC_Control_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private static Mutex _mutex = null;
        private EnhancedExceptionHandler _exceptionHandler;

        public IServiceProvider Services { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "YourUniqueAppName";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                // 如果互斥体已存在，说明已有程序实例在运行
                MessageBox.Show("程序已经在运行！");
                System.Windows.Application.Current.Shutdown();
            }
            string logDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
               "CrashLogs");

            _exceptionHandler = new EnhancedExceptionHandler(
                "YourUniqueAppName",
                null,
                (ex, reason) =>
                {
                    // 自定义错误处理，例如发送错误报告
                    // 可以根据不同的退出原因采取不同的操作
                    switch (reason)
                    {
                        case ExitReason.UnhandledException:
                            // 发送错误报告
                            break;
                        case ExitReason.ResourceExhaustion:
                            // 提醒用户清理资源
                            break;
                        case ExitReason.ExternalTermination:
                            // 记录异常终止
                            break;
                    }
                });
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            if (EnhancedExceptionHandler.Instance?.GetExitReason() == ExitReason.Normal)
            {
                EnhancedExceptionHandler.Instance?.SetUserRequestedExit();
            }
            EnhancedExceptionHandler.Instance?.SetUserRequestedExit();
            _mutex?.Close();
            _mutex = null;
            _exceptionHandler?.Dispose();

            base.OnExit(e);
        }

        public static Rules DefaultRules =>
            Rules
                .Default.WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                .WithFuncAndLazyWithoutRegistration()
        .WithTrackingDisposableTransients()
        //.WithoutFastExpressionCompiler()
                .WithFactorySelector(Rules.SelectLastRegisteredFactory());

        protected override Rules CreateContainerRules()
        {
            return DefaultRules;
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region new
            containerRegistry.RegisterSingleton<BenchStationService>();
            containerRegistry.RegisterSingleton<IBenchStationEntity, BenchStationEntity>();
            containerRegistry.RegisterSingleton<BatchDataService>();
            #endregion
            containerRegistry.RegisterSingleton<ILogCommandService, LogCommandService>();
            containerRegistry.RegisterSingleton<ThemeManager>();
            containerRegistry.RegisterSingleton<DataBaseAddService>();
            containerRegistry.RegisterSingleton<SysAdminService>();
            containerRegistry.RegisterSingleton<SysAdmin>();
            containerRegistry.RegisterSingleton<EndofRunLogService>(); //瞬态
            containerRegistry.RegisterSingleton<AlarmLogService>(); //瞬态
            containerRegistry.RegisterSingleton<EventLogService>();
            containerRegistry.RegisterSingleton<OperatorLogService>();
            containerRegistry.RegisterSingleton<EFAMMessage>();
            containerRegistry.RegisterSingleton<ETCHRecipeControl>();
            containerRegistry.RegisterSingleton<AuthService>();
            containerRegistry.RegisterSingleton<FlowRecipeControl>();          
            containerRegistry.RegisterSingleton<TK1LogDataService>();
            containerRegistry.RegisterSingleton<GlobalVariable>();
            containerRegistry.RegisterSingleton<EAPService>();
            containerRegistry.RegisterSingleton<ViewTransitionNavigator>();
            containerRegistry.Register<IPLCHelper, PLCSelect>();
            containerRegistry.Register<IPLCControl, PLCControl>();
            containerRegistry.RegisterSingleton<TK2LogDataService>();
            containerRegistry.RegisterSingleton<TK3LogDataService>();
            containerRegistry.RegisterSingleton<TK4LogDataService>();
            containerRegistry.RegisterSingleton<TK5LogDataService>();
            containerRegistry.RegisterSingleton<TK6LogDataService>();
            containerRegistry.RegisterSingleton<IExcelOperation, MiniExcelOperation>();
            //containerRegistry.RegisterSingleton<IExcelOperation, NPIOExcelOpration>();
            containerRegistry.RegisterSingleton<ILogOpration, ControllerLog>();
            containerRegistry.RegisterSingleton<IRecipeFileCommand, RecipeFileCommandSerice>();
            containerRegistry.RegisterSingleton<ZC_Control_EFAM.ProcessControl.ProcessControl>();
            containerRegistry.Register<SysAdminService>();
            containerRegistry.Register<ISqlSugarHelper<SysAdmin>, SqlSugarHelper<SysAdmin>>();
            containerRegistry.Register<TKClass>();
            // 注册服务
            containerRegistry.RegisterSingleton<SysAdminService>();
            //containerRegistry.RegisterForNavigation<LFR1IOView>("LFR_1");
            //containerRegistry.RegisterForNavigation<LFR_2IOView>("LFR_2");
            containerRegistry.RegisterForNavigation<LoginView>("LoginView");
            // 新增对话框注册
            

            containerRegistry.RegisterSingleton<OpenRecipeEditorViewCommand>();
            containerRegistry.RegisterDialog<CarrierOut, CarrierOutViewModel>();
            containerRegistry.RegisterDialog<ChartView, ChartViewModel>();
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();
            containerRegistry.RegisterDialog<SearchByTimeView, SearchByTimeViewModel>();
            containerRegistry.RegisterDialog<SortView, SortViewModel>();
            containerRegistry.RegisterDialog<SaveFileView, SaveFileViewModel>();
            containerRegistry.RegisterForNavigation<UseRegView, UseRegViewModel>(); // 页面导航
            containerRegistry.RegisterDialog<UseRegView, UseRegViewModel>();
            containerRegistry.RegisterForNavigation<LDAccessStateView, LDAccessStateViewModel>();
            containerRegistry.RegisterForNavigation<OpenFileView, OpenFileViewModel>();
            containerRegistry.RegisterForNavigation<IOView, IOViewModel>();
            containerRegistry.RegisterForNavigation<ThemeView, ThemeViewModel>();
            containerRegistry.RegisterForNavigation<
                LoadportTrackingView,
                LoadPortTrackingViewModel
            >();
            containerRegistry.RegisterForNavigation<
                LoadportTrackingView,
                LoadPortTrackingViewModel
            >();
            containerRegistry.RegisterForNavigation<CarrierTrackView, CarrierTrackViewModel>();
            containerRegistry.RegisterForNavigation<BatchTrackingView, BatchTrackingViewModel>();
            containerRegistry.RegisterForNavigation<ProcessStartView, ProcessStartViewModel>();
            containerRegistry.RegisterForNavigation<
                InsertTimeLogMainView,
                InsertTimeMainLogViewModel
            >();
            containerRegistry.RegisterForNavigation<
                Tank1TraceLogView,
                Tank1TraceLogViewModel
            >();


            
           
            containerRegistry.RegisterForNavigation<ActAlarmView, ActAlarmViewModel>();

            containerRegistry.RegisterForNavigation<MainWindow, MainWindowModel>();
            containerRegistry.RegisterForNavigation<OverView, OverViewModel>();
            containerRegistry.RegisterForNavigation<ParameterView, ParameterCollectionViewModel>();
            containerRegistry.RegisterForNavigation<StatusMainView, StatusOverViewModel>();
            containerRegistry.RegisterForNavigation<
                HorizontalStateView,
                HorizontalStateViewModel
            >();
            containerRegistry.RegisterForNavigation<PLCValueView, PLCValueViewModel>();

            containerRegistry.RegisterForNavigation<EndofRunLogView, EndofRunLogViewModel>();
            containerRegistry.RegisterForNavigation<ProcessLogDetailedInformationView, ProcessLogDetailedInformationViewModel>();
            containerRegistry.RegisterForNavigation<AlarmLogData, AlarmLogViewModel>();
            containerRegistry.RegisterForNavigation<EventLogView, EventLogViewModel>();
            containerRegistry.RegisterForNavigation<GraphView, GraphViewModel>();
            containerRegistry.RegisterForNavigation<OprationLogView, OprationLogViewModel>();
            containerRegistry.RegisterForNavigation<WaferRecordView, WaferRecordViewModel>();

            containerRegistry.RegisterForNavigation<ControlPanelView,ControlPanelViewModel>();
            containerRegistry.RegisterForNavigation<StationValueView, StationValueViewModel>();
            containerRegistry.RegisterForNavigation<LFR_1IOView, LFR_1IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_2IOView, LFR_2IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_3IOView, LFR_3IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_4IOView, LFR_4IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_5IOView, LFR_5IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_6IOView, LFR_6IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_7IOView, LFR_7IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_8IOView, LFR_8IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_9IOView, LFR_9IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_10IOView, LFR_10IOViewModel>();
            containerRegistry.RegisterForNavigation<LFR_11IOView, LFR_11IOViewModel>();
            containerRegistry.RegisterForNavigation<WTR_1IOView, WTRIOViewModel>();



            containerRegistry.RegisterForNavigation<Ag_1IOView,Ag_1IOViewModel>();
            containerRegistry.RegisterForNavigation<Ag_2IOView, Ag_2ViewModel>();
            containerRegistry.RegisterForNavigation<Ni_1IOView, Ni_1ViewModel>();
            containerRegistry.RegisterForNavigation<Ni_2IOView, Ni_2ViewModel>();
            containerRegistry.RegisterForNavigation<Ti_1IOView, Ti_1IOViewModel>();
            containerRegistry.RegisterForNavigation<QDR_1IOView, QDR_1IOViewModel>();
            containerRegistry.RegisterForNavigation<QDR_2IOView, QDR_2IOViewModel>();
            containerRegistry.RegisterForNavigation<QDR_3IOView, QDR_3IOViewModel>();
            containerRegistry.RegisterForNavigation<QDR_4IOView, QDR_4IOViewModel>();
            containerRegistry.RegisterForNavigation<QDR_5IOView, QDR_5IOViewModel>();
            containerRegistry.RegisterForNavigation<CC_1IOView, CC_1IOViewModel>();
            containerRegistry.RegisterForNavigation<LPD_1IOView, LPD_1IOViewModel>();

            containerRegistry.RegisterForNavigation<MixTank_1IOView, MixTank_1IOViewModel>();
            containerRegistry.RegisterForNavigation<MixTank_2IOView, MixTank_2IOViewModel>();
            containerRegistry.RegisterForNavigation<MixTank_3IOView, MixTank_3IOViewModel>();
            containerRegistry.RegisterForNavigation<MixTank_4IOView, MixTank_4IOViewModel>();
            containerRegistry.RegisterForNavigation<MixTank_5IOView, MixTank_5IOViewModel>();


            containerRegistry.RegisterForNavigation<ModuleRecipeMainView, ModuleRecipeMainViewModel>();
            containerRegistry.RegisterForNavigation<ETCHRecipeView, ETCHTank_RecipeViewModel>();
            containerRegistry.RegisterForNavigation<QDRRecipeView, QDR_RecipeViewModel>();
            containerRegistry.RegisterForNavigation<LPD_RecipeView, LPD_RecipeViewModel>();
            containerRegistry.RegisterForNavigation<FlowRecipeStepView, FlowRecipeStepViewModel>();
            containerRegistry.RegisterForNavigation<FlowRecipe, FlowRecipeViewModel>();

            #region Log
            containerRegistry.RegisterForNavigation<Tank1TraceLogView, Tank1TraceLogViewModel>("Tank_1TraceLog");
            containerRegistry.RegisterForNavigation<Tank3SC1TraceLogView, Tank3TraceLogViewModel>("Tank_3TraceLog");
            containerRegistry.RegisterForNavigation<Tank5SC1TraceLogView, Tank5TraceLogViewModel>("Tank_5TraceLog");
            containerRegistry.RegisterForNavigation<Tank7SC1TraceLogView, Tank7TraceLogViewModel>("Tank_7TraceLog");
            containerRegistry.RegisterForNavigation<Tank9SC1TraceLogView, Tank9TraceLogViewModel>("Tank_9TraceLog");

            containerRegistry.RegisterForNavigation<Tank2QDRTraceLogView, Tank2TraceLogViewModel>("Tank_2TraceLog");
            containerRegistry.RegisterForNavigation<Tank4QDRTraceLogView, Tank4TraceLogViewModel>("Tank_4TraceLog");
            containerRegistry.RegisterForNavigation<Tank6QDRTraceLogView, Tank6TraceLogViewModel>("Tank_6TraceLog");
            containerRegistry.RegisterForNavigation<Tank8QDRTraceLogView, Tank8TraceLogViewModel>("Tank_8TraceLog");
            containerRegistry.RegisterForNavigation<Tank10QDRTraceLogView, Tank10TraceLogViewModel>("Tank_10TraceLog");
            containerRegistry.RegisterForNavigation<Tank11TraceLogView, Tank11TraceLogViewModel>("Tank_11TraceLog");
            #endregion
            // UI线程未处理异常
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            // 非UI线程未处理异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // Task异常
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // 处理UI线程异常
            HandleException(e.Exception);
            e.Handled = true; // 标记为已处理，防止应用程序崩溃
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // 处理非UI线程异常
            if (e.ExceptionObject is Exception ex)
            {
               
                HandleException(ex);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // 处理Task异常
            HandleException(e.Exception);            
            e.SetObserved(); // 标记为已观察
        }

        private void HandleException(Exception ex)
        {
            var logopration=Container.Resolve<ILogOpration>();
            logopration.WriteError(ex);
        }
        protected override void ConfigureRegionAdapterMappings(
            RegionAdapterMappings regionAdapterMappings
        )
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            // 可以添加自定义区域适配器
        }

        protected override void OnInitialized()
        {
           
            //通过自定义的配置服务接口，获取需要显示的内容和用户名
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
            {
                service.MainConfigure();
            }
            //CommonStaticMethods.OpenRecipeViewCommand=Container.Resolve<>
            base.OnInitialized();
        }

    }

}
