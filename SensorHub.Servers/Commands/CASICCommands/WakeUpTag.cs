using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class WakeUpTag:Tag
    {
      
        private bool isWakeUp; //是否已经唤醒

        public static String WAKEUP_TAG_OID = "60000200";

        public WakeUpTag()
        {
 
        }

        public WakeUpTag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
            int day = Int32.Parse(dataValue, System.Globalization.NumberStyles.HexNumber);
            isWakeUp = day == 1 ? true : false;
        }

        public bool IsWAkeUp
        {
            get { return isWakeUp; }
            set { isWakeUp = value; }
        }

    }
}
