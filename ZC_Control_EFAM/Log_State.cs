using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZC_Control_EFAM
{
    /// <summary>
    /// 写日志类
    /// </summary>
    public class Log_State
    {
        #region 字段

        public static string LogPath = "D:\\Info\\Status\\";
        public static int fileSize = 10 * 1024 * 1024; //日志分隔文件大小
        private static ConcurrentQueue<Tuple<string, DateTime>> queue =
            new ConcurrentQueue<Tuple<string, DateTime>>();

        #endregion 字段

        #region 构造函数

        static Log_State()
        {
            Task.Factory.StartNew(
                new Action(
                    delegate ()
                    {
                        StringBuilder log;
                        string path;
                        Tuple<string, DateTime> tuple;
                        string item;

                        while (true)
                        {
                            log = new StringBuilder();
                            path = CreateLogPath();

                            while (queue.TryDequeue(out tuple))
                            {
                                item = string.Format(
                                    @"{0} {1}",
                                    tuple.Item2.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                    tuple.Item1
                                );
                                log.AppendFormat("\r\n{0}", item);
                            }

                            if (log.Length > 0)
                                WriteFile(log.ToString(2, log.Length - 2), path);

                            Thread.Sleep(200);
                        }
                    }
                )
            );
        }

        #endregion 构造函数

        #region 写文件

        /// <summary>
        /// 写文件
        /// </summary>
        public static void WriteFile(string log, string path)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                if (!File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        fs.Close();
                    }
                }
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(log);
                        sw.Flush();
                    }
                    fs.Close();
                }
            }
            catch { }
        }

        #endregion 写文件

        #region 生成日志文件路径

        /// <summary>
        /// 生成日志文件路径
        /// </summary>
        public static string CreateLogPath()
        {
            int index = 0;
            string logPath;
            bool bl = true;
            do
            {
                index++;
                logPath = Path.Combine(
                    LogPath,
                    DateTime.Now.ToString("yyyyMMdd"),
                    DateTime.Now.ToString("yyyyMMddHH")
                        + (index == 1 ? "" : "_" + index.ToString())
                        + ".txt"
                );
                if (File.Exists(logPath))
                {
                    FileInfo fileInfo = new FileInfo(logPath);
                    if (fileInfo.Length < fileSize)
                    {
                        bl = false;
                    }
                }
                else
                {
                    bl = false;
                }
            } while (bl);

            return logPath;
        }

        #endregion 生成日志文件路径

        #region 写错误日志

        /// <summary>
        /// 写错误日志
        /// </summary>
        public static void LogError(string log)
        {
            queue.Enqueue(new Tuple<string, DateTime>("[Error] " + log, DateTime.Now));
        }

        #endregion 写错误日志

        #region 写操作日志

        /// <summary>
        /// 写操作日志
        /// </summary>
        public static void LogInfo(string log)
        {
            queue.Enqueue(new Tuple<string, DateTime>("[Info]  " + log, DateTime.Now));
        }

        #endregion 写操作日志
    }
}
