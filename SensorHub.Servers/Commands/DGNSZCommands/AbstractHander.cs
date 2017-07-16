using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace SensorHub.Servers.Commands.DGNSZCommands
{
    public abstract class AbstractHander
    {
    
        private AbstractHander nextHandler;

        public AbstractHander NextHandler
        {
            set { nextHandler = value; }
        }

        public void handleMessage(DGNSZSession session,StringRequestInfo requestInfo)
        {
            if (isThisLevel(requestInfo))
            {
                handleSection(session);
            }
            else
            {
                if (nextHandler != null)
                {
                    nextHandler.handleMessage(session, requestInfo);
                }
            }
        }
        public abstract bool isThisLevel(StringRequestInfo requestInfo);
        public abstract void handleSection(DGNSZSession session);

    }
}
