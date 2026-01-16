using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC_Control_Helper;
using BC_Control_Models;
using BC_Control_Models.eap;
using BC_Control_Models.EAP;
using ZC_Control_System;
using ZCCommunication;

namespace BC_Control_BLL.Services
{

    public class EAPService
    {
        private IPLCHelper _plcHelper;
        private XHuaiEAPClass eapStatus;
        private CancellationTokenSource _cts;
        private string cjPath;
        private string pjPath;
        private int cjuStatus = 260000;
        private int cjsID = 260100;
        private int pjuStatus = 260500;
        private int pjsID = 260600;
        private int PJStatus;
        private int offlineAddress = 5001;
        private int onlineRemoteAddress = 5002;
        private int onlineLocalAddress = 5003;
        public Action<string, string, string, string> CJStartAction;
        public Func<string> CJStopAction;
        public Action<int, string> CarrierReleaseAction;
        public Action<int, string> CarrierOutAction;
        public bool CJEndState { get; set; }
        public int EAPControlState { get; set; }

        public EAPService(XHuaiEAPClass _eapStatus, IPLCHelper plcHelper)
        {
            _cts = new CancellationTokenSource();
            _plcHelper = plcHelper;
            eapStatus = _eapStatus;
            //cjPath = @"D:\XHuai\LotJob\CJ";
            //pjPath = @"D:\XHuai\LotJob\PJ";
            cjPath = @"D:\EAP\LotJob\CJ";
            pjPath = @"D:\EAP\LotJob\PJ";

        }
        private void CJEndAction()
        {
            if (CJEndState)
            {
                CJStopAction?.Invoke();
            }
        }
        public bool ChangeEAPControlState(int value)
        {
            try
            {
                switch (value)
                {
                    case 0:
                        return _plcHelper.SelectPLC().Write($"M{offlineAddress}","True").IsSuccess;
                    case 1:
                        return _plcHelper.SelectPLC().Write($"M{onlineLocalAddress}", "True").IsSuccess;
                    case 2:
                        return _plcHelper.SelectPLC().Write($"M{onlineRemoteAddress}", "True").IsSuccess;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task RunUpdateEapStatus(CancellationTokenSource cts)
        {
            _cts = cts;
            await Task.Run(() =>
            {
                try
                {
                    Device device = _plcHelper.SelectDevice();
                    string cjpathTemp = Path.Combine(cjPath, "CJMListJson.json");
                    if (!File.Exists(cjpathTemp)) return;
                    eapStatus.CJCollection = Directory.GetDirectories(cjPath).ToList();
                    string tempjsCJ = File.ReadAllText(cjpathTemp);
                    eapStatus.CJS = JSONHelper.JSONToEntity<RootObject>(tempjsCJ);
                    if (device.IsConnected)
                    {
                        eapStatus.EAPMode = Convert.ToInt32(device[$"D12"].ToString());
                        EAPControlState = eapStatus.EAPMode;
                        //获取当前storage工位运行的状态信息
                        for (int i = 0; i < eapStatus.EAPStatusCollection.Count(); i++)
                        {
                            eapStatus.EAPStatusCollection[i].No = i;
                            eapStatus.EAPStatusCollection[i].CJStatus = device[$"ZR{cjuStatus + (i * 1)}"].ToString();
                            eapStatus.EAPStatusCollection[i].CJID = device[$"ZR{cjsID + i * 20}"].ToString().Replace("\0", "");
                            eapStatus.EAPStatusCollection[i].PJStatus = device[$"ZR{pjuStatus + i * 1}"].ToString();
                            eapStatus.EAPStatusCollection[i].PJID = device[$"ZR{pjsID + i * 20}"].ToString().Replace("\0", "");

                        }
                        ControlJobStartFun();
                        CarrierOutOrRelease();
                    }
                }
                catch (Exception ee)
                {
                    throw;
                }

            }, cts.Token);
        }
        public void ControlJobStartFun()
        {
            try
            {
                if (eapStatus.EAPMode != 3)
                {
                    return;
                }
                foreach (var item in eapStatus.CJS.CJS)
                {
                    EAPStatusClass temp;
                    Thread.Sleep(1000);
                    if (!(eapStatus.EAPStatusCollection?.Where(P => P.CJID == item.objid)?.FirstOrDefault() == null))
                    {
                        temp = eapStatus.EAPStatusCollection?.Where(P => P.CJID == item.objid)?.FirstOrDefault();
                        if ((temp.PJID == null) || (temp.CJID == null) || (temp.CJStatus == null) || (temp.PJStatus == null) || (temp == null)) continue;
                    }
                    else
                    {
                        continue;
                    }
                    if (temp.CJStatus == "3")
                    {
                        string RFID1 = item.carrierinputspec[0];
                        string RFID2 = null;
                        //根据当前的PJID 未运行的运单号
                        if (item.carrierinputspec.Count() == 2)
                        {
                            RFID2 = item.carrierinputspec[1];
                        }
                        string cjTempName = temp.CJID;
                        string pjTempName = temp.PJID;
                        string tempPath = Path.Combine(cjPath, $"{temp.CJID}", $"{temp.PJID}.json");
                        string temppj = File.ReadAllText(tempPath);
                        ProcessJobClass temppjEntity = JSONHelper.JSONToEntity<ProcessJobClass>(temppj);
                        string tempRecipeName = $"{temppjEntity.Recipe.Name}.json";
                        CJStartAction?.Invoke(cjTempName, tempRecipeName, RFID1, RFID2);
                        CommonMethods.CommonWrite($"ZR{cjuStatus + temp.No}", "4");
                        Thread.Sleep(1000);
                        CommonMethods.CommonWrite($"ZR{pjuStatus + temp.No}", "4");
                    }
                }            
            }
            catch (Exception ex)
            {

            }

        }
        public async Task ControlJobFinishFunc(string cjtemp)
        {
            await Task.Run(async () =>
            {
                try
                {
                    //tring cjtemp = CJStopAction?.Invoke();
                    List<EAPStatusClass> tempend = eapStatus.EAPStatusCollection.Where(P => P.CJID == cjtemp).ToList();
                    if (tempend.Count != 0)
                    {
                        foreach (EAPStatusClass eapstatus in tempend)
                        {
                            _plcHelper.CommonWrite($"ZR{pjuStatus + eapstatus.No}", "5");
                            await Task.Delay(1000);
                            _plcHelper.CommonWrite($"ZR{cjuStatus + eapstatus.No}", "6");
                        }
                    }

                }
                catch (Exception ee)
                {
                    throw;
                }
            });
        }

        /// <summary>
        /// CarrierOut Or Release 20251016
        /// </summary>
        /// <summary>
        /// CarrierOut Or Release 20251016
        /// </summary>
        private void CarrierOutOrRelease()
        {
            try
            {
                int downloadReleaseStatus = 271125;
                int downloadoutStatus = 270040;
                int downloadReleaseCarrierID = 258000;//LP1起始地址  共4个LP  
                int downloadOutCarrierID = 258100;//LP1起始地址  共4个LP  


                for (int i = 0; i < 4; i++)
                {
                    OperateResult<short> releaseAck = PLCSelect.Instance.SelectPLC(PlcEnum.PLC1).ReadInt16($"ZR{downloadReleaseStatus + i * 1}");

                    if (releaseAck.Content == 1)
                    {
                        string carrierIDByRelease = PLCSelect.Instance.SelectPLC(PlcEnum.PLC1).ReadString($"ZR{downloadReleaseCarrierID + i * 20}", 20).Content.Replace('\0', ' ').Trim();
                        CarrierReleaseAction?.Invoke(i + 1, carrierIDByRelease);
                        PLCSelect.Instance.SelectPLC(PlcEnum.PLC1).Write($"ZR{downloadReleaseStatus + i * 1}", (short)0);
                    }
                    OperateResult<short> outdAck = PLCSelect.Instance.SelectPLC(PlcEnum.PLC1).ReadInt16($"ZR{downloadoutStatus + i * 1}");
                    if (outdAck.Content == 1)
                    {
                        string carrierIDByOut = PLCSelect.Instance.SelectPLC(PlcEnum.PLC1).ReadString($"ZR{downloadOutCarrierID + i * 20}", 20).Content.Trim();
                        CarrierOutAction?.Invoke(i + 1, carrierIDByOut);
                        bool b = PLCSelect.Instance.SelectPLC(PlcEnum.PLC1).Write($"ZR{downloadoutStatus + i * 1}", (short)0).IsSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }

}
