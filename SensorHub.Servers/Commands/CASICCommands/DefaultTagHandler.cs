using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class DefaultTagHandler: TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return true;
        }

        //TODO LIST:默认打印出来未处理的tag信息
        public override void execute(Tag tag, String devCode, CellTag cellTag,
            SystemDateTag systemDateTag, CasicSession session)
        {
            switch(tag.Oid)
            {
                case "10000022":
                    //TODO LIST:处理ip的逻辑
                    String ad_ip = getInfoFromStrEncoding(tag);
                    session.Logger.Info("服务器IP TAG" + " oid:" + tag.Oid + " len:" + tag.Len + " value:" + tag.DataValue + " IP:" + ad_ip);
                    break;
                case "10000023":
                    //TODO LIST:处理端口的逻
                    String ad_port = getInfoFromStrEncoding(tag);
                    session.Logger.Info("服务器端口 TAG" + " oid:" + tag.Oid + " len:" + tag.Len + " value" + tag.DataValue + " PORT:" + ad_port);
                    break;
                default:
                    session.Logger.Info("未处理TAG" + " oid:" + tag.Oid + " len:" + tag.Len + " value:" + tag.DataValue);
                break;
            }

        }

        //字符串最后结尾追加了一个
        private String getInfoFromStrEncoding(Tag tag)
        {
            byte[] src = new byte[tag.Len-1];
            for (int j = 0; j < tag.Len-1; )
            {
                src[j] = byte.Parse(tag.DataValue.Substring(j, 2), System.Globalization.NumberStyles.HexNumber);
                j = j + 2;
            }
            return  Encoding.ASCII.GetString(src, 0, src.Length);
        }
    }
}
