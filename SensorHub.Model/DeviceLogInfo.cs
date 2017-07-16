using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DeviceLogInfo
    {
        private long dbid;
        private long deviceId;
        private string message;
        private string operateType;
        private DateTime logTime;

        public long DBID
        {
            get { return dbid; }
            set { dbid = value; }
        }

        public long DEVICE_ID
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public string MESSAGE
        {
            get { return message; }
            set { message = value; }
        }

        public string OPERATETYPE
        {
            get { return operateType; }
            set { operateType = value; }
        }

        public DateTime LOGTIME
        {
            get { return logTime; }
            set { logTime = value; }
        }
    }
}
