using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class AKFSHJInfo
    {
        private long dbId;
        private string devCode;

        private string underTemp1;
        private string underTemp2;

        private string outterTemp1;
        private string outterTemp2;

        private string underVo11;
        private string underVo12;
        private string underVo13;
        private string underVo14;
        private string underVo15;
        private string underVo16;

        private string underWaterIn1;
        private string underWaterIn2;
        private string underWaterIn3;
        private string underWaterIn4;
        private string underWaterIn5;
        private string underWaterIn6;

        private DateTime logTime;
        private DateTime upTime;
        private string cell;
        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
       
        public string UnderTemp1
        {
            get { return underTemp1; }
            set { underTemp1 = value; }
        }
        
        public string UnderTemp2
        {
            get { return underTemp2; }
            set { underTemp2 = value; }
        }
        
        public string OutterTemp1
        {
            get { return outterTemp1; }
            set { outterTemp1 = value; }
        }
        
        public string OutterTemp2
        {
            get { return outterTemp2; }
            set { outterTemp2 = value; }
        }
        
        public string UnderVo11
        {
            get { return underVo11; }
            set { underVo11 = value; }
        }
        
        public string UnderVo12
        {
            get { return underVo12; }
            set { underVo12 = value; }
        }
        
        public string UnderVo13
        {
            get { return underVo13; }
            set { underVo13 = value; }
        }
        
        public string UnderVo14
        {
            get { return underVo14; }
            set { underVo14 = value; }
        }
        
        public string UnderVo15
        {
            get { return underVo15; }
            set { underVo15 = value; }
        }
        
        public string UnderVo16
        {
            get { return underVo16; }
            set { underVo16 = value; }
        }
        
        public string UnderWaterIn1
        {
            get { return underWaterIn1; }
            set { underWaterIn1 = value; }
        }
        
        public string UnderWaterIn2
        {
            get { return underWaterIn2; }
            set { underWaterIn2 = value; }
        }
        
        public string UnderWaterIn3
        {
            get { return underWaterIn3; }
            set { underWaterIn3 = value; }
        }
        
        public string UnderWaterIn4
        {
            get { return underWaterIn4; }
            set { underWaterIn4 = value; }
        }
        
        public string UnderWaterIn5
        {
            get { return underWaterIn5; }
            set { underWaterIn5 = value; }
        }
        
        public string UnderWaterIn6
        {
            get { return underWaterIn6; }
            set { underWaterIn6 = value; }
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
