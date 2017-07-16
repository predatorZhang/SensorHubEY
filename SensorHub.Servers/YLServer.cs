using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Servers.Commands;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SensorHub.Utility;
namespace SensorHub.Servers
{
    //二院：雨量计
    public class YLServer : AppServer<YLSession, BinaryRequestInfo>
    {

        public YLServer()
            : base(new DefaultReceiveFilterFactory<YLCmdFilter,BinaryRequestInfo>())
        {
            //实现一个自定义内置的协议 默认的客户端请求类型是stringRequestInfo
            //使用这个接受过滤器 使用默认的接收过滤器工厂
        }
       
        protected override void OnStarted()
        {
            base.OnStarted();//启动注册的会话
            
            //TODO LIST：删除雨量sequence，获取雨量表最大数值，更新sequence
            ApplicationContext.getInstance().updateSequence("SEQ_AD_YL_YLIANG_ID", "DBID", "AD_YL_YLIANG");
        }
        
    }
}

 