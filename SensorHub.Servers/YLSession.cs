using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Utility;
using System.Timers;

namespace SensorHub.Servers
{
    public class YLSession : AppSession<YLSession, BinaryRequestInfo>
    {
        /**
           * 集中器的ID信息，代表雨量计的ID信息
           * **/
        private String deviceID;

        public String DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        private String hubID;
        public String HubID
        {
            get { return hubID; }
            set { hubID = value; }
        }

        private int yuLiangValue;

        public int YuLiangValue
        {
            get { return yuLiangValue; }
            set { yuLiangValue = value; }
        }

        private bool isFirstTime = true;

        public bool IsFirstTime
        {
            get { return isFirstTime; }
            set { isFirstTime = value; }
        }

        protected override void OnSessionStarted()
        {
            //TODO LIST:delete the following code
            byte[] set00 = ApplicationContext.getInstance().getUpdateTime();
            /*
            byte[] mockeSet = {0x48,
                              0x43,
                              0x00,0x6E,
                              0x00,0x00,
                              0x05,
                              0x10,0x01,0x01,0x17,0x3A,0x01,
                              0x00,0x00,
                              0xAF};
             * **/
            byte[] sendCode2 = CodeUtils.yl_addCrc(set00);
            this.Send(sendCode2, 0, sendCode2.Length);
            this.Logger.Info("服务器->设备：校时信息:" + BitConverter.ToString(sendCode2));


            string YLInterval = "YL_GetYLDatat_Interval";
            double ylInterval = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings[YLInterval]);
            SetInterval(ylInterval, delegate
            {
                //判断是否已经保存设备信息
                /*
                if (deviceID != null)
                {
                    byte[] set = ApplicationContext.getInstance().getMinuteYuliangCount();
                    set[2] = byte.Parse(deviceID.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    set[3] = byte.Parse(deviceID.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    byte[] sendCode = CodeUtils.yl_addCrc(set);
                    this.Send(sendCode, 0, sendCode.Length);
                    this.Logger.Info("雨量计请求配置信息帧" + BitConverter.ToString(sendCode));
                }
                 * **/
                String sdeviceID = "006E";
                byte[] set = ApplicationContext.getInstance().getMinuteYuliangCount();
                set[2] = byte.Parse(sdeviceID.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                set[3] = byte.Parse(sdeviceID.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte[] sendCode = CodeUtils.yl_addCrc(set);
                this.Send(sendCode, 0, sendCode.Length);
                this.Logger.Info("雨量计请求配置信息帧" + BitConverter.ToString(sendCode));

            });


        }


        /// <summary> 
        /// 在指定时间过后执行指定的表达式 
        /// </summary> 
        /// <param name="interval">事件之间经过的时间（以毫秒为单位）</param> 
        /// <param name="action">要执行的表达式</param> 
        public static void SetTimeout(double interval, Action action)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Enabled = false;
                action();
            };
            timer.Enabled = true;
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

    }
}

/*定时器方法
System.Timers.Timer t = new System.Timers.Timer(30000);
 t.Elapsed += new System.Timers.ElapsedEventHandler(SendYuLiangData);
 t.AutoReset = true;
 t.Enabled = true;
 
  
  
       public void SendYuLiangData(object source, System.Timers.ElapsedEventArgs e)
       {
           if (deviceID != null)
           {
               byte[] set = ApplicationContext.getInstance().getMinuteYuliangCount();
               set[2] = byte.Parse(deviceID.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
               set[3] = byte.Parse(deviceID.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
               byte[] sendCode = CodeUtils.yl_addCrc(set);
               this.Send(sendCode, 0, sendCode.Length);
           }
       }
 */

