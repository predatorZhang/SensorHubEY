using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;

namespace SensorHub.Servers
{
    public class SZLiquidSession : AppSession<SZLiquidSession, StringRequestInfo>
    {
        private string macID;
        private string productCompany;
        private string sensorType;
        private uint batch = 0;
    
        private DeviceConfigInfo liquidConf;

        public DeviceConfigInfo LiquidConf
        {
            get { return liquidConf; }
            set { liquidConf = value; }
        }

        private DeviceConfigInfo noiseConf;
        public DeviceConfigInfo NoiseConf
        {
            get { return noiseConf; }
            set { noiseConf = value; }
        }

        private DeviceConfigInfo pressConf;
        public DeviceConfigInfo PressConf
        {
            get { return pressConf; }
            set { pressConf = value; }
        }

        private DeviceConfigInfo flowConf;
        public DeviceConfigInfo FlowConf
        {
            get { return flowConf; }
            set { flowConf = value; }
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

        public uint Batch
        {
            get { return batch; }
            set { batch = value; }
        }
    }
}
