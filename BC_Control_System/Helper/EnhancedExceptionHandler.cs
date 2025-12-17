using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BC_Control_System
{
    /// <summary>
    /// 应用程序退出原因
    /// </summary>
    public enum ExitReason
    {
        Normal,             // 正常退出
        UnhandledException, // 未处理异常
        UserRequest,        // 用户请求退出
        SystemShutdown,     // 系统关闭
        ResourceExhaustion, // 资源耗尽
        ExternalTermination,// 外部终止
        Unknown             // 未知原因
    }

    /// <summary>
    /// WPF全局异常处理器，用于捕获和记录应用程序未处理的异常及退出原因
    /// </summary>
    public class EnhancedExceptionHandler : IDisposable
    {
        private readonly string _applicationName;
        private readonly string _logDirectory;
        private readonly Action<Exception, ExitReason> _customErrorAction;
        private bool _disposed = false;
        private ExitReason _exitReason = ExitReason.Normal;
        private Exception _lastException = null;
        private DateTime _startTime;
        private static EnhancedExceptionHandler _instance;

        // 退出原因追踪相关
        private bool _isSystemShuttingDown = false;
        private bool _isUserRequestedExit = false;

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static EnhancedExceptionHandler Instance => _instance;

        /// <summary>
        /// 初始化全局异常处理器
        /// </summary>
        public EnhancedExceptionHandler(string applicationName = null,
            string logDirectory = null,
            Action<Exception, ExitReason> customErrorAction = null)
        {
            _applicationName = applicationName ?? AppDomain.CurrentDomain.FriendlyName;
            _logDirectory = logDirectory ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash_logs");
            _customErrorAction = customErrorAction;
            _startTime = DateTime.Now;
            _instance = this;

            // 创建日志目录
            Directory.CreateDirectory(_logDirectory);

            // 订阅WPF全局异常事件
            Application.Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // 订阅应用程序退出事件
            Application.Current.Exit += Application_Exit;
            Application.Current.SessionEnding += Application_SessionEnding;

            // 订阅系统事件
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _exitReason = ExitReason.UnhandledException;
            _lastException = e.Exception;

            HandleException(e.Exception, "DispatcherUnhandledException");
            e.Handled = true; // 标记为已处理，防止应用程序立即退出

            // 延迟退出，让日志有时间写入
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Application.Current.Shutdown(1);
            }), DispatcherPriority.ApplicationIdle);
        }

        /// <summary>
        /// 处理非UI线程异常
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _exitReason = ExitReason.UnhandledException;
            _lastException = e.ExceptionObject as Exception;

            HandleException(e.ExceptionObject as Exception,
                e.IsTerminating ? "UnhandledException(Terminating)" : "UnhandledException");

            if (e.IsTerminating)
            {
                // 给日志写入一点时间
                Task.Delay(500).Wait();
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// 处理任务异常
        /// </summary>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _exitReason = ExitReason.UnhandledException;
            _lastException = e.Exception;

            HandleException(e.Exception, "UnobservedTaskException");
            e.SetObserved(); // 标记为已观察，防止进程终止
        }

        /// <summary>
        /// 应用程序退出事件
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            LogExitReason();
        }

        /// <summary>
        /// 会话结束事件
        /// </summary>
        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            _isSystemShuttingDown = true;
            _exitReason = ExitReason.SystemShutdown;
        }

        /// <summary>
        /// 系统事件 - 会话结束
        /// </summary>
        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            _isSystemShuttingDown = true;
            _exitReason = ExitReason.SystemShutdown;
        }

        /// <summary>
        /// 进程退出事件
        /// </summary>
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // 如果之前没有记录退出原因，现在记录
            if (_exitReason == ExitReason.Normal)
            {
                // 检查是否是外部终止
                if (IsExternalTermination())
                {
                    _exitReason = ExitReason.ExternalTermination;
                }
            }

            LogExitReason();
        }

        /// <summary>
        /// 异常处理核心方法
        /// </summary>
        private void HandleException(Exception ex, string source)
        {
            if (ex == null) return;

            // 调用自定义错误处理（如果有）
            //_customErrorAction?.Invoke(ex, _exitReason);

            // 记录异常
            LogCrash(ex, source);

            // 检查是否是资源耗尽异常
            if (IsResourceExhaustionException(ex))
            {
                _exitReason = ExitReason.ResourceExhaustion;
            }

            // 显示错误消息
            //ShowCrashMessage(ex);
        }

        /// <summary>
        /// 记录退出原因
        /// </summary>
        private void LogExitReason()
        {
            try
            {
                string logFile = Path.Combine(_logDirectory, $"exit_{DateTime.Now:yyyyMMdd_HHmmssfff}.log");

                using (StreamWriter writer = new StreamWriter(logFile, true, Encoding.UTF8))
                {
                    writer.WriteLine($"Application: {_applicationName}");
                    writer.WriteLine($"Exit Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                    writer.WriteLine($"Exit Reason: {_exitReason}");
                    writer.WriteLine($"Uptime: {DateTime.Now - _startTime}");

                    if (_lastException != null)
                    {
                        writer.WriteLine();
                        writer.WriteLine("=== Exception Information ===");
                        writer.WriteLine($"Exception Type: {_lastException.GetType().FullName}");
                        writer.WriteLine($"Exception Message: {_lastException.Message}");
                    }

                    // 添加系统信息
                    writer.WriteLine();
                    writer.WriteLine("=== System Information ===");
                    writer.WriteLine($"OS Version: {Environment.OSVersion}");
                    writer.WriteLine($"CLR Version: {Environment.Version}");
                    writer.WriteLine($"Memory Usage: {Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024}MB");
                    writer.WriteLine($"Exit Code: {Environment.ExitCode}");

                    // 添加应用程序统计信息
                    writer.WriteLine();
                    writer.WriteLine("=== Application Statistics ===");
                    writer.WriteLine($"Start Time: {_startTime:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Total Processor Time: {Process.GetCurrentProcess().TotalProcessorTime}");
                }
            }
            catch
            {
                // 日志记录失败
            }
        }

        /// <summary>
        /// 记录崩溃信息到文件
        /// </summary>
        private void LogCrash(Exception ex, string source)
        {
            try
            {
                string logFile = Path.Combine(_logDirectory, $"crash_{DateTime.Now:yyyyMMdd_HHmmssfff}.log");

                using (StreamWriter writer = new StreamWriter(logFile, true, Encoding.UTF8))
                {
                    writer.WriteLine($"Application: {_applicationName}");
                    writer.WriteLine($"Crash Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                    writer.WriteLine($"Exception Source: {source}");
                    writer.WriteLine($"Exception Type: {ex.GetType().FullName}");
                    writer.WriteLine($"Exception Message: {ex.Message}");
                    writer.WriteLine($"Stack Trace:");
                    writer.WriteLine(ex.StackTrace);

                    // 记录内部异常
                    Exception inner = ex.InnerException;
                    int depth = 0;
                    while (inner != null && depth < 10)
                    {
                        writer.WriteLine();
                        writer.WriteLine($"Inner Exception #{++depth}:");
                        writer.WriteLine($"Type: {inner.GetType().FullName}");
                        writer.WriteLine($"Message: {inner.Message}");
                        writer.WriteLine($"Stack Trace:");
                        writer.WriteLine(inner.StackTrace);
                        inner = inner.InnerException;
                    }

                    // 添加系统信息
                    writer.WriteLine();
                    writer.WriteLine("=== System Information ===");
                    writer.WriteLine($"OS Version: {Environment.OSVersion}");
                    writer.WriteLine($"CLR Version: {Environment.Version}");
                    writer.WriteLine($"Machine Name: {Environment.MachineName}");
                    writer.WriteLine($"Memory: {Environment.WorkingSet / 1024 / 1024}MB");
                    writer.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}");
                }
            }
            catch (Exception logEx)
            {
                // 如果日志记录失败，尝试其他方式
                TryAlternativeLogging(ex, source, logEx);
            }
        }

        /// <summary>
        /// 备用日志记录方法
        /// </summary>
        private void TryAlternativeLogging(Exception originalEx, string source, Exception logEx)
        {
            try
            {
                // 尝试写入事件日志
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    string message = $"Failed to write crash log: {logEx.Message}. Original exception: {originalEx.GetType().Name}: {originalEx.Message}";
                    eventLog.WriteEntry(message, EventLogEntryType.Error);
                }

                // 尝试写入临时文件
                string tempFile = Path.Combine(Path.GetTempPath(), $"{_applicationName}_crash.log");
                File.WriteAllText(tempFile, $"Crash at {DateTime.Now}: {originalEx}");
            }
            catch
            {
                // 所有日志方法都失败了，我们无能为力
            }
        }

        /// <summary>
        /// 显示崩溃消息
        /// </summary>
        private void ShowCrashMessage(Exception ex)
        {
            try
            {
                // 使用WPF的MessageBox
                MessageBox.Show(
                    $"应用程序发生错误:\n{ex.Message}\n\n详细信息已记录到日志文件中。",
                    "应用程序错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch
            {
                // 如果UI显示也失败，尝试控制台输出
                try
                {
                    Console.WriteLine($"应用程序错误: {ex.Message}");
                }
                catch
                {
                    // 最后的手段
                }
            }
        }

        /// <summary>
        /// 判断是否是资源耗尽异常
        /// </summary>
        private bool IsResourceExhaustionException(Exception ex)
        {
            if (ex is OutOfMemoryException) return true;
            if (ex is IOException ioEx && ioEx.Message.Contains("磁盘空间")) return true;
            if (ex is System.ComponentModel.Win32Exception win32Ex)
            {
                // 处理一些Windows系统错误代码
                switch (win32Ex.NativeErrorCode)
                {
                    case 8:   // ERROR_NOT_ENOUGH_MEMORY
                    case 14:  // ERROR_OUTOFMEMORY
                    case 112: // ERROR_DISK_FULL
                    case 1450: // ERROR_NO_SYSTEM_RESOURCES
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否是外部终止
        /// </summary>
        private bool IsExternalTermination()
        {
            try
            {
                // 检查进程退出代码
                if (Environment.ExitCode != 0) return true;

                // 检查是否有未处理的异常
                if (_lastException != null) return false;

                // 检查是否是正常退出路径
                // 这里可以根据您的应用程序逻辑添加更多检查
                return !_isUserRequestedExit && !_isSystemShuttingDown;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置用户请求退出
        /// </summary>
        public void SetUserRequestedExit()
        {
            _isUserRequestedExit = true;
            _exitReason = ExitReason.UserRequest;
        }

        /// <summary>
        /// 获取当前退出原因
        /// </summary>
        public ExitReason GetExitReason()
        {
            return _exitReason;
        }

        /// <summary>
        /// 获取最后的异常
        /// </summary>
        public Exception GetLastException()
        {
            return _lastException;
        }

        /// <summary>
        /// 打开日志目录
        /// </summary>
        public void OpenLogDirectory()
        {
            try
            {
                if (Directory.Exists(_logDirectory))
                {
                    Process.Start("explorer.exe", _logDirectory);
                }
                else
                {
                    MessageBox.Show("没有找到日志目录。", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开日志目录: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 模拟崩溃（用于测试）
        /// </summary>
        public void SimulateCrash()
        {
            throw new InvalidOperationException("这是一个模拟的应用程序崩溃异常");
        }

        /// <summary>
        /// 启动子进程
        /// </summary>
        public Process StartChildProcess(string arguments = null)
        {
            Process process = new Process();
            process.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            process.StartInfo.Arguments = arguments ?? "--child";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            return process;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 取消订阅事件
                    Application.Current.DispatcherUnhandledException -= Application_DispatcherUnhandledException;
                    AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
                    TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
                    Application.Current.Exit -= Application_Exit;
                    Application.Current.SessionEnding -= Application_SessionEnding;
                    SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
                    AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
                }
                _disposed = true;
            }
        }

        ~EnhancedExceptionHandler()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// 退出日志分析工具
    /// </summary>
    public static class ExitAnalyzer
    {
        public static void AnalyzeExitLogs(string logDirectory)
        {
            if (!Directory.Exists(logDirectory))
            {
                Console.WriteLine("日志目录不存在");
                return;
            }

            var exitLogs = Directory.GetFiles(logDirectory, "exit_*.log");
            var crashLogs = Directory.GetFiles(logDirectory, "crash_*.log");

            Console.WriteLine($"找到 {exitLogs.Length} 个退出日志和 {crashLogs.Length} 个崩溃日志");

            // 分析退出原因分布
            var exitReasons = new Dictionary<ExitReason, int>();
            foreach (var logFile in exitLogs)
            {
                var content = File.ReadAllText(logFile);
                foreach (ExitReason reason in Enum.GetValues(typeof(ExitReason)))
                {
                    if (content.Contains($"Exit Reason: {reason}"))
                    {
                        if (exitReasons.ContainsKey(reason))
                            exitReasons[reason]++;
                        else
                            exitReasons[reason] = 1;
                        break;
                    }
                }
            }

            // 输出统计结果
            Console.WriteLine("退出原因统计:");
            foreach (var kvp in exitReasons.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value} 次");
            }
        }
    }
}
