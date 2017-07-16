using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SensorException0Tag:Tag
    {
      
         public static String SENSOR_EXCEP0_TAG_OID = "60000009";

        public String state;

        public SensorException0Tag()
        {
 
        }

        public SensorException0Tag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
            int intState = Int32.Parse(dataValue, System.Globalization.NumberStyles.HexNumber);
            state = intState + "";
        }
    }
}
