using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.ADCommands
{
    public class FlowTagHandler:TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType==1?true:false;
        
        }

        public override void execute(Tag tag)
        {
            //TODO LIST:解析流量数据保存流量数据
            UploadTag flowTag = tag as UploadTag;
            int itv = flowTag.CollectInter;
            String collecTime = flowTag.CollectTime;
            int len = flowTag.Len;
            String dataValue = flowTag.DataValue;

            AdlerCmd.adlerSession.Logger.Info("流量数据上传TAG：oid：" + flowTag.Oid + " 采集间隔: " +
              itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 12; //上传的流量数据个数
            List<Model.DjFlowInfo> djs = new List<DjFlowInfo>();

            DateTime baseTime  = Convert.ToDateTime(AdlerCmd.currentSystemDate+" "+collecTime);
            for (int i = 0; i < num; i++)
            {
                DjFlowInfo flowInfo = new DjFlowInfo();
                String insFlow = strHexToFloat(dataValue.Substring(i * 24, 8)).ToString();
                String posFlow = strHexToFloat(dataValue.Substring(i * 24 + 8, 8)).ToString();
                String negFlow = strHexToFloat(dataValue.Substring(i * 24 + 16, 8)).ToString();
                flowInfo.INSDATA = insFlow;
                flowInfo.POSDATA = posFlow;
                flowInfo.NEGDATA = negFlow;
                flowInfo.NETDATA = (float.Parse(posFlow) + float.Parse(negFlow))+"";

                //TODO LIST:电池电量的获取
                flowInfo.LOGTIME = DateTime.Now;
                flowInfo.UPTIME = baseTime.AddMinutes(i * itv);
                flowInfo.DEVID = AdlerCmd.devCode;

                djs.Add(flowInfo);
            }
            new BLL.DjFlow().insert(djs);
            new BLL.DjFlow().saveAlarmInfo(djs);
            new BLL.DjFlow().updateDevStatus(AdlerCmd.devCode);

            AdlerCmd.adlerSession.Logger.Info("流量数据保存成功");
            
        }

        //网络序列转float
        private float strHexToFloat(String src)
        {
            if (src.Length != 8)
                return 0;

            byte[] lBt ={
                                   byte.Parse(src.Substring(6,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(4,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(2,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(0,2),System.Globalization.NumberStyles.HexNumber)
                                 
                               };
            float ss = BitConverter.ToSingle(lBt, 0);
            return ss;

        }
    }
}
