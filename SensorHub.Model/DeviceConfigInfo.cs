using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DeviceConfigInfo
    {
        private int dbid;
        private string deviceCode;
        private string frameContent;
        private DateTime sendTime;
        private DateTime writeTime;
        private string sensorCode;
        private bool status;

        public int DBID
        {
            get { return dbid; }
            set { dbid = value; }
        }

        public string DeviceCode
        {
            get { return deviceCode; }
            set { deviceCode = value; }
        }

        public string FrameContent
        {
            get { return frameContent; }
            set { frameContent = value; }
        }

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }

        public DateTime WriteTime
        {
            get { return writeTime; }
            set { writeTime = value; }
        }

        public DateTime SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        public String SensorCode
        {
            get { return sensorCode; }
            set { sensorCode = value; }
        }
    }
}
