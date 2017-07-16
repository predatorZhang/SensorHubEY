
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class TaskInfo
    {
        private long dbId;     
        private DateTime deployTime;        
        private string description;        
        private DateTime finishTime;        
        private string street;        
        private string taskState;        
        private string userName;        
        private long patroler_id;       

        public long DbId
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public DateTime DeployTime
        {
            get { return deployTime; }
            set { deployTime = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; }
        }

        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        public string TaskState
        {
            get { return taskState; }
            set { taskState = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public long Patroler_id
        {
            get { return patroler_id; }
            set { patroler_id = value; }
        }
    }
}
