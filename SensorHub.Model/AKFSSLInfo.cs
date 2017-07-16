using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class AKFSSLInfo
    {
        private long dbId;
        private string devCode;
        private string openCir;
        private string jhResist;
        private string ryResist;
        private string currentDen;
        private string errosionRat;
        private DateTime logTime;
        private DateTime upTime;
        private string cell;
        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        
        public string JhResist
        {
            get { return jhResist; }
            set { jhResist = value; }
        }
        public string RyResist
        {
            get { return ryResist; }
            set { ryResist = value; }
        }
        public string CurrentDen
        {
            get { return currentDen; }
            set { currentDen = value; }
        }
        public string ErrosionRat
        {
            get { return errosionRat; }
            set { errosionRat = value; }
        }
        public string OpenCir
        {
            get { return openCir; }
            set { openCir = value; }
        }

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
