using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class WakeUpTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return tag is WakeUpTag?true:false;
        }

        public override void execute(Tag tag,String devCode,CellTag cellTag,
            SystemDateTag systemDateTag,CasicSession session)
        {
            WakeUpTag wakeUpTag = tag as WakeUpTag;
            if (session.devMaps.ContainsKey(devCode))
            {

                CasicSession.DeviceDTO dev = session.devMaps[devCode];

                byte flag = byte.Parse(wakeUpTag.DataValue, System.Globalization.NumberStyles.HexNumber);
                
                dev.IsWakeUp = flag==0?true:false; //上传0代表已经唤醒
                session.devMaps[devCode] = dev;
                session.Logger.Info("设备唤醒" + devCode);

                session.Logger.Info("唤醒TAG：oid：" + wakeUpTag.Oid +
        "唤醒状态：" + wakeUpTag.IsWAkeUp);

            }
        }
    }
}
