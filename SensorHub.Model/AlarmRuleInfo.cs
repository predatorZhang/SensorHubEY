using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class AlarmRuleInfo
    {

        private long dbid;
        private float highValue;
        private float lowValue;
        private float overtime;
        private float saltation;
        private string sensorCode;
        private int deviceId;

        public long Dbid
        {
            get { return dbid; }
            set { dbid = value; }
        }

        public float HighValue
        {
            get { return highValue; }
            set { highValue = value; }
        }

        public float LowValue
        {
            get { return lowValue; }
            set { lowValue = value; }
        }

        public float Overtime
        {
            get { return overtime; }
            set { overtime = value; }
        }

        public float Saltation
        {
            get { return saltation; }
            set { saltation = value; }
        }

        public string SensorCode
        {
            get { return sensorCode; }
            set { sensorCode = value; }
        }

        public int DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }        
        
    }
}
