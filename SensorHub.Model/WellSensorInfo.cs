using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class WellSensorInfo
    {
        private long dbId;
        private string devId;
        private string cell;
        private string status;
        private string descn;        
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
        public string DESCN
        {
            get { return descn; }
            set { descn = value; }
        }
        public string CELL
        {
            get { return cell; }
            set { cell = value; }
        }
        public string STATUS
        {
            get { return status; }
            set { status = value; }
        }
        public DateTime LOGTIME
        {
            get { return logTime; }
            set { logTime = value; }
        }
    }
}
