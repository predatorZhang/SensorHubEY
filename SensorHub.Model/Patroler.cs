using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class Patroler
    {
        private long dbId;
        private string accountState;
        private string age;
        private string password;        
        private string phoneNum;        
        private string sex;        
        private string userName;        
        private long region_id;        

        public long DbId
        {
            get { return dbId; }
            set { dbId = value; }
        }

        public string AccountState
        {
            get { return accountState; }
            set { accountState = value; }
        }

        public string Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string PhoneNum
        {
            get { return phoneNum; }
            set { phoneNum = value; }
        }

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public long Region_id
        {
            get { return region_id; }
            set { region_id = value; }
        }
    }
}
