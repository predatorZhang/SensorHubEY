using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
namespace SensorHub.Servers.Commands.CASICCommands
{
    public class HeartBeatTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            if (!(tag is UploadTag))
            {
                return false;
            }

            UploadTag uploadTag = tag as UploadTag;

            return uploadTag.BizType==6?true:false;
        
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag,
            SystemDateTag systemDateTag, CasicSession session)
        {
            //TODO LIST：处理对应的心跳上传数据
            UploadTag heartBeatTag = tag as UploadTag;
            int itv = heartBeatTag.CollectInter;
            String collecTime = heartBeatTag.CollectTime;
            int len = heartBeatTag.Len;
            String dataValue = heartBeatTag.DataValue;

            session.Logger.Info("心跳上传TAG：oid：" + heartBeatTag.Oid + " 采集间隔: " +
                itv + "采集时间：" + collecTime + "上传数值：" + dataValue);
        }

    }
}
