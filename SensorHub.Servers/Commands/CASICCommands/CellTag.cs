using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public class CellTag:Tag
    {
      
        private String cell; //是否已经唤醒

        public static String CELL_TAG_OID = "60000020";

        public CellTag()
        {
 
        }

        public CellTag(String oid, int len, String dataValue)
            : base(oid, len, dataValue)
        {
            byte btCell = byte.Parse(dataValue, System.Globalization.NumberStyles.HexNumber);
            cell = btCell + "";
        }

        public String Cell
        {
            get { return cell; }
            set { cell = value; }
        }

    }
}
