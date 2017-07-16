using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public class NormalTag:Tag
    {

        public NormalTag() 
        {

        }

        public NormalTag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
 
        }
    }
}
