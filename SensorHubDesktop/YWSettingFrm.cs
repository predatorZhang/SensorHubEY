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
    public partial class YWSettingFrm : Office2007Form
    {
        private DeviceDTO dev;
        private DeviceService ds;
        private Dictionary<String, String> map = new Dictionary<String, String>();
        private const String PARAM_ITR = "yw_itr";
        private const String PARAM_CNT = "yw_cnt";
        private const String PARAM_REPT= "yw_rept";
        private const String PARAM_RESET = "yw_reset";
        private const String PARAM_HEIGHT = "yw_height";
        private const String PARAM_PERIOD = "yw_period";
          

        public YWSettingFrm(DeviceDTO dev,DeviceService ds)
        {
            InitializeComponent();
            this.dev = dev;
            this.ds = ds;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            map.Clear();
            String intr = this.txtIntr.Text;
            if (intr != "")
            {
                map.Add(PARAM_ITR, intr);
            }

            String cnt = this.txtCnt.Text;
            if (cnt != "")
            {
                map.Add(PARAM_CNT, cnt);
            }

            String rept = this.txtRep.Text;
            if (rept != "")
            {
                map.Add(PARAM_REPT, rept);
            }

            String rst = this.txtRst.Text;
            if (rst != "")
            {
                map.Add(PARAM_RESET, rst);
            }

            String height = this.txtHeight.Text;
            if (height != "")
            {
                map.Add(PARAM_HEIGHT, height);
            }

            String per = this.txtPer.Text;
            if (per != "")
            {
                map.Add(PARAM_PERIOD, per);
            }

           // ds.wakeUpCasicDev(dev);

            bool isSuccess = ds.sendDeviceInfo(dev, map);
            if (!isSuccess)
            {
                MessageBox.Show("设备未唤醒，请重新下发");
            }
        }

        private void YWSettingFrm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text +":"+ this.dev.Name;
        }
    }
}
