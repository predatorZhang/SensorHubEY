using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class AlarmRecordInfo
    {
        private long id;
        private string devCode;
        private string devType;
        private string recordCode;
        private string message;
        private string itemName;
        private string itemValue;
        private int deviceId;
        private DateTime recordDate;
        private int messageStatus;
        private bool active;

        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        public string DEVICE_CODE
        {
            get { return devCode; }
            set { devCode = value; }
        }
        public string DEVICE_TYPE_NAME
        {
            get { return devType; }
            set { devType = value; }
        }
        public string RECORDCODE
        {
            get { return recordCode; }
            set { recordCode = value; }
        }
        public string MESSAGE
        {
            get { return message; }
            set { message = value; }
        }
        public string ITEMNAME
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public string ITEMVALUE
        {
            get { return itemValue; }
            set { itemValue = value; }
        }
        public int DEVICE_ID
        {
            get { return deviceId; }
            set { deviceId = value; }
        }
        public DateTime RECORDDATE
        {
            get { return recordDate; }
            set { recordDate = value; }
        }
        public int MESSAGE_STATUS
        {
            get { return messageStatus; }
            set { messageStatus = value; }
        }
        public bool ACTIVE
        {
            get { return active; }
            set { active = value; }

        }
    }
}
