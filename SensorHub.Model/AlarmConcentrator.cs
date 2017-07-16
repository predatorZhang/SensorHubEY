using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    class AlarmConcentrator
    {
        private long dbId;
        private string concenCode;
        private string location;
        private DateTime lastTime;
        private int status;
        private int active;

        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }
        public string CONCENCODE
        {
            get { return concenCode; }
            set { concenCode = value; }
        }
        public string LOCATION
        {
            get { return location; }
            set { location = value; }
        }
        public DateTime LASTTIME
        {
            get { return lastTime; }
            set { lastTime = value; }
        }
        public int STATUS
        {
            get { return status; }
            set { status = value; }
        }
        public int ACTIVE
        {
            get { return active; }
            set { active = value; }
        }

    }
}
