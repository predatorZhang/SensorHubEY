
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class Position
    {
        private long dbId;
        private double latitude;
        private double longitude;
        private System.DateTime receiveTime;
        private string username;
        private long patroler_id;
        private long task_id;

        public long DbId
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public System.DateTime ReceiveTime
        {
            get { return receiveTime; }
            set { receiveTime = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public long Patroler_id
        {
            get { return patroler_id; }
            set { patroler_id = value; }
        }

        public long Task_id
        {
            get { return task_id; }
            set { task_id = value; }
        }
    }
}
