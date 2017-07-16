using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class Equipment
    {
        private long dbId;
        private string macId;
        private string status;
        private string owner;
        private string description;

        public long DbId
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string MacId
        {
            get { return macId; }
            set { macId = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


    }
}
