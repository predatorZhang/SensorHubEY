using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class CellTagHandler : TagHandler
    {
        public override bool isThisTag(Tag tag)
        {
            return tag is CellTag?true:false;
        }

        public override void execute(Tag tag, String devCode, CellTag cellTag, 
            SystemDateTag systemDateTag,CasicSession session)
        {
            CellTag slCellTag = tag as CellTag;
          //  CasicCmd.cell = slCellTag.Cell; 
           
        }
    }
}
