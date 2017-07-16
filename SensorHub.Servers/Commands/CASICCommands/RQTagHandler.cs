using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class RQTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType == 5 ? true : false;
        }


        public override void execute(Tag tag, String devCode, CellTag cellTag, 
            SystemDateTag systemDateTag,CasicSession session)
        {
            //TODO LIST:解析压力数据保存压力数据
            UploadTag rqTag = tag as UploadTag;
            int itv = rqTag.CollectInter;
            String collecTime = rqTag.CollectTime;
            int len = rqTag.Len;
            String dataValue = rqTag.DataValue;

            session.Logger.Info("燃气数据上传TAG：oid：" + rqTag.Oid + " 采集间隔: " +
                itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 4; //上传的燃气数据个数
            List<Model.RQPeriodInfo> rqs = new List<Model.RQPeriodInfo>();

            DateTime baseTime = Convert.ToDateTime(systemDateTag.CollectDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                String density = strHexToFloat(dataValue.Substring(i * 8, 8)).ToString();

                Model.RQPeriodInfo rq = new Model.RQPeriodInfo();
                rq.ADDRESS = devCode;
                rq.INPRESS = "0";
                rq.OUTPRESS = "0";
                rq.FLOW = "0";
                rq.STRENGTH = density;
                rq.TEMPERATURE = "0";
                rq.CELL = (cellTag == null ? "" : cellTag.Cell);
                rq.UPTIME = baseTime.AddMinutes(i * itv);
                rq.LOGTIME = DateTime.Now;
                rqs.Add(rq);
            }
            BLL.RQPeriod bll = new BLL.RQPeriod();
            bll.insert(rqs);
            bll.saveAlarmInfo(rqs);
            session.Logger.Info("燃气数据保存成功");
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
