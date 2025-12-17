using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_Helper.FILE
{
    public class ControllerLog : ILogOpration
    {
        public  BlockingCollection<string> SendValueQueue { get; set; } = new BlockingCollection<string>();
        private CancellationTokenSource ctsQueue = new CancellationTokenSource();
        private CancellationTokenSource ctsIoE = new CancellationTokenSource();

        public ControllerLog()
        {
            WriteIOE();
        }
        public void WriteInfo(string Message)
        {
            try
            {
                SendValueQueue.Add($"  [Info]       {Message}");
            }
            catch (Exception ee)
            {
                SendValueQueue.Add("Log Error");
            }
        }
        public void WriteError(string Message)
        {
            try
            {
                SendValueQueue.Add($"   [Error]        {Message}");
            }
            catch (Exception ee)
            {
                SendValueQueue.Add("Log Error");
            }
        }
        public void WriteError(Exception EX)
        {
            try
            {
                SendValueQueue.Add($"   [Error]:{EX.StackTrace}\r\n{EX.Message}");
            }
            catch (Exception ee)
            {
                SendValueQueue.Add("Log Error");
            }
        }
        private void WriteIOE()
        {
            Task.Run(() =>
            {
                var token = ctsIoE.Token;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        Thread.Sleep(20);
                        if (SendValueQueue.Count == 0) { continue; }
                        string value = SendValueQueue.Take();
                        if (!string.IsNullOrEmpty(value))
                        {
                            string logValue = $"{DateTime.Now.ToString()}:{value}{Environment.NewLine}";
                            WirteLog(logValue); //给Robot发送数据
                            Thread.Sleep(30);
                        }
                    }
                    catch (Exception ee)
                    {

                    }

                }
            });
        }
        public void WirteLog(string Log)
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;
            int day = now.Day;

            // 创建以年月日命名的文件夹路径（格式：yyyy-MM-dd）
            string basePath = @"D:\324Logs\";
            string folderPath = Path.Combine(basePath, year.ToString(), month.ToString("00"), day.ToString("00"));

            // 如果文件夹不存在则创建
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine($"文件夹创建成功：{folderPath}");
            }
            else
            {
                Console.WriteLine($"文件夹已存在：{folderPath}");
            }

            // 创建文件路径（例如：日志文件）
            string filePath = Path.Combine(folderPath, $"log_{now:yyyyMMdd}.txt");

            // 创建文件并写入内容
            try
            {
                File.AppendAllText(filePath, Log);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"文件创建失败：{ex.Message}");
            }
        }

    }
}
