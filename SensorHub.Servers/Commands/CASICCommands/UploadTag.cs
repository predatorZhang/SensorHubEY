using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class UploadTag:Tag
    {
        /**
         * 1,2,3,4,5,分别代表流量、压力、液位、噪声
         */
        private int bizType; //业务类型
        private int collectInter; //采集间隔(单位是分钟)
        private String collectTime; //采集时间（15时）
        /*
        * if oid beigins with 1100 return true
        */
        public static bool isUploadTag(String oid)
        {
           int intoid = Int32.Parse(oid, System.Globalization.NumberStyles.HexNumber);
           int temp  = (intoid >>28) & 0x0F;
           return temp == 0x0C ? true : false;
        }

        public UploadTag()
        {
 
        }

        public UploadTag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {

            int btpduType = Int32.Parse(this.Oid, System.Globalization.NumberStyles.HexNumber);
            bizType = (btpduType >> 24) & 0x0F;

            //TODO LIST:转换采集间隔
            collectInter = (btpduType >> 11) & 0x7FF;

            int collectMin = btpduType & 0x7FF;
            int minute = collectMin % 60;
            int hour = collectMin / 60;
            collectTime = hour + ":" + minute+":00";
        }

        public int BizType
        {
            get { return bizType; }
            set { bizType = value; }
        }

        public int CollectInter
        {
            get { return collectInter; }
            set { collectInter = value; }
        }

        public String CollectTime
        {
            get { return collectTime; }
            set { collectTime = value; }
        }

       
    }
}
