using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public class SystemDateTag:Tag
    {
        /**
         * value;100115 对应于2016年1月21日
         */
        private String collectDate; //采集的系统日期

        public static String SYSTEM_DATE_OID = "10000050";

        public SystemDateTag()
        {
 
        }

        public SystemDateTag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
            int year = 2000+Int32.Parse(this.DataValue.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            int month = Int32.Parse(this.DataValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int day = Int32.Parse(this.DataValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            collectDate = year + "-" + month + "-" + day;
        }

        public String CollectDate
        {
            get { return collectDate; }
            set { collectDate = value; }
        }

    }
}
