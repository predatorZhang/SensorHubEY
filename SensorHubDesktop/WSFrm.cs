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
    public partial class WSFrm : Office2007Form
    {
        private DeviceDTO dev;
        private DeviceService ds;
        private Dictionary<String, String> map = new Dictionary<String, String>();
        private const String PARAM_PERIOD = "ws_period";

        public WSFrm()
        {
            InitializeComponent();
        }


        public WSFrm(DeviceDTO dev, DeviceService ds)
        {
            InitializeComponent();
            this.dev = dev;
            this.ds = ds;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            map.Clear();

            String stime = this.txtTime.Text;
            if (stime != "")
            {
                map.Add(PARAM_PERIOD, stime);
            }
            else
            {
                MessageBox.Show("有害气体配置周期不能为空:");
                return;
            }


            bool isSuccess = ds.sendDeviceInfo(dev, map);
            if (isSuccess)
            {
                MessageBox.Show("有害气体配置下发成功");
            }
        }

        private void WSFrm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + this.dev.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ds.calibraRQ(dev);
        }
    }
}
