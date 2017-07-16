using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SystemTimeTag:Tag
    {
        /**
         * value;100115 对应于2016年1月21日
         */
        private String sysTime; //采集的系统日期

        public static String SYSTEM_TIME_OID = "10000051";

        public SystemTimeTag()
        {
 
        }

        public SystemTimeTag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
            
            int year = 2000+Int32.Parse(this.DataValue.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            int month = Int32.Parse(this.DataValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int day = Int32.Parse(this.DataValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            int hour = Int32.Parse(this.DataValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            int min = Int32.Parse(this.DataValue.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            int second = Int32.Parse(this.DataValue.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);

            sysTime = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + second;
             
        }

        public String SysTime
        {
            get { return sysTime; }
            set { sysTime = value; }
        }

    }
}
