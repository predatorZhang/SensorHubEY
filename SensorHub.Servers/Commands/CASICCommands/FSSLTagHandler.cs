using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class FSSLTagHandler:TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType==11?true:false;
        
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag,
            SystemDateTag systemDateTag, CasicSession session)
        {

            //TODO LIST:解析腐蚀速率数据，保存腐蚀速率
            
            UploadTag fsslTag = tag as UploadTag;
            int itv = fsslTag.CollectInter;
            String collecTime = fsslTag.CollectTime;
            int len = fsslTag.Len;
            String dataValue = fsslTag.DataValue;

            session.Logger.Info("腐蚀速率数据上传TAG：oid：" + fsslTag.Oid + " 采集间隔: " +
                itv+"采集时间："+collecTime+"上传数值："+dataValue);

            int num = len / 20; //上传的腐蚀速率的组数，1组5个数据，1个数据4个字节
            /*
             * 开路电位OCP：单位V
溶液电阻Rs：单位Ω.cm2
极化电阻Rp：单位Ω.cm2
腐蚀电流密度Icorr：单位mA/cm2
腐蚀速率Vcorr：单位mm/a
             * */
            List<Model.AKFSSLInfo> djs = new List<Model.AKFSSLInfo>();

            DateTime baseTime = Convert.ToDateTime(systemDateTag.CollectDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                Model.AKFSSLInfo fssl = new Model.AKFSSLInfo();
                String openCircultVol = strHexToFloat(dataValue.Substring(i * 40, 8)).ToString("f4");
                String ryResist = strHexToFloat(dataValue.Substring(i * 40+8, 8)).ToString("f4");
                String jhResist = strHexToFloat(dataValue.Substring(i * 40+16, 8)).ToString("f4");
                String concurDen = strHexToFloat(dataValue.Substring(i * 40+24, 8)).ToString("f4");
                String errosionRate = strHexToFloat(dataValue.Substring(i * 40+32, 8)).ToString("f4");

                fssl.Cell = (cellTag == null ? "" : cellTag.Cell);
                fssl.OpenCir=openCircultVol;
                fssl.RyResist=ryResist;
                fssl.JhResist = jhResist;
                fssl.CurrentDen=concurDen;
                fssl.ErrosionRat=errosionRate;
                fssl.LogTime = DateTime.Now;
                fssl.UpTime = baseTime.AddMinutes(i * itv);
                fssl.DEVCODE = devCode;
                djs.Add(fssl);
            }
            new BLL.AKFSSL().insert(djs);
            new BLL.AKFSSL().saveAlarmInfo(djs);
          //  new BLL.DjLiquid().updateDevStatus(CasicCmd.devCode);
            session.Logger.Info("腐蚀速率数据保存成功");
           
        }

        //网络序列转float
        private float strHexToFloat(String src)
        {
            if (src.Length != 8)
                return 0;

            byte[] lBt ={
                                   byte.Parse(src.Substring(0,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(2,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(4,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(6,2),System.Globalization.NumberStyles.HexNumber)
                                 
                               };
            float ss = BitConverter.ToSingle(lBt, 0);
            return ss;

        }
    }
}
