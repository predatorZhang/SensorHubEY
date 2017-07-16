using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DjFlowInfo
    {
        private long dbId;
        //设备ID
        private string devId;
        private string insData;
        private string netData;
        private string posData;
        private string negData;
        private string signal;
        private string cell;
        private string status;
        private DateTime upTime;
        private DateTime logTime;

        //数据库主键
        public long DBID
        {
            get { return dbId; }
            set { dbId = value; }
        }

        //设备ID
        public string DEVID
        {
            get { return devId; }
            set { devId = value; }
        }

        //瞬时流量
        public string INSDATA
        {
            get { return insData; }
            set { insData = value; }
        }

        //净累计流量
        public string NETDATA
        {
            get { return netData; }
            set { netData = value; }
        }

        //正累计流量
        public string POSDATA
        {
            get { return posData; }
            set { posData = value; }
        }

        //负累计流量
        public string NEGDATA
        {
            get { return negData; }
            set { negData = value; }
        }

        public string SIGNAL
        {
            get { return signal; }
            set { signal = value; }
        }

        //流量计电量
        public string CELL
        {
            get { return cell; }
            set { cell = value; }
        }

        public string STATUS
        {
            get { return status; }
            set { status = value; }
        }

        //数据采集时间
        public DateTime UPTIME
        {
            get { return upTime; }
            set { upTime = value; }
        }

        //数据记录时间
        public DateTime LOGTIME
        {
            get { return logTime; }
            set { logTime = value; }
        }
    }
}
