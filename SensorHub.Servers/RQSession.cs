using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers
{
    public class RQSession : AppSession<RQSession, BinaryRequestInfo>
    {
        private string macID;
        private long id;
        private string productCompany;
        private string sensorType;
        private string ip;
        private string message;
        private float longitude;
        private float latitude;
        private float collectPeriod;
        private float uploadPeriod;
        private List<ChannelInfo> channelInfos;

        #region 属性设置
        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public float CollectPeriod
        {
            get { return collectPeriod; }
            set { collectPeriod = value; }
        }
        public float UploadPeriod
        {
            get { return uploadPeriod; }
            set { uploadPeriod = value; }
        }
        public List<ChannelInfo> ChannelInfos
        {
            get { return channelInfos; }
            set { channelInfos = value; }
        }
        public string Message
        {
            get {return message; }
            set { message = value; }
        }
        public string SensorType
        {
            get { return sensorType; }
            set { sensorType = value; }
        }
        public string ProductCompany
        {
            get { return productCompany; }
            set { productCompany = value; }
        }
        public string MacID
        {
            get { return macID; }
            set { macID = value; }
        }
        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        #endregion
    }

    public class ChannelInfo
    {
        private int channelNum;
        private float upper;
        private float lower;
        private bool isAlarmActive;

        public bool IsAlarmActive
        {
            get { return isAlarmActive; }
            set { isAlarmActive = value; }
        }

        public int ChannelNum
        {
            get { return channelNum; }
            set { channelNum = value; }
        }

        public float Upper
        {
            get { return upper; }
            set { upper = value; }
        }

        public float Lower
        {
            get { return lower; }
            set { lower = value; }
        }

        public ChannelInfo()
        { 

        }

        public ChannelInfo(int channelNumber, float upper, float lower,bool isAlarm)
        {
            this.channelNum = channelNumber;
            this.upper = upper;
            this.lower = lower;
            this.isAlarmActive = isAlarm;
        }
    }



}
