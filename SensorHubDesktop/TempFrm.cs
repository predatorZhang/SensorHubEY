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
    public partial class TempFrm : Office2007Form
    {
        private DeviceDTO dev;
        private DeviceService ds;
        private Dictionary<String, String> map = new Dictionary<String, String>();
        private const String PARAM_STIME = "temp_stime";
        private const String PARAM_PERIOD = "temp_period";

        public TempFrm()
        {
            InitializeComponent();
        }

        public TempFrm(DeviceDTO dev, DeviceService ds)
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
                map.Add(PARAM_STIME, stime);
            }

            String period = this.txtHibernate.Text;
            if (period != "")
            {
                map.Add(PARAM_PERIOD, period);
            }

            // ds.wakeUpCasicDev(dev);

            bool isSuccess = ds.sendDeviceInfo(dev, map);
            if (!isSuccess)
            {
                MessageBox.Show("设备未唤醒，请重新下发");
            }
        }

        private void TempFrm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + this.dev.Name;
        }
    }
}
