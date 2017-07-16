using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class SlNoiseInfo
    {
        private long dbId;
        private string srcId;
        private string dstId;
        private string denseData;
        private string cell;
        private string signal;
        private string status;    
        private DateTime upTime;
        private DateTime logTime;

        private string frequency;

        public long DBID
        {
            get { return dbId;}
            set { dbId = value; }
        }

        public string SRCID
        {
            get { return srcId; }
            set { srcId = value; }
        }

        public string DSTID
        {
            get { return dstId; }
            set { dstId = value; }
        }

        public string DENSEDATA
        {
            get { return denseData; }
            set { denseData = value; }
        }

        public string CELL
        {
            get { return cell; }
            set { cell = value; }
        }

        public string SIGNAL
        {
            get { return signal; }
            set { signal = value; }
        }

        public string STATUS
        {
            get { return status; }
            set { status = value; }
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

        public String FREQUENCY
        {
            get { return frequency; }
            set { frequency = value; }
        }
    }
}
