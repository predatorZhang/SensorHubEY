using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public abstract class TagHandler
    {
        private TagHandler nextHandler;

        public TagHandler NextHandler
        {
            get { return nextHandler; }
            set { nextHandler = value; }
        }

        public void handleTag(Tag tag, String devCode, CellTag cellTag, 
            SystemDateTag systemDateTag,CasicSession session)
        {
            if (this.isThisTag(tag))
            {
                //处理当前逻辑
                this.execute(tag, devCode, cellTag, systemDateTag, session);
            }
            else
            {
                if (nextHandler != null)
                {
                    nextHandler.handleTag(tag, devCode, cellTag, systemDateTag,
                        session);
                }
                else
                {
                    //TODO LIST：用默认的TagHandler来处理
                    nextHandler = new DefaultTagHandler();
                    nextHandler.execute(tag, devCode, cellTag, systemDateTag,
                        session);
                }
            }
        }

        public abstract bool isThisTag(Tag tag);

        public abstract void execute(Tag tag, String devCode, CellTag cellTag,
            SystemDateTag systemDateTag, CasicSession session);
    }
}
