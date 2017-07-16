using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
using System.Timers;
using SensorHub.BLL;
using SensorHub.Utility;
using Quartz.Impl;
using Quartz;
using System.Threading;
namespace SensorHub.Servers
{
    /*
     * 航天二院最新服务器协议
     */
    public class CasicServer : AppServer<CasicSession>
    {
    
        public CasicServer()
            : base(new DefaultReceiveFilterFactory<CasicReceiveFilter, StringRequestInfo>())
        {

        }

        private void updateSequence()
        {
            try
            {
                ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_LIQUID", "DBID", "AD_DJ_LIQUID");
                ApplicationContext.getInstance().updateSequence("SEQ_AD_SL_NOISE_ID", "DBID", "AD_SL_NOISE");
                ApplicationContext.getInstance().updateSequence("SEQ_ALARM_PRESS_ID", "DBID", "ALARM_PRESS");
                ApplicationContext.getInstance().updateSequence("SEQ_ALARM_TEMPERATURE_ID", "DBID", "ALARM_TEMPERATURE");
                ApplicationContext.getInstance().updateSequence("SEQ_ALARM_WATERQUANTITY_ID", "DBID", "ALARM_WATERQUANTITY");
                ApplicationContext.getInstance().updateSequence("SEQ_CASIC_WELL_INFO_ID", "DBID", "CASIC_WELL_INFO");
                ApplicationContext.getInstance().updateSequence("SEQ_ALARM_RECORD_ID", "DBID", "ALARM_ALARM_RECORD");
                ApplicationContext.getInstance().updateSequence("SEQ_ALARM_DEVICE_LOG1_ID", "DBID", "ALARM_DEVICE_LOG1");
                
                ApplicationContext.getInstance().updateSequence("SEQ_XT_RQ_PERIOD", "DBID", "XT_RQ_PERIOD");
                ApplicationContext.getInstance().updateSequence("SEQ_AD_DJ_FLOW_ID", "DBID", "AD_DJ_FLOW");
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message);
            }
        }

        private void SchedualFsWakeUp()
        {
            ISchedulerFactory sf = null;
            try
            {
                sf = new StdSchedulerFactory();
            }
            catch (Exception e)
            {
               this.Logger.Error(e.Message);
            }

            IScheduler sched = sf.GetScheduler();
            JobDataMap map = new JobDataMap();
            map.Put("server", this);
            IJobDetail job = JobBuilder.Create<CasicWakeUpJob>()
                .WithIdentity("job4", "group1")
                .SetJobData(map)
                .Build();
            String jobCronSetting = System.Configuration.ConfigurationSettings.AppSettings["WAKE_UP_JOB"];
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                         .WithIdentity("trigger4", "group1")
                                         .WithCronSchedule(jobCronSetting)
                                         .Build();
            DateTimeOffset ft = sched.ScheduleJob(job, trigger);
            sched.Start();
        }

        private void SchedualSlGetData()
        {
            ISchedulerFactory sf = null;
            try
            {
                sf = new StdSchedulerFactory();
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message);
            }

            IScheduler sched = sf.GetScheduler();
            JobDataMap map = new JobDataMap();
            map.Put("server", this);
            IJobDetail job = JobBuilder.Create<CasicGetDataJob>()
                .WithIdentity("job5", "group1")
                .SetJobData(map)
                .Build();
            String jobCronSetting = System.Configuration.ConfigurationSettings.AppSettings["GET_DATA_JOB"];
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                         .WithIdentity("trigger5", "group1")
                                         .WithCronSchedule(jobCronSetting)
                                         .Build();
            DateTimeOffset ft = sched.ScheduleJob(job, trigger);
            sched.Start();
        }

        private void doSearchRealTimeData(CasicSender casicSender)
        {
            Dictionary<String, String> devHubMaps = new Dictionary<String, String>();
            List<Model.DevHubInfo> devHubs = new BLL.DevHub().findAllRealTimeDT();
            foreach (Model.DevHubInfo devHub in devHubs)
            {
                devHubMaps.Add(devHub.DevCode, devHub.HubCode);
            }
            casicSender.SendGetRealTimeDataReq(devHubMaps);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            
            this.updateSequence();
            Dictionary<String, String> devHubMaps = new Dictionary<String, String>();
            List<Model.DevHubInfo> devHubs = new BLL.DevHub().findAll();
            foreach (Model.DevHubInfo devHub in devHubs)
            {
                devHubMaps.Add(devHub.DevCode, devHub.HubCode);
            }
            
            CasicSender casicSender = new CasicSender(this);

            double configItr2 = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CASIC_REALTIMEDATA_SER"]);
            SetInterval(configItr2, delegate
            {
                doSearchRealTimeData(casicSender);
            });
            //this.SchedualFsWakeUp();//定时去唤醒腐蚀渗漏设备lk
            //this.SchedualSlGetData();//定时去获取渗漏设备数据
           
           double configItr = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CASIC_CONFIGCHECK_INTR"]);
           SetInterval(configItr, delegate
           {
               doSendConfig(casicSender, devHubMaps);
           });

           //等待集中器一会
           double configItr1 = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CASIC_WAIT_CONCERNTRATOR"]);
           SetInterval2(configItr1, delegate
           {
               timer2.Enabled = false;
               doSendConfig(casicSender,devHubMaps);
           });
        }

        private void doSendConfig(CasicSender casicSender, Dictionary<String, String> devHubMaps)
        {
            //液位监测仪 000034
            List<Model.DeviceConfigInfo> ywConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000034");
            List<Model.DeviceConfigInfo> validYwConfigs = new BLL.DeviceConfig().removeTimeoutConfig(ywConfigs);
            if (validYwConfigs.Count != 0)
            {
                casicSender.sendYWConfig(validYwConfigs,devHubMaps);
            }

            //噪声记录仪 000032
            List<Model.DeviceConfigInfo> slConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000032");
            List<Model.DeviceConfigInfo> validSlConfigs = new BLL.DeviceConfig().removeTimeoutConfig(slConfigs);
            if (validSlConfigs.Count != 0)
            {
                casicSender.sendSLConfig(validSlConfigs, devHubMaps);
            }

            //燃气智能监测终端
            List<Model.DeviceConfigInfo> rqConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000044");
            List<Model.DeviceConfigInfo> validRqConfigs = new BLL.DeviceConfig().removeTimeoutConfig(rqConfigs);
            if (validRqConfigs.Count != 0)
            {
                casicSender.sendRqConfig(validRqConfigs, devHubMaps);
            }
            
            //温度压力监测仪:
            List<Model.DeviceConfigInfo> tempAndPressConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000050");
            List<Model.DeviceConfigInfo> validTempAndPressConfigs = new BLL.DeviceConfig().removeTimeoutConfig(tempAndPressConfigs);
            if (validTempAndPressConfigs.Count != 0)
            {
                casicSender.sendTempAndPressConfig(validTempAndPressConfigs, devHubMaps);
            }

            //远传水表:
            List<Model.DeviceConfigInfo> waterMeterConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000052");
            List<Model.DeviceConfigInfo> validWaterMeterConfigs = new BLL.DeviceConfig().removeTimeoutConfig(waterMeterConfigs);
            if (validWaterMeterConfigs.Count != 0)
            {
                casicSender.sendWaterMeterConfig(validWaterMeterConfigs, devHubMaps);
            }
            /*
            List<Model.DeviceConfigInfo> fsslConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000040");
            List<Model.DeviceConfigInfo> fshjConfigs = new BLL.DeviceConfig().getDeviceConfigBySensorCode("000041");

            List<Model.DeviceConfigInfo> validSlConfigs = new BLL.DeviceConfig().removeTimeoutConfig(slConfigs);
            List<Model.DeviceConfigInfo> validFSSLConfigs = new BLL.DeviceConfig().removeTimeoutConfig(fsslConfigs);
            List<Model.DeviceConfigInfo> validFSHJConfigs = new BLL.DeviceConfig().removeTimeoutConfig(fshjConfigs);

            //发送噪声配置信息
            if (validSlConfigs.Count != 0)
            {
                casicSender.sendSLConfig(validSlConfigs, devHubMaps);
            }

            //保温层下腐蚀速率监测仪
            if (validFSSLConfigs.Count != 0)
            {
                casicSender.sendFSSLConfig(validFSSLConfigs, devHubMaps);
            }


            //保温层下腐蚀环境参数监测仪
            if (validFSHJConfigs.Count != 0)
            {
                casicSender.sendFSHJConfig(validFSHJConfigs, devHubMaps);
            }**/
        }
        /// <summary> 
        /// 在指定时间周期重复执行指定的表达式 
        /// </summary> 
        /// <param name="interval">事件之间经过的时间（以毫秒为单位）</param> 
        /// <param name="action">要执行的表达式</param> 
        public static void SetInterval(double interval, Action<ElapsedEventArgs> action)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                action(e);
            };
            timer.Enabled = true;
        }

        System.Timers.Timer timer2;
        public void SetInterval2(double interval, Action<ElapsedEventArgs> action)
        {
            timer2 = new System.Timers.Timer(interval);
            timer2.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                action(e);
            };
            timer2.Enabled = true;
        }

    }
}
