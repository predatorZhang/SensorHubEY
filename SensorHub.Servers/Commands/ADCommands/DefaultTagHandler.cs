using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public class DefaultTagHandler: TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return true;
        }

        //TODO LIST:默认打印出来未处理的tag信息
        public override void execute(Tag tag)
        {
            switch(tag.Oid)
            {
                case "10000022":
                    //TODO LIST:处理ip的逻辑
                    String ad_ip = getInfoFromStrEncoding(tag);
                    AdlerCmd.adlerSession.Logger.Info("服务器IP TAG" + " oid:" + tag.Oid + " len:" + tag.Len + " value:"+tag.DataValue+" IP:" + ad_ip);
                    break;
                case "10000023":
                    //TODO LIST:处理端口的逻
                    String ad_port = getInfoFromStrEncoding(tag);
                    AdlerCmd.adlerSession.Logger.Info("服务器端口 TAG" + " oid:" + tag.Oid + " len:" + tag.Len + " value"+tag.DataValue+" PORT:" + ad_port);
                    break;
                default:
                    AdlerCmd.adlerSession.Logger.Info("未处理TAG" + " oid:" + tag.Oid + " len:" + tag.Len + " value:" + tag.DataValue + "ascii:" + getInfoFromStrEncoding(tag));
                break;
            }
        }
























        //字符串最后结尾追加了一个00,解析的时候需要去除掉
       // 服务器IP TAG oid:10000022 len:15 value:3131392E3235342E3130332E383000 IP:119.254.103.80
       //服务器端口 TAG oid:10000023 len:5 value3230313600 PORT:2016
        private String getInfoFromStrEncoding(Tag tag)
        {
            byte[] src = new byte[tag.Len-1];
            int index = 0;
            for (int j = 0; index < tag.Len-1; )
            {
                src[index++] = byte.Parse(tag.DataValue.Substring(j, 2), System.Globalization.NumberStyles.HexNumber);
                j = j + 2;
            }
            return  Encoding.ASCII.GetString(src, 0, src.Length);
        }
    }
}
