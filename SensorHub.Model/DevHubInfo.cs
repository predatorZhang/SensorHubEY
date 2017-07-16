using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class DevHubInfo
    {
        private string devCode;
        public string DevCode
        {
            get { return devCode; }
            set { devCode = value; }
        }

        private string hubCode;
        public string HubCode
        {
            get { return hubCode; }
            set { hubCode = value; }
        }

    }
}
