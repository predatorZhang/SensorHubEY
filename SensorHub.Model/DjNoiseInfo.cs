using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DjNoiseInfo
    {
        private long dbId;
        private string devId;
        private string dData;
        private string dBegin;
        private string dInterval;
        private string dCount;
        private string warelessOpen;
        private string warelessClose;
        private string cell;
        private string signal;
        private string status;        
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
        public string DDATA
        {
            get { return dData; }
            set { dData = value; }
        }
        public string DBEGIN
        {
            get { return dBegin; }
            set { dBegin = value; }
        }
        public string DINTERVAL
        {
            get { return dInterval; }
            set { dInterval = value; }
        }
        public string DCOUNT
        {
            get { return dCount; }
            set { dCount = value; }
        }
        public string WARELESSOPEN
        {
            get { return warelessOpen; }
            set { warelessOpen = value; }
        }
        public string WARELESSCLOSE
        {
            get { return warelessClose; }
            set { warelessClose = value; }
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
    }
}
