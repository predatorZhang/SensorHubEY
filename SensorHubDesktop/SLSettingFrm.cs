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
    public partial class SLSettingFrm : Office2007Form
    {
        private DeviceDTO dev;
        private DeviceService ds;
        private Dictionary<String, String> map = new Dictionary<String, String>();
        private const String PARAM_STIME = "sl_stime";
        private const String PARAM_ITRL = "sl_itrl";
        private const String PARAM_CNT = "sl_cnt"; 
        private const String PARAM_REPT = "sl_rept";
          
        public SLSettingFrm(DeviceDTO dev,DeviceService ds)
        {
            InitializeComponent();
            this.dev = dev;
            this.ds = ds;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            map.Clear();

            String stime = this.txtColTime.Text;
            if (stime != "")
            {
                map.Add(PARAM_STIME, stime);
            }

            String itr = this.txtIntr.Text;
            if (itr != "")
            {
                map.Add(PARAM_ITRL, itr);
            }

            String cnt = this.txtCnt.Text;
            if (cnt != "")
            {
                map.Add(PARAM_CNT, cnt);
            }

            String rept = this.txtRept.Text;
            if (rept != "")
            {
                map.Add(PARAM_REPT, rept);
            }

           // ds.wakeUpCasicDev(dev);

           bool isSuccess =  ds.sendDeviceInfo(dev, map);
           if (!isSuccess)
           {
               MessageBox.Show("设备未唤醒，请重新下发");
           }

        }

        private void SLSettingFrm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + ":"+this.dev.Name;
        }
    }
}
