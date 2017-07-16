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
    public partial class YLSettingFrm : Office2007Form
    {
        private DeviceDTO dev;
        private DeviceService ds;
        private Dictionary<String, String> map = new Dictionary<String, String>();
        private const String PARAM_DEVCODE = "yw_deviceId";

        public YLSettingFrm(DeviceDTO dev,DeviceService ds)
        {
            InitializeComponent();
            this.dev = dev;
            this.ds = ds;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.txtDevCode.Text = this.dev.Name;
            this.Text = this.Text + ":"+this.dev.Name;
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            map.Clear();
            String devCode = this.txtDevCode.Text;
            if (devCode != "")
            {
                map.Add(PARAM_DEVCODE, devCode);
            }
            ds.sendDeviceInfo(dev, map);
        }
    }
}
