using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class SystemTimeTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return tag is SystemTimeTag?true:false;
        }

        public override void execute(Tag tag,String devCode,CellTag cellTag,
             SystemDateTag systemDateTag,CasicSession session)
        {
         SystemTimeTag sysTag = tag as SystemTimeTag;

            session.Logger.Info("系统时间TAG：oid：" + sysTag.Oid +
           "系统时间：" + sysTag.SysTime );
        }
    }
}
