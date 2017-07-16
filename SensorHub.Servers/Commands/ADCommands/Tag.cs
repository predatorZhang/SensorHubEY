using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public abstract class Tag
    {
        private String oid;
        private int len;
        private String dataValue;

        public int Len
        {
            get { return len; }
            set { len = value; }
        }

        public String DataValue
        {
            get { return dataValue; }
            set { dataValue = value; }
        }

        public String Oid
        {
            get { return oid; }
            set { oid = value; }
        }

        public Tag(String oid,int len,String dataValue)
        {
            this.oid = oid;
            this.len = len;
            this.dataValue = dataValue;
        }

        public Tag()
        {
 
        }
      
    }
}
