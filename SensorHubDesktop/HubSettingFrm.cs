using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace SensorHubDesktop
{
    public partial class HubSettingFrm : Office2007Form
    {
        private DeviceDTO dev;
        private DeviceService ds;
        private Dictionary<String,String> map = new Dictionary<String,String>();
        private const String PARAM_IP = "hub_ip";
        private const String PARAM_PORT = "hub_port";

        public HubSettingFrm(DeviceDTO dev,DeviceService ds)
        {
            InitializeComponent();
            this.dev = dev;
            this.ds = ds;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            map.Clear();
            String ip = this.txtIp.Text;
            if (ip != "")
            {
                map.Add(PARAM_IP, ip);
            }

            String port = this.txtPort.Text;
            if (ip != "")
            {
                map.Add(PARAM_PORT, port);
            }

            ds.sendDeviceInfo(dev, map);
        }

        private void HubSettingFrm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + ":"+this.dev.Name;
        }
    }
}
