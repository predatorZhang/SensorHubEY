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
    public class CasicSession : AppSession<CasicSession, StringRequestInfo>
    {

        protected override void OnSessionClosed(CloseReason reason){
            try
            {
                Model.DeviceLogInfo1 log = new Model.DeviceLogInfo1();
                log.DEVICECODE = this.hubAddr;
                log.DEVTYPE = "数据集中器";
                log.OPERATETYPE = "下线";
                log.LOGTIME = DateTime.Now;
                log.MESSAGE = "下线";
                new BLL.DeviceLog1().insert(log);
                //TODO LIST：待实现
                new BLL.AlarmConcentrator().setHubOffLine(this.hubAddr);
            }
            catch (Exception e) {
                this.Logger.Error(e.Message);
            }
        }
        
        /*
        private String hubAddr;
        private List<String> devCodes;

        public String HubAddr
        {
            get { return hubAddr; }
            set { hubAddr = value; }
        }

        public List<String> DevCodes
        {
            get { return devCodes; }
            set { devCodes = value; }
        }
         * */

        /**
         * key:设备ID
         * value：上传数值
         * **/

        private String hubAddr;
        public String HubAddr
        {
            get { return hubAddr; }
            set { hubAddr = value; }
        }

        public Dictionary<string, DeviceDTO> devMaps = new Dictionary<string, DeviceDTO>();


        public class DeviceDTO
        {
            private String devCode;
            private String detail;
            private bool isWakeup;
            private String company;
            private String devType;
            private String status;
            private UInt16 seq;

            public UInt16 Seq
            {
                get { return seq; }
                set { seq = value; }
            }

            public String DevCode
            {
                get { return devCode; }
                set { devCode = value; }
            }

            public String Detail
            {
                get { return detail; }
                set { detail = value; }
            }

            public bool IsWakeUp
            {
                get { return isWakeup; }
                set { isWakeup = value; }
            }


            public String Company
            {
                get { return company; }
                set { company = value; }
            }


            public String DevType
            {
                get { return devType; }
                set { devType = value; }
            }

            public String Status
            {
                get { return status; }
                set { status = value; }
            }
        }
    }
}
