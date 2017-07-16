using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class NKVibratingCurveInfo
    {
        private long dbId;
        private string devId;
        private string distance;
        private string vibrating;
        private DateTime upTime;
        private DateTime logTime;


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

        public DateTime UPTIME
        {
            get { return upTime; }
            set { upTime = value; }
        }

        public DateTime LOGTIME
        {
            get { return logTime; }
            set { logTime = value; }
        }
    }
}
