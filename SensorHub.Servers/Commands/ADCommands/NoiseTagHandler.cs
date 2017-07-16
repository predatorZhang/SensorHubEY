using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.ADCommands
{
    public  class NoiseTagHandler:TagHandler
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
        public override void execute(Tag tag)
        {
            //TODO LIST:解析流量数据保存流量数据
            UploadTag noiseTag = tag as UploadTag;
            int itv = noiseTag.CollectInter;
            String collecTime = noiseTag.CollectTime;
            int len = noiseTag.Len;
            String dataValue = noiseTag.DataValue;

            AdlerCmd.adlerSession.Logger.Info("噪声数据上传TAG：oid：" + noiseTag.Oid + " 采集间隔: " +
  itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 4; //上传的流量数据个数
            List<Model.DjNoiseInfo> djs = new List<DjNoiseInfo>();

            DateTime baseTime = Convert.ToDateTime(AdlerCmd.currentSystemDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                DjNoiseInfo noiseInfo = new DjNoiseInfo();
                //TODO LIST:密集开始时间、密集间隔、密集样本数、无线开启时间、
                //无线关闭时间、密集噪声、电池电量
                noiseInfo.LOGTIME = DateTime.Now;
                noiseInfo.UPTIME = baseTime.AddMinutes(i * itv);
                noiseInfo.DEVID = AdlerCmd.devCode;
                String dStr = dataValue.Substring(i * 8 + 4, 4);
                noiseInfo.DDATA = int.Parse("0"+dStr.Substring(1, 1) + dStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                djs.Add(noiseInfo);
            }
            new BLL.DjNoise().insert(djs);
            new BLL.DjNoise().saveAlarmInfo(djs);
            new BLL.DjNoise().updateDevStatus(AdlerCmd.devCode);
            AdlerCmd.adlerSession.Logger.Info("噪声数据保存成功");
        }
    }
}
