using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DeviceStatus
    {
        private long dbId;
        private string devCode;
        private DateTime lastTime;
        private bool status;
        private string sensorTypeCode;

        public string SensorTypeCode
        {
            get { return sensorTypeCode; }
            set { sensorTypeCode = value; }
        }

        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string DEVCODE
        {
            get { return devCode; }
            set { devCode = value; }
        }

        public DateTime LASTTIME
        {
            get { return lastTime; }
            set { lastTime = value; }
        }

        public bool STATUS
        {
            get { return status; }
            set { status = value; }
        }
    }
}
