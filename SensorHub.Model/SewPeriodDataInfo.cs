using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class SewPeriodDataInfo
    {
        private long dbId;
        private string devId;
        private DateTime upTime;
        private string co;
        private string o2;
        private string h2s;
        private string fireGas;

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

        public DateTime UPTIME
        {
            get { return upTime; }
            set { upTime = value; }
        }

        public string CO
        {
            get { return co; }
            set { co = value; }
        }

        public string O2
        {
            get { return o2; }
            set { o2 = value; }
        }

        public string H2S
        {
            get { return h2s; }
            set { h2s = value; }
        }

        public string FIREGAS
        {
            get { return fireGas; }
            set { fireGas = value; }
        }
    }
}
