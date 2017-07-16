using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class RqGasInfo
    {
        private long dbId;
        private string devId;
        private string leakData;
        private string inPress;
        private string outPress;
        private string tempGas;
        private string tempRoom;
        private string cellPower;
        private string recordTime;

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

        public string LEAKDATA
        {
            get { return leakData; }
            set { leakData = value; }
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

        public string TEMPGAS
        {
            get { return tempGas; }
            set { tempGas = value; }
        }

        public string TEMPROOM
        {
            get { return tempRoom; }
            set { tempRoom = value; }
        }

        public string CELLPOWER
        {
            get { return cellPower; }
            set { cellPower = value; }
        }

        public string RECORDTIME
        {
            get { return recordTime; }
            set { recordTime = value; }
        }
    }
}
