using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SystemDateTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return tag is SystemDateTag?true:false;
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag, 
            SystemDateTag systemDateTag0,CasicSession session)
        {
            //更新系统日期
            SystemDateTag systemDateTag = tag as SystemDateTag;
            //CasicCmd.currentSystemDate = systemDateTag.CollectDate;

            session.Logger.Info("系统日期TAG：oid：" + systemDateTag.Oid +
           "系统日期：" + systemDateTag.CollectDate);

        }
    }
}
