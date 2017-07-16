using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class NoiseTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType == 4 ? true : false;
        }

        //0000 0071 前四个字节暂时不用
        public override void execute(Tag tag,String devCode,CellTag cellTag,
            SystemDateTag systemDateTag,CasicSession session)
        {
            //TODO LIST:解析流量数据保存流量数据
            UploadTag noiseTag = tag as UploadTag;
            int itv = noiseTag.CollectInter;
            String collecTime = noiseTag.CollectTime;
            int len = noiseTag.Len;
            String dataValue = noiseTag.DataValue;

            session.Logger.Info("噪声数据上传TAG：oid：" + noiseTag.Oid + " 采集间隔: " +
  itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 4; //上传的流量数据个数,修改未2个字节//yxw修改为4个字节，加频率
            List<Model.SlNoiseInfo> djs = new List<SlNoiseInfo>();

            DateTime baseTime = Convert.ToDateTime(systemDateTag.CollectDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                // DjNoiseInfo noiseInfo = new DjNoiseInfo();
                SlNoiseInfo slNoiseInfo = new SlNoiseInfo();
                //TODO LIST:密集开始时间、密集间隔、密集样本数、无线开启时间、
                //无线关闭时间、密集噪声、电池电量
                slNoiseInfo.CELL = (cellTag == null ? "" : cellTag.Cell);
                slNoiseInfo.LOGTIME = DateTime.Now;
                slNoiseInfo.UPTIME = baseTime.AddMinutes(i * itv);
                slNoiseInfo.SRCID = devCode;
                slNoiseInfo.DSTID = "FFFF";
                String dStr = dataValue.Substring(i * 8 + 0, 4);
                String frequency = dataValue.Substring(i * 8 + 4, 4);

                slNoiseInfo.DENSEDATA = int.Parse(dStr.Substring(0, 2) + dStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                slNoiseInfo.FREQUENCY = int.Parse(frequency.Substring(0, 2) + frequency.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                djs.Add(slNoiseInfo);
            }
            new BLL.SlNoise().insert(djs);
            new BLL.SlNoise().SaveZZAlarmInfo(djs);
            new BLL.SlNoise().updateDevStatus(devCode);

            session.Logger.Info("噪声数据保存成功");
        }
    }
}
