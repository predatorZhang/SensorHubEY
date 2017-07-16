using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class FSHJTagHandler:TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType==12?true:false;
        
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag,
            SystemDateTag systemDateTag, CasicSession session)
        {
             //TODO LIST:解析腐蚀环境数据，保存腐蚀环境数据
            UploadTag fshjTag = tag as UploadTag;
            int itv = fshjTag.CollectInter;
            String collecTime = fshjTag.CollectTime;
            int len = fshjTag.Len;
            String dataValue = fshjTag.DataValue;

            session.Logger.Info("腐蚀环境数据上传TAG：oid：" + fshjTag.Oid + " 采集间隔: " +
                itv+"采集时间："+collecTime+"上传数值："+dataValue);

            int num = len /(2*16); //上传的数据组数，1组16个数据，每个数据4个字节
            List<Model.AKFSHJInfo> djs = new List<Model.AKFSHJInfo>();

            DateTime baseTime = Convert.ToDateTime(systemDateTag.CollectDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                Model.AKFSHJInfo fshjInfo = new  Model.AKFSHJInfo();

                String underVol1 = strHexToInt(dataValue.Substring(i * 64, 4)).ToString();
                String underVol2 = strHexToInt(dataValue.Substring(i * 64 + 4, 4)).ToString();
                String underVol3 = strHexToInt(dataValue.Substring(i * 64 + 8, 4)).ToString();
                String underVol4 = strHexToInt(dataValue.Substring(i * 64 + 12, 4)).ToString();
                String underVol5 = strHexToInt(dataValue.Substring(i * 64 + 16, 4)).ToString();
                String underVol6 = strHexToInt(dataValue.Substring(i * 64 + 20, 4)).ToString();


                String under_temp1 = (strHexToInt(dataValue.Substring(i * 64 + 24, 4))/10.0).ToString();
                String under_temp2 = (strHexToInt(dataValue.Substring(i * 64 + 28, 4))/10.0).ToString();
                String outter_temp1 =(strHexToInt(dataValue.Substring(i * 64 + 32, 4))/10.0).ToString();
                String outter_temp2 =(strHexToInt(dataValue.Substring(i * 64 + 36, 4))/10.0).ToString();

                String underWater1 = strHexToInt(dataValue.Substring(i * 64 + 40, 4)).ToString();
                String underWater2 = strHexToInt(dataValue.Substring(i * 64 + 44, 4)).ToString();
                String underWater3 = strHexToInt(dataValue.Substring(i * 64 + 48, 4)).ToString();
                String underWater4 = strHexToInt(dataValue.Substring(i * 64 + 52, 4)).ToString();
                String underWater5 = strHexToInt(dataValue.Substring(i * 64 + 56, 4)).ToString();
                String underWater6 = strHexToInt(dataValue.Substring(i * 64 + 60, 4)).ToString();

                fshjInfo.UnderTemp1 = under_temp1;
                fshjInfo.UnderTemp2 = under_temp2;
                fshjInfo.OutterTemp1 = outter_temp1;
                fshjInfo.OutterTemp2 = outter_temp2;
                fshjInfo.UnderVo11 = underVol1;
                fshjInfo.UnderVo12 = underVol2;
                fshjInfo.UnderVo13 = underVol3;
                fshjInfo.UnderVo14 = underVol4;
                fshjInfo.UnderVo15 = underVol5;
                fshjInfo.UnderVo16 = underVol6;

                fshjInfo.UnderWaterIn1 = underWater1;
                fshjInfo.UnderWaterIn2 = underWater2;
                fshjInfo.UnderWaterIn3 = underWater3;
                fshjInfo.UnderWaterIn4 = underWater4;
                fshjInfo.UnderWaterIn5 = underWater5;
                fshjInfo.UnderWaterIn6 = underWater6;

                fshjInfo.Cell = (cellTag == null ? "" : cellTag.Cell);
                fshjInfo.LogTime = DateTime.Now;
                fshjInfo.UpTime = baseTime.AddMinutes(i * itv);
                fshjInfo.DEVCODE = devCode;
                djs.Add(fshjInfo);
            }
            new BLL.AKFSHJ().insert(djs);
            String pipeType = new BLL.AKFSHJ().getHeatPipeTypeByDevCode(devCode);
            new BLL.AKFSHJ().saveAlarmInfo(djs, pipeType);
            //new BLL.AKFSHJ().saveAlarmInfo(djs);//todo list:生成报警规则
            session.Logger.Info("腐蚀环境数据保存成功");
          
        }

        //网络序列转float
        private float strHexToInt(String src)
        {
            if (src.Length != 4)
                return 0;

            byte[] lBt ={
                                   byte.Parse(src.Substring(0,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(2,2),System.Globalization.NumberStyles.HexNumber)
                               };
            float ss = BitConverter.ToInt16(lBt, 0);
            return ss;

        }
    }
}
