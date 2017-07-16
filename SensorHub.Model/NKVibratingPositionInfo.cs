using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class NKVibratingPositionInfo
    {
        private long dbId;
        private string devId;
        private string distance;
        private string vibrating;
        private string upTime;
        private string logTime;

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
        public string DISTANCE
        {
            get { return distance; }
            set { distance = value; }
        }
        public string VIBRATING
        {
            get { return vibrating; }
            set { vibrating = value; }
        }
        public string UPTIME
        {
            get { return upTime; }
            set { upTime = value; }
        }
        public string LOGTIME
        {
            get { return logTime; }
            set { logTime = value; }
        }
    }
}
