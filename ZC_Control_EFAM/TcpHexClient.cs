using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using SuperSimpleTcp;

namespace ZC_Control_EFAM
{
    public class TcpHexClient
    {
        private SimpleTcpClient client;
        private string ip;
        private int port;

        private uint transactionId = 0;
        private readonly object idLock = new object();
        private readonly object rcvLock = new object();
        private readonly List<byte> receiveBuffer = new List<byte>();
        private readonly StringBuilder receiveBuffer1 = new StringBuilder();

        private BlockingCollection<byte[]> sendQueue = new BlockingCollection<byte[]>();
        private Thread sendThread;

        public bool IsConnected => client.IsConnected;

        //public bool IsConnected { get; set; }
        public event Action<long, List<byte>> OnHexDataReceived;
        public event Action<string> OnSended;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public TcpHexClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            client = new SimpleTcpClient($"{ip}:{port}");
            client.Events.Connected += (s, e) =>
            {
                OnConnected?.Invoke();
            };
            client.Events.Disconnected += (s, e) =>
            {
                DisplayLog($"通信连接断开！", 3);
                OnDisconnected?.Invoke();
            };
            client.Events.DataReceived += (s, e) =>
            {
             
              
                //ProcessReceived(e.Data.Slice(0, e.Data.Count).ToArray());
                byte[] data = e.Data.Take(e.Data.Count).ToArray();
                ProcessReceived(data);
                
            };

            sendThread = new Thread(SendQueueWorker) { IsBackground = true };
            sendThread.Start();
        }

        public bool Connect()
        {
            try
            {
                client.Connect();
                DisplayLog($"通信连接成功！", 3);
                return IsConnected;
            }
            catch
            {
                DisplayLog($"通信连接失败！", 3);
                return false;
            }
        }

        public void Disconnect()
        {
            client.Disconnect();
            while (sendQueue.TryTake(out _)) { }
            //sendQueue.CompleteAdding();
        }

        public long SendHexPayload(string hex)
        {
            if (!IsConnected)
                return 0;
            byte[] payload = HexStringToBytes(hex);

            lock (idLock)
            {
                transactionId++;
                if (transactionId == 0 || transactionId > 0x0000FFFF)
                    transactionId = 1;


                byte[] idBytes = BitConverter.GetBytes(transactionId);

                int totalLength = 4 + payload.Length;

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(idBytes);

                byte[] fullPacket = new byte[1 + totalLength];
                fullPacket[0] = (byte)totalLength;
                Buffer.BlockCopy(idBytes, 0, fullPacket, 1, 4);
                Buffer.BlockCopy(payload, 0, fullPacket, 5, payload.Length);
                sendQueue.Add(fullPacket);
                return transactionId;
            }
        }

        private void SendQueueWorker()
        {
            foreach (var hex in sendQueue.GetConsumingEnumerable())
            {
                if (!IsConnected)
                    continue;

                try
                {
                    //byte[] payload = HexStringToBytes(hex);

                    //lock (idLock)
                    //{
                    //    transactionId++;
                    //    if (transactionId == 0 || transactionId > 0xFFFFFFFF)
                    //        transactionId = 1;
                    //}

                    //byte[] idBytes = BitConverter.GetBytes(transactionId);

                    //if (BitConverter.IsLittleEndian)
                    //    Array.Reverse(idBytes);

                    //int totalLength = 4 + payload.Length;
                    //if (totalLength > 255)
                    //    continue; // 丢弃错误数据

                    //byte[] fullPacket = new byte[1 + totalLength];
                    //fullPacket[0] = (byte)totalLength;
                    //Buffer.BlockCopy(idBytes, 0, fullPacket, 1, 4);
                    //Buffer.BlockCopy(payload, 0, fullPacket, 5, payload.Length);
                    string aaa = string.Empty;
                    foreach (var item in hex)
                    {
                        aaa += item.ToString("X2");
                    }

                    client.Send(hex);
                    OnSended?.Invoke(BitConverter.ToString(hex).Replace("-", " "));

                    if (CheckSendIsStatus(hex))
                        DisplayLog_1(BitConverter.ToString(hex).Replace("-", " "), 0);
                    else
                        DisplayLog(BitConverter.ToString(hex).Replace("-", " "), 0);
                }
                catch (Exception ex)
                {
                    DisplayLog($"Command发送失败！{ex.Message}", 3);
                }
            }
        }

        private void ProcessReceived(byte[] data)
        {
            lock (rcvLock)
            {

                try
                {
                    receiveBuffer.AddRange(data);
                    //receiveBuffer1.Append(Encoding.UTF8.GetString(data));

                    while (receiveBuffer.Count > 5)
                    {
                        byte length = receiveBuffer[0];
                        if (receiveBuffer.Count < length + 1)
                            break;
                        List<byte> subList = receiveBuffer.Skip(0).Take(length + 1).ToList();
                        receiveBuffer.RemoveRange(0, length + 1);

                        Byte[] bytes = subList.Skip(1).Take(4).ToArray();
                        Array.Reverse(bytes);
                        uint u32 = BitConverter.ToUInt32(bytes, 0);
                        long intValue = u32;
                        OnHexDataReceived?.Invoke(intValue, subList);
                        if (CheckRecvIsStatus(subList))
                            DisplayLog_1(BitConverter.ToString(subList.ToArray()).Replace("-", " "), 1);
                        else
                            DisplayLog(BitConverter.ToString(subList.ToArray()).Replace("-", " "), 1);
                    }
                }
                catch (Exception ex)
                {
                    ;
                }
            }
        }

        private byte[] HexStringToBytes(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
                throw new ArgumentException("十六进制字符串长度必须为偶数");

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return result;
        }

        public void DisplayLog(string strLog, int lx)
        {
            switch (lx)
            {
                case 0:
                    LogU.LogInfo("Send:     " + strLog);
                    break;
                case 1:
                    LogU.LogInfo("Recive:   " + strLog);
                    break;
                default:
                    LogU.LogInfo(strLog);
                    break;
            }
        }

        public void DisplayLog_1(string strLog, int lx)
        {
            switch (lx)
            {
                case 0:
                    Log_State.LogInfo("Send:     " + strLog);
                    break;
                case 1:
                    Log_State.LogInfo("Recive:   " + strLog);
                    break;
                default:
                    Log_State.LogInfo(strLog);
                    break;
            }
        }

        private bool CheckSendIsStatus(byte[] bytes)
        {
            if (bytes[5] == 0 && bytes[7] == 1)
                return true;
            else if (bytes[5] == 02 && bytes[7] == 1)
                return true;
            else if (bytes[5] == 02 && bytes[7] == 0x32)
                return true;
            else
                return false;
        }

        private bool CheckRecvIsStatus(List<byte> bytes)
        {
            if (bytes[5] == 1 && bytes[7] == 0x61)
                return true;
            else if (bytes[5] == 3 && bytes[7] == 01)
                return true;
            else if (bytes[5] == 3 && bytes[7] == 0x18)
                return true;
            else if (bytes[5] == 0 && bytes[6] == 0x0)
                return true;
            else if (bytes[5] == 4)
                return true;
            else
                return false;
        }
    }
}
