using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class YLInfo
    {
        private long dbId;
        private string srcId;//源地址
        private string dstId;//接收端地址
        private string curMinuteYuLiang;//当前一分钟雨量
        private string forMinuteYuLiang;//前一分钟雨量
        private string totalYuLiang;//总量
        private DateTime upTime;
        private DateTime logTime;
        private Double yLiangValue;//十分钟累计的雨量

        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string DstId
        {
            get { return dstId; }
            set { dstId = value; }
        }

        public string SrcId
        {
            get { return srcId; }
            set { srcId = value; }
        }

        public string CurMinuteYuLiang
        {
            get { return curMinuteYuLiang; }
            set { curMinuteYuLiang = value; }
        }

        public string ForMinuteYuLiang
        {
            get { return forMinuteYuLiang; }
            set { forMinuteYuLiang = value; }
        }

        public string TotalYuLiang
        {
            get { return totalYuLiang; }
            set { totalYuLiang = value; }
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
        public Double YLiangValue
        {
            get { return yLiangValue; }
            set { yLiangValue = value; }
        }

    }
}
