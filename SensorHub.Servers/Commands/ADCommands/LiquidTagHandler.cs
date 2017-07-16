using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.ADCommands
{
    public class LiquidTagHandler:TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType==3?true:false;
        
        }

        public override void execute(Tag tag)
        {
             //TODO LIST:解析液位数据，保存液位数据
            UploadTag liquidTag = tag as UploadTag;
            int itv = liquidTag.CollectInter;
            String collecTime = liquidTag.CollectTime;
            int len = liquidTag.Len;
            String dataValue = liquidTag.DataValue;

            AdlerCmd.adlerSession.Logger.Info("液位数据上传TAG：oid："+liquidTag.Oid+" 采集间隔: "+
                itv+"采集时间："+collecTime+"上传数值："+dataValue);

            int num = len / 4; //上传的液位数据个数
            List<Model.DjLiquidInfo> djs = new List<Model.DjLiquidInfo>();

            DateTime baseTime = Convert.ToDateTime(AdlerCmd.currentSystemDate + " " + collecTime);
            for (int i = 0; i < num; i++)
            {
                DjLiquidInfo liquidInfo = new DjLiquidInfo();
                String liquid = strHexToFloat(dataValue.Substring(i * 8, 8)).ToString();
            
                //TODO LIST:电池电量
                liquidInfo.LIQUIDDATA = liquid;
                liquidInfo.LOGTIME = DateTime.Now;
                liquidInfo.UPTIME = baseTime.AddMinutes(i * itv);
                liquidInfo.DEVID = AdlerCmd.devCode;
                djs.Add(liquidInfo);
            }
            new BLL.DjLiquid().insert(djs);
            AdlerCmd.adlerSession.Logger.Info("液位数据保存成功");
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
