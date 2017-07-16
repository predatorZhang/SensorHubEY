using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class CasicPress
    {
        private long dbId;
        private string devCode;
        private string data;
        private DateTime logTime;
        private DateTime upTime;
        private string cell;

        public string Cell
        {
            get { return cell; }
            set { cell = value; }
        }


        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        public string DEVCODE
        {
            get { return devCode; }
            set { devCode = value; }
        }

        public DateTime LogTime
        {
            get { return logTime; }
            set { logTime = value; }
        }

        public DateTime UpTime
        {
            get { return upTime; }
            set { upTime = value; }
        }
    }
}
