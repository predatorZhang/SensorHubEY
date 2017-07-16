using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SensorException1Tag:Tag
    {
    
        public static String SENSOR_EXCEP1_TAG_OID = "6000000A";

        public String state;

        public SensorException1Tag()
        {
 
        }

        public SensorException1Tag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
            int intState = Int32.Parse(dataValue, System.Globalization.NumberStyles.HexNumber);
            state = intState + "";
        }
    }
}
