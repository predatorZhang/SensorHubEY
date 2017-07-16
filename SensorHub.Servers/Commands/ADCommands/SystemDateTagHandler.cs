using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public class SystemDateTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return tag is SystemDateTag?true:false;
        }

        public override void execute(Tag tag)
        {
            //更新系统日期
            SystemDateTag systemDateTag = tag as SystemDateTag;
            AdlerCmd.currentSystemDate = systemDateTag.CollectDate;

            AdlerCmd.adlerSession.Logger.Info("系统日期TAG：oid：" + systemDateTag.Oid +
           "系统日期：" + systemDateTag.CollectDate);

        }
    }
}
