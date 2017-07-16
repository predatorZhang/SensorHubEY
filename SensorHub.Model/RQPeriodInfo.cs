using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class RQPeriodInfo
    {
        private long dbId;
        private string address;
        private string inPress;
        private string outPress;
        private string flow;
        private string strength;
        private string temperature;
        private string cell;
        private DateTime upTime;
        private DateTime logTime;

        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string ADDRESS
        {
            get { return address; }
            set { address = value; }
        }

        public string INPRESS
        {
            get { return inPress; }
            set { inPress = value; }
        }
        public string OUTPRESS
        {
            get { return outPress; }
            set { outPress = value; }
        }
        public string FLOW
        {
            get { return flow; }
            set { flow = value; }
        }
        public string STRENGTH
        {
            get { return strength; }
            set { strength = value; }
        }
        public string TEMPERATURE
        {
            get { return temperature; }
            set { temperature = value; }
        }
        public string CELL
        {
            get { return cell; }
            set { cell = value; }
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
