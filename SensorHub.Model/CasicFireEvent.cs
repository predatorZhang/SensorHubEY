using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class CasicFireEvent
    {
        private string cmd;
        private string status;
        private string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }


    }
}
