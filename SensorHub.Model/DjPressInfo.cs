﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DjPressInfo
    {
        private long dbId;
        private string devId;
        private string pressData;
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

        public string PRESSDATA
        {
            get { return pressData; }
            set { pressData = value; }
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
