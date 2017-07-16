using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SensorHub.OracleDAL;
using System.Data.OracleClient;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;

namespace SensorHubDesktop
{
    public partial class MainFrm : Form
    {
        //int dbidIndex = 0;
        public DeviceService ds;
        public MainFrm()
        {
            InitializeComponent();

            winGridViewPager1.OnPageChanged += new EventHandler(winGridViewPager1_OnPageChanged);
            //winGridViewPager1.OnEditSelected += new EventHandler(winGridViewPager1_OnEditSelected);
            winGridViewPager1.OnRefresh += new EventHandler(winGridViewPager1_OnRefresh);

            winGridViewPager1.BackColor = Color.LightCyan;//间隔颜色
            winGridViewPager1.PagerInfo.PageSize = maxPageIndex;
           

        }
        int maxPageIndex = 20;

        public void UpdateDeviceListFunc()
        {
            /*
            string sql = "select * from ALARM_DEVICE";

            DataTable table = OracleHelper.ExecuteDataset(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql).Tables[0];
            winGridViewPager1.PagerInfo.RecordCount = table.Rows.Count;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string factory = table.Rows[i].ItemArray[5].ToString();
                if (!comboBoxFactory.Items.Contains(factory))
                {
                    comboBoxFactory.Items.Add(factory);
                }
            }

            string devSql = "select * from ALARM_DEVICE_TYPE";
            DataTable dt = OracleHelper.ExecuteDataset(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, devSql).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string typename = dt.Rows[i].ItemArray[4].ToString();
                if (!comboBoxdevType.Items.Contains(typename))
                {
                    comboBoxdevType.Items.Add(typename);
                }
            }
             * */
        }

        /// <summary>
        /// add by yan
        /// </summary>
        /// <param name="sql"></param>
        public void UpdateDeviceListFunc(string sql)
        {
            /*
            DataTable table = OracleHelper.ExecuteDataset(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql).Tables[0];
            int i = table.Rows.Count;
            winGridViewPager1.DataSource = table.DefaultView;
             * */
        }

        private void Form2_Load(object  sender, EventArgs e)
        {

            if (StartServer() == false)
            {
                MessageBox.Show("数据采集服务未启动！");
                this.Close();
            }
            this.timer1.Enabled = true;

         
   
            initlizeDatagrid();
            /*
            IList<DeviceDTO> devs = new List<DeviceDTO>();
            for (int i = 0; i < 20; i++)
            {
                DeviceDTO de = new DeviceDTO();
                de.TypeName = "渗漏预警仪";
                de.SessionId = "11";
                devs.Add(de);
            }

            for (int j = 0; j < 20; j++)
            {
                DeviceDTO yw = new DeviceDTO();
                yw.TypeName = "液位监测仪";
                yw.SessionId = "12";
                devs.Add(yw);
            }

            for (int m = 0; m < 20; m++)
            {
                DeviceDTO jzq = new DeviceDTO();
                jzq.TypeName = "集中器";
                jzq.SessionId = "13";
                devs.Add(jzq);
            }

            for (int n = 0; n < 20; n++)
            {
                DeviceDTO yl = new DeviceDTO();
                yl.TypeName = "雨量计";
                devs.Add(yl);
            }
            DataTable dt = DeviceDTO.ConvertToTables(devs);
            winGridViewPager1.DataSource = dt.DefaultView;
             * */

            /*
            DataGridViewButtonColumn Column1 = new DataGridViewButtonColumn();
            Column1.Name = "shit";
            Column1.Text = "peizhi";
            Column1.UseColumnTextForButtonValue = true;
            Column1.HeaderText = "shit2";
            this.winGridViewPager1.dataGridView1.Columns.Add(Column1);
             * */
        }

        private void initlizeDatagrid()
        {

            this.winGridViewPager1.Dock = DockStyle.Fill;
            this.winGridViewPager1.dataGridView1.Dock = DockStyle.Fill;
            this.winGridViewPager1.AddColumnAlias("Name", "设备名称");
            this.winGridViewPager1.AddColumnAlias("Company", "厂商");
            this.winGridViewPager1.AddColumnAlias("TypeName", "设备类型");
            this.winGridViewPager1.AddColumnAlias("Status", "当前状态");
            this.winGridViewPager1.DisplayColumns = "Name,TypeName,Company,Status";

            winGridViewPager1.OnEditSelected += new EventHandler(winGridViewPager1_OnEditSelected);

            winGridViewPager1.BackColor = Color.LightCyan;//间隔颜色

            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[1].Visible = false;
            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[3].Visible = false;
            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[4].Visible = false;
            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[5].Visible = false;
            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[6].Visible = false;
            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[7].Visible = false;
            winGridViewPager1.dataGridView1.ContextMenuStrip.Items[2].Text = "修改配置";
            this.winGridViewPager1.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void winGridViewPager1_OnRefresh(object sender, EventArgs e)
        {
           // BindData();
        }

        private void winGridViewPager1_OnPageChanged(object sender, EventArgs e)
        {
           // BindData();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            /*
            string sql = "select DBID,DEVNAME,DEVCODE,FACTORY,TURNX,(select typename from ALARM_DEVICE_TYPE where dbid = devicetype_id) from ALARM_DEVICE where ACTIVE = 1";
            if (comboBoxFactory.Text.ToString() != "")
            {
                sql += " and factory = '" + comboBoxFactory.Text.ToString() + "'";
            }
            if (comboBoxdevType.Text.ToString() != "")
            {
                sql += " and devicetype_id = (select dbid from ALARM_DEVICE_TYPE where typename = '" + comboBoxdevType.Text.ToString() + "')";
            }
            if (textBoxdevCode.Text.ToString() != "")
            {
                sql += " and devcode like '%" + textBoxdevCode.Text.ToString() + "%'";
            }
            UpdateDeviceListFunc(sql);
             * */
           
            /*
            IList<DeviceDTO> devs = new List<DeviceDTO>();
            for (int i = 0; i < 20; i++)
            {
                DeviceDTO de = new DeviceDTO();
                de.TypeName = "渗漏预警仪";
                de.SessionId = "11";
                devs.Add(de);
            }

            for (int j = 0; j < 20; j++)
            {
                DeviceDTO yw = new DeviceDTO();
                yw.TypeName = "液位监测仪";
                yw.SessionId = "12";
                devs.Add(yw);
            }

            for (int m = 0; m < 20; m++)
            {
                DeviceDTO jzq = new DeviceDTO();
                jzq.TypeName = "集中器";
                jzq.SessionId = "13";
                devs.Add(jzq);
            }

            for (int n = 0; n < 20; n++)
            {
                DeviceDTO yl = new DeviceDTO();
                yl.TypeName = "雨量计";
                devs.Add(yl);
            }
            DataTable dt = DeviceDTO.ConvertToTables(devs);
            winGridViewPager1.DataSource = dt.DefaultView;
             * */
        }

        //为了使分页控件能够显示总数，并记住当前的分页，那么在OnPageChanged实现中需要修改分页控件的 RecordCount和CurrenetPageIndex这两个属性。
        private void BindData()
        {/*
            #region 添加别名解析
            this.winGridViewPager1.AddColumnAlias("DBID", "编号");
            this.winGridViewPager1.AddColumnAlias("DEVCODE", "设备编号");
            this.winGridViewPager1.AddColumnAlias("DEVNAME", "设备名称");
            this.winGridViewPager1.AddColumnAlias("FACTORY", "所属道路");
            this.winGridViewPager1.AddColumnAlias("TURNX", "监测对象");
            this.winGridViewPager1.AddColumnAlias("(SELECTTYPENAMEFROMALARM_DEVICE_TYPEWHEREDBID=DEVICETYPE_ID)", "设备类型");
            #endregion

            this.winGridViewPager1.DisplayColumns = "DBID,DEVCODE,(SELECTTYPENAMEFROMALARM_DEVICE_TYPEWHEREDBID=DEVICETYPE_ID),DEVNAME,FACTORY,BTNSEARCH,TURNX";

            string where = GetSearchSql();

            UpdateDeviceListFunc(where);

            winGridViewPager1.dataGridView1.Refresh();
          * */

        }

        /*
        private string GetSearchSql()
        {
           
            int pageIndex = winGridViewPager1.PagerInfo.CurrenetPageIndex;

            int dataCount = maxPageIndex;

            string sql = "";

            sql = "select DBID,DEVNAME,DEVCODE,FACTORY,TURNX,(select typename from ALARM_DEVICE_TYPE where dbid = devicetype_id) from ( select t.*,rownum row_num from ALARM_DEVICE t order by t.dbid) b where ( b.row_num between " + maxPageIndex * (pageIndex - 1) + " and " + maxPageIndex * pageIndex+" ) and active = 1";

            return sql;
             
           
        }
         * */

        #region 
        private void buttonOK_Click(object sender, EventArgs e)
        {
           // int zhangfan = 0;
            //string active = "";

            //if (comboBoxactive.Text.ToString() == "在线")
            //{
            //    active = "1";
            //}
            //else
            //{
            //    active = "0";
            //}

            //string sql = "update alarm_device set active = " + active + "where dbid =" + dbidIndex;
            //try
            //{
            //    OracleHelper.ExecuteNonQuery(OracleHelper.ConnectionStringOrderDistributedTransaction, CommandType.Text, sql);
            //    MessageBox.Show("配置下发成功！", "提示");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("error", "message");
            //}

        }

        private void winGridViewPager1_OnEditSelected(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                 string name = grid.SelectedRows[0].Cells["Name"].Value.ToString();
                 string company = grid.SelectedRows[0].Cells["Company"].Value.ToString();
                 string typeName = grid.SelectedRows[0].Cells["TypeName"].Value.ToString();
                 string tag = grid.SelectedRows[0].Cells["Tag"].Value.ToString();
                 string sessionId = grid.SelectedRows[0].Cells["SessionId"].Value.ToString();
                 string status = grid.SelectedRows[0].Cells["Status"].Value.ToString();
                 string serverName = grid.SelectedRows[0].Cells["ServerName"].Value.ToString();
                 DeviceDTO dto = new DeviceDTO();
                 dto.Name = name;
                 dto.Company = company;
                 dto.TypeName = typeName;
                 dto.Tag = tag;
                 dto.SessionId = sessionId;
                 //dto.Status = status;
                 dto.ServerName = serverName;

                 switch (dto.TypeName)
                {
                    case "渗漏预警仪":
                        SLSettingFrm f1 = new SLSettingFrm(dto,ds);
                        f1.Show();
                        break;
                    case "液位监测仪":
                        YWSettingFrm f2 = new YWSettingFrm(dto,ds);
                        f2.Show();
                        break;
                    case "集中器":
                        HubSettingFrm f3 = new HubSettingFrm(dto,ds);
                        f3.Show();
                        break;
                    case "雨量计":
                        YLSettingFrm f4 = new YLSettingFrm(dto,ds);
                        f4.Show();
                        break;
                    case "有害气体监测仪":
                        WSFrm f5= new WSFrm(dto, ds);
                        f5.Show();
                        break;
                    case "温度压力监测仪":
                        TempFrm f6 = new TempFrm(dto, ds);
                        f6.Show();
                        break;
                    case "远传水表":
                        WaterMeterFrm f7 = new WaterMeterFrm(dto, ds);
                        f7.Show();
                        break;
                     default:
                        break;

                }
            }
        }

        #endregion


        /**
         * 启动server服务器实例
         * **/
        private IBootstrap bootstrap;
        private bool StartServer()
        {
            try 
            {
                bootstrap = BootstrapFactory.CreateBootstrap();
                if (!bootstrap.Initialize())
                {
                    return false;
                }
                var result = bootstrap.Start();
                if (result == StartResult.Failed)
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("采集服务启动失败");
                return false;

            }
            ds = new DeviceService(bootstrap);
            return true;
        }

        /**
         * 关闭server服务器实例
         * **/
        private void StopServer()
        {
            if (bootstrap != null)
            {
                bootstrap.Stop();
            }

        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopServer();
            this.timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        
            List<DeviceDTO> devices= ds.getAllOnlineDev();
            if (devices.Count == 0)
            {
                return;
            }

            DataTable dt = DeviceDTO.ConvertToTables(devices);
            winGridViewPager1.DataSource = devices;
            winGridViewPager1.dataGridView1.Refresh();

    
        }

    }
}
