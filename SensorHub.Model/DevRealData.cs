using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DevRealData
    {
        private long dbId;
        private string devCode;
        private string hubCode;
        private string status;

        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }
        public string DevCode
        {
            get { return devCode; }
            set { devCode = value; }
        }
        public string HubCode
        {
            get { return hubCode; }
            set { hubCode = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
