using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class PressTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType == 9 ? true : false;
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag, 
            SystemDateTag systemDateTag,CasicSession session)
        {
            //TODO LIST:解析压力数据保存压力数据
            UploadTag pressTag= tag as UploadTag;
            int itv = pressTag.CollectInter;
            String collecTime = pressTag.CollectTime;
            int len = pressTag.Len;
            String dataValue = pressTag.DataValue;

            session.Logger.Info("压力数据上传TAG：oid：" + pressTag.Oid + " 采集间隔: " +
                itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 4; //上传的液位数据个数
            //List<Model.CasicPress> djs = new List<Model.CasicPress>();
            List<Model.DjPressInfo> djs = new List<Model.DjPressInfo>();

            DateTime baseTime = Convert.ToDateTime(systemDateTag.CollectDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                DjPressInfo pressInfo = new DjPressInfo();
                String press = strHexToFloat(dataValue.Substring(i * 8, 8)).ToString();

                //TODO LIST:电池电量
                pressInfo.CELL = (cellTag == null ? "" : cellTag.Cell);
                pressInfo.PRESSDATA = press;
                pressInfo.LOGTIME = DateTime.Now;
                pressInfo.UPTIME = baseTime.AddMinutes(i * itv);
                pressInfo.DEVID = devCode;

                djs.Add(pressInfo);
            }

            new BLL.DjPress().insert(djs);
            //new BLL.DjPress().saveAlarmInfo(djs);
            new BLL.DjPress().updateDevStatus(devCode);
            session.Logger.Info("压力数据保存成功");

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
