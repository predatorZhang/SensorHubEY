using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class WaterMeterTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType == 10 ? true : false;
        }

       
        public override void execute(Tag tag,String devCode,CellTag cellTag,
            SystemDateTag systemDateTag,CasicSession session)
        {
            //TODO LIST:解析水表数据，保存水表欧数据
            UploadTag tempTag= tag as UploadTag;
            int itv = tempTag.CollectInter;
            String collecTime = tempTag.CollectTime;
            int len = tempTag.Len;
            String dataValue = tempTag.DataValue;

            session.Logger.Info("水表数据上传TAG：oid：" + tempTag.Oid + " 采集间隔: " +
              itv + "采集时间：" + collecTime + "上传数值：" + dataValue);

            int num = len / 4; //上传的温度数据个数
            List<Model.CasicWaterMeter> djs = new List<Model.CasicWaterMeter>();

            DateTime baseTime = Convert.ToDateTime(systemDateTag.CollectDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                Model.CasicWaterMeter tempInfo = new CasicWaterMeter();
                String temp = strHexToInt32(dataValue.Substring(i * 8, 8)).ToString();

                //TODO LIST:电池电量
                tempInfo.Cell = (cellTag == null ? "" : cellTag.Cell);
                tempInfo.Data = temp;
                tempInfo.LogTime = DateTime.Now;
                tempInfo.UpTime = baseTime.AddMinutes(i * itv);
                tempInfo.DEVCODE = devCode;
                djs.Add(tempInfo);
            }
            new BLL.CasicWaterMeter().insert(djs);
            new BLL.CasicWaterMeter().saveAlarmInfo(djs);
            new BLL.CasicWaterMeter().updateDevStatus(devCode);
            session.Logger.Info("水表数据保存成功");
        }

        //网络序列转float
        private Int32 strHexToInt32(String src)
        {
            if (src.Length != 8)
                return 0;

            byte[] lBt ={
                                   byte.Parse(src.Substring(0,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(2,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(4,2),System.Globalization.NumberStyles.HexNumber),
                                   byte.Parse(src.Substring(6,2),System.Globalization.NumberStyles.HexNumber)
                                 
                               };
            Int32 ss = BitConverter.ToInt32(lBt, 0);
            return ss;

        }
    }
}
