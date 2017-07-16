using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SensorException0TagHandler : TagHandler
    {
        private Int16 getPduType(String devCode)
        {
            String devType = devCode.Substring(0, 2);
            if (devType == "11")
            {
                //液位监测仪
                return 7;
            }
            else if (devType == "21")
            {
                //噪声记录仪
                return 2;
            }
            else if (devType == "31")
            {
                //燃气智能监测终端
                return 4;
            }
            else if (devType == "41")
            {
                //井盖传感器
                return 6;

            }
            else if (devType == "52")
            {
                //远传水表
                return 10;

            }
            else if (devType == "81")
            {
                //温度压力监测仪
                return 8;
            }
            else if (devType == "91")
            {
                //腐蚀速率监测仪
                return 11;
            }
            else if (devType == "92")
            {
                //腐蚀速率监测仪
                return 12;
            }
            else
            {
                return 0;
            }
        }

        public override bool isThisTag(Tag tag)
        {
            return tag is SensorException0Tag?true:false;
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag, 
            SystemDateTag systemDateTag0,CasicSession session)
        {
            SensorException0Tag sensorException0 = tag as SensorException0Tag;
            String state = sensorException0.state;
            int type = getPduType(devCode);//获取设备类型
            //todo list:根据设备类型来进行存储

            switch (type)
            {
              
                case 7:
                    //液位监测仪 探测0
                    new BLL.DjLiquid().setDeviceStatus(devCode, state);
                    break;
                case 2:
                    //噪声记录仪
                    new BLL.SlNoise().setDeviceStatus(devCode, state);
                    break;
                default:
                    break;
            }
        }
    }
}
