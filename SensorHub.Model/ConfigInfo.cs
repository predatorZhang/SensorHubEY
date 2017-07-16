using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class ConfigInfo
    {
        private long dbId;
        private string devId;
        private string frameContent;
        private DateTime writeTime;
        private DateTime sendTime;
        private bool status;

        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string DEVID
        {
            get { return devId; }
            set { devId = value; }
        }

        public string FRAMECONTENT
        {
            get { return frameContent; }
            set { frameContent = value; }
        }

        public DateTime WRITETIME
        {
            get { return writeTime; }
            set { writeTime = value; }
        }

        public DateTime SENDTIME
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        public bool STATUS
        {
            get { return status; }
            set { status = value; }
        }
    }
}
